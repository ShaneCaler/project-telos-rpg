using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour {

    [SerializeField] float yRotationAmount = -170f;

	// Use this for initialization
	void Start () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        print("collided");
        transform.RotateAround(transform.position, transform.up, yRotationAmount);
    }
    // Update is called once per frame
    void OntriggerEnter () {
        print("triggered");
        transform.RotateAround(transform.position, transform.up, yRotationAmount);
    }
}
