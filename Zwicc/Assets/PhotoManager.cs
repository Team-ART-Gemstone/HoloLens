using HoloToolkit.Unity;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA.WebCam;

#if UNITY_UWP
using Windows.Storage;
#endif

public class PhotoManager : MonoBehaviour
{
    private TextMesh _statusText;
    private PhotoCapture _capture;
    private TextToSpeech _textToSpeechComponent;
    private bool _isCameraReady = false;
    private string _currentImagePath;
    private string _pictureFolderPath;
   

#if UNITY_UWP
    private CognitiveServicesVisionLibrary.CognitiveVisionHelper _cognitiveHelper;
#endif
    private void Start()
    {
        _statusText = GetComponent<TextMesh>();
        _textToSpeechComponent = GetComponent<TextToSpeech>();

#if UNITY_UWP
        _cognitiveHelper = new CognitiveServicesVisionLibrary.CognitiveVisionHelper();
#endif
        StartCamera();
    }

#if UNITY_UWP
    private async void getPicturesFolderAsync() {
        StorageLibrary picturesStorage = await StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Pictures);
        _pictureFolderPath = picturesStorage.SaveFolder.Path;
    }
#endif

    private void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        _capture = captureObject;

        Resolution resolution = PhotoCapture.SupportedResolutions.OrderByDescending(res => res.width * res.height).First();

        var camera = new CameraParameters(WebCamMode.PhotoMode)
        {
            hologramOpacity = 1.0f,
            cameraResolutionWidth = resolution.width,
            cameraResolutionHeight = resolution.height,
            pixelFormat = CapturePixelFormat.BGRA32
        };

        _capture.StartPhotoModeAsync(camera, OnPhotoModeStarted);
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        _isCameraReady = result.success;
        SetStatus("Camera ready. Say 'Describe' to start");
    }

    public void TakePhoto()
    {
        if (_isCameraReady)
        {
            var fileName = string.Format(@"Image_{0:yyyy-MM-dd_hh-mm-ss-tt}.jpg", DateTime.Now);
            _currentImagePath = Application.persistentDataPath + "/" + fileName;

            _capture.TakePhotoAsync(_currentImagePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
        }
        else
        {
            SetStatus("The camera is not yet ready.");
        }
    }

    public void StartCamera()
    {
        PhotoCapture.CreateAsync(true, OnPhotoCaptureCreated);

#if UNITY_UWP
        getPicturesFolderAsync();
#endif
    }
    //this takes a filename/path and turns it into a 2dtexture that is returned
    //trying to display that texture on the hololens
    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

    private async void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {

            /*
            photoCaptureFrame.UploadImageDataToTexture(fakeTexture);
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Renderer quadRenderer = quad.GetComponent<Renderer>() as Renderer;
            quadRenderer.material = new Material(Shader.Find("Custom/Unlit/UnlitTexture"));

            quad.transform.parent = this.transform;
            quad.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            quadRenderer.material.SetTexture("_MainTex", fakeTexture);
            */
            //the texture2d crap is to try and make a quad that displas the picture.
#if UNITY_UWP
            try 
            {
                SetStatus("1");
                if (_pictureFolderPath != null)
                {
                    SetStatus("2");
                    var newFile = System.IO.Path.Combine(_pictureFolderPath, "Camera Roll", 
                        System.IO.Path.GetFileName(_currentImagePath));

                    SetStatus("3");
                    if (System.IO.File.Exists(newFile))
                    {
                        System.IO.File.Delete(newFile);
                    }
                    SetStatus("4");
                    System.IO.File.Move(_currentImagePath, newFile);
                    SetStatus("5");
                    var storageFile = await StorageFile.GetFileFromPathAsync(newFile);
                    SetStatus("Analysing picture...");
                    var visionResult = await _cognitiveHelper.start(storageFile);
                    //SetStatus("WE ARE HERE");
                    var description = _cognitiveHelper.ExtractOcr(visionResult);
                    _statusText.text = description;

        
                    Texture2D tex = LoadPNG(_currentImagePath)
                    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    Renderer quadRenderer = quad.GetComponent<Renderer>() as Renderer;
                    quadRenderer.material = new Material(Shader.Find("Custom/Unlit/UnlitTexture"));
                    quad.transform.parent = this.transform;
                    quad.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                    quadRenderer.material.SetTexture("_MainTex", fakeTexture);

                    Speak(description);
                }
            } 
            catch(Exception e) 
            {
                SetStatus(e.Message);
            }
#endif
        }
        else
        {
            SetStatus("Failed to save photo");
        }
    }

    private void SetStatus(string statusText)
    {
        _statusText.text = statusText;
        Speak(statusText);
    }

    private void Speak(string description)
    {
        _textToSpeechComponent.StartSpeaking(description);
    }

    private void OnPhotoModeStopped(PhotoCapture.PhotoCaptureResult result)
    {
        _capture.Dispose();
        _capture = null;
        _isCameraReady = false;

        SetStatus("Camera off");
    }
}
