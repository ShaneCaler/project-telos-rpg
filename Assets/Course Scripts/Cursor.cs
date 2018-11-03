using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

    CameraRaycaster cameraRayCaster;


	// Use this for initialization
	void Start () {
        cameraRayCaster = FindObjectOfType<CameraRaycaster>();
	}


	// Update is called once per frame
	void Update () {
        //if (Input.GetMouseButtonDown(0))
        //{
        //   print(cameraRayCaster.layerHit);
        //}
    }
}
