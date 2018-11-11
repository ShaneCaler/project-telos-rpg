using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeModeText : MonoBehaviour {

    CameraCollision cameraCollisionRef;
    Text modeText;
	// Use this for initialization
	void Start () {
        cameraCollisionRef = FindObjectOfType<CameraCollision>();
        modeText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if(cameraCollisionRef.inCTMview)
        {
            modeText.text = "Press G to enter Direct control mode";
        }
        else
        {
            modeText.text = "Press G to enter Click-To-Move control mode";
        }
    }
}
