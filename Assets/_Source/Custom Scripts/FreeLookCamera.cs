using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLookCamera : MonoBehaviour {

    [SerializeField] float cameraMoveSpeed = 120f;
    [SerializeField] float clampAngle = 80f;
    [SerializeField] float inputSensitivity = 150f;
    [SerializeField] float minFov = 15f;
    [SerializeField] float maxFov = 90f;
    [SerializeField] float fovSensitivity = 10f;
    [SerializeField] GameObject cameraFollowObject;

    Vector3 followPosition;
    private float rotationY = 0f;
    private float rotationX = 0f;
    private float mouseX;
    private float mouseY;
    private float finalInputX;
    private float finalInputZ;
    bool rightClicked = false;


    // Use this for initialization
    void Start () {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotationY = rot.y;
        rotationX = rot.x;

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            rightClicked = true;
            Cursor.visible = false;
        }
        if(Input.GetMouseButtonUp(1))
        {
            rightClicked = false;
            Cursor.visible = true;
        }
        var fov = Camera.main.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * fovSensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;
    }

    void LateUpdate()
    {
        // set up controller input
        float inputX = Input.GetAxis("RightStickHorizontal");
        float inputZ = Input.GetAxis("RightStickVertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = inputX - mouseX;
        finalInputZ = inputZ - mouseY;
        rotationY += finalInputX * inputSensitivity * Time.deltaTime;
        rotationX += finalInputZ * inputSensitivity * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -clampAngle, +clampAngle);

        if (rightClicked)
        {
            transform.RotateAround(cameraFollowObject.transform.position, Vector3.up, mouseX * 10f);
            transform.RotateAround(cameraFollowObject.transform.position, Vector3.up, mouseY * 10f);
        }
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
