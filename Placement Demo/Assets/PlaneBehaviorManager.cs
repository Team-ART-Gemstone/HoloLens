using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBehaviorManager : MonoBehaviour {

    // Alternate texture to change to
    public Texture alternateTexture;

    public void MoveCloser()
    {
        gameObject.GetComponent<HoloToolkit.Unity.SpatialMapping.TapToPlace>().DefaultGazeDistance *= (float)(2.0 / 3.0);
    }

    public void MoveFarther()
    {
        gameObject.GetComponent<HoloToolkit.Unity.SpatialMapping.TapToPlace>().DefaultGazeDistance *= (float)1.5;
    }

    public void ChangeTexture()
    {
        // Collect current texture to be changed
        Texture mainTexture = gameObject.GetComponent<Renderer>().material.GetTexture("_MainTex");
        
        // Change texture
        gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", alternateTexture);

        // Swap storage locations of textures
        Texture temp;
        temp = mainTexture;
        mainTexture = alternateTexture;
        alternateTexture = temp;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
