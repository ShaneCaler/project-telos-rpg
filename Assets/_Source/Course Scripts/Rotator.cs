using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    [SerializeField] float xRotationsPerMinute = 1f;
    [SerializeField] float yRotationsPerMinute = 1f;
    [SerializeField] float zRotationsPerMinute = 1f;
	
	// Update is called once per frame
	void Update () {
        // to work regardless of the games current framerate, we need to use Time.deltaTime
        // xDeg reesPerFrame = Time.deltaTime (also written as seconds frame^-1), 60 seconds in 1 min, 360 degrees for inner rotation, xRotationsPerMinute
        // using degrees frame^-1 rather than degrees / frame

        float xDegreesPerFrame = Time.deltaTime / 60 * 360 * xRotationsPerMinute;
        transform.RotateAround(transform.position, transform.right, xDegreesPerFrame);

        float yDegreesPerFrame = Time.deltaTime / 60 * 360 * yRotationsPerMinute;
        transform.RotateAround(transform.position, transform.up, yDegreesPerFrame);

        float zDegreesPerFrame = Time.deltaTime / 60 * 360 * zRotationsPerMinute;
        transform.RotateAround(transform.position, transform.up, zDegreesPerFrame);
    }
}
