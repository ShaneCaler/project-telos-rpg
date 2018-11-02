using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLookCamera : MonoBehaviour {

    [SerializeField] float cameraMoveSpeed = 120f;
    [SerializeField] float clampAngle = 80f;
    [SerializeField] float inputSensitivity = 150f;
    [SerializeField] float camDistanceXtoPlayer;
    [SerializeField] float camDistanceYtoPlayer;
    [SerializeField] float camDistanceZtoPlayer;
    [SerializeField] float mouseX;
    [SerializeField] float mouseY;
    [SerializeField] float finalInputX;
    [SerializeField] float finalInputZ;
    [SerializeField] float smoothX;
    [SerializeField] float smoothY;

    [SerializeField] GameObject cameraObject;
    [SerializeField] GameObject playerObject;
    [SerializeField] GameObject cameraFollowObject;

    Vector3 followPosition;
    private float rotationY = 0f;
    private float rotationX = 0f;


    // Use this for initialization
    void Start () {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotationY = rot.y;
        rotationX = rot.x;

	}
	
	// Update is called once per frame
	void Update () {
        // set up controller input
        float inputX = Input.GetAxis("RightStickHorizontal");
        float inputZ = Input.GetAxis("RightStickVertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = inputX - mouseX;
        finalInputZ = inputZ - mouseY;
        if (Input.GetMouseButtonDown(0))
        {
            rotationY += finalInputX * inputSensitivity * Time.deltaTime;
            rotationX += finalInputZ * inputSensitivity * Time.deltaTime;

            rotationX = Mathf.Clamp(rotationX, -clampAngle, +clampAngle);

            Quaternion localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
            transform.rotation = localRotation;
        }

    }

    void LateUpdate()
    {
            CameraUpdater();

    }

    void CameraUpdater()
    {
        Transform target = cameraFollowObject.transform;

        // move toward target game object
        float step = cameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
