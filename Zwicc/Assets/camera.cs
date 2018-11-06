using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour {

    WebCamTexture webcam;
	// Use this for initialization
	void Start () {
        webcam = new WebCamTexture();
        webcam.Play();

	}
	
    public Texture2D TakePhoto()
    {
        Texture2D webcamImage = new Texture2D(webcam.width, webcam.height);
        webcamImage.SetPixels(webcam.GetPixels());
        webcamImage.Apply();

        return webcamImage;
    }

    public void TakePhotoToPreview(Renderer preview)
    {
        Texture2D image = TakePhoto();
        preview.material.mainTexture = image;

        float aspectRatio = (float)image.width / (float)image.height;
        Vector3 scale = preview.transform.localScale;
        scale.x = scale.y * aspectRatio;
        preview.transform.localScale = scale;
    }
}
