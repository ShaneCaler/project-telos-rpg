using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float rotateVelocity = 100f;
    [SerializeField] float strafeSpeed = 10f;
    [SerializeField] float runSpeed = 1f;
    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    // Vector3 currentDestination;
    // Vector3 clickPoint;
    Quaternion targetRotation;
    float turnInput = 0;

    private void Start()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        // currentDestination = transform.position;
        targetRotation = transform.rotation;

        cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
    }

    public Quaternion GetTargetRotation
    {
        get { return targetRotation; }
    }

    void FixedUpdate()
    {
        ProcessDirectMovement();
        Turn();
    }

    void Turn()
    {
        turnInput = Input.GetAxis("Horizontal");
        targetRotation *= Quaternion.AngleAxis(rotateVelocity * turnInput * Time.deltaTime, Vector3.up);
        transform.rotation = targetRotation;    
    }

    void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
    {
        switch (layerHit)
        {
            case enemyLayerNumber:
                GameObject enemy = raycastHit.collider.gameObject;

                break;
            case walkableLayerNumber:
                break;
            default:
                Debug.LogError("Unknown layer clicked");
                return;
        }
    }

    private void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal") * runSpeed * Time.deltaTime;
        float v = Input.GetAxis("Vertical");
        float strafe = Input.GetAxis("Strafe") * strafeSpeed * Time.deltaTime;
        bool crouch = Input.GetButton("Crouch");
        bool jump = false;

        // calculate camera relative direction to move:
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * camForward + h * Camera.main.transform.right;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        // todo change strafe to A and D, only when right mouse is clicked, otherwise Q and E
        transform.Translate(strafe, 0f, 0f);
        thirdPersonCharacter.Move(movement, crouch, jump);
    }
}

