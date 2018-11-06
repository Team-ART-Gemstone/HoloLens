using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour {
    // Based on code in tutorial
    // https://docs.microsoft.com/en-us/windows/mixed-reality/voice-input-in-unity

    public Texture alternateTexture;
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    void Start () {
        Debug.Log("hello i made it here");
        // Collect current texture to be changed later
        Texture mainTexture = gameObject.GetComponent<Renderer>().material.GetTexture("_MainTex");

        KeywordRecognizer keywordRecognizer;

        // Say "closer" or "farther" to move attached GameObject correspondingly
        keywords.Add("closer", () =>
        {
            gameObject.GetComponent<HoloToolkit.Unity.SpatialMapping.TapToPlace>().DefaultGazeDistance *= (float) (2.0 / 3.0);
        });
        keywords.Add("farther", () =>
        {
            gameObject.GetComponent<HoloToolkit.Unity.SpatialMapping.TapToPlace>().DefaultGazeDistance *= (float) 1.5;
        });
        // Say change to change image displayed
        keywords.Add("change", () =>
        {
            gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", alternateTexture);
            Texture temp;
            temp = mainTexture;
            mainTexture = alternateTexture;
            alternateTexture = temp;
        });
        
        // Initialize keywordRecognizer using keywords specified above
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;

        // Start recognizing
        keywordRecognizer.Start();
        Debug.Log("i didnt crash on start");
    }


    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log(args.text);
        System.Action keywordAction;
            // if the keyword recognized is in our dictionary, call that Action.
            if (keywords.TryGetValue(args.text, out keywordAction))
            {
                keywordAction.Invoke();
            }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
