using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackMoveStopRadius = 5f;
    [SerializeField] float rotateVelocity = 100f;

    bool isInDirectMode = true; // false is point-to-click mode
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination;
    Vector3 clickPoint;
    Quaternion targetRotation;
    float turnInput;

    private void Start()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
        targetRotation = transform.rotation;
        turnInput = 0;
    }

    public Quaternion GetTargetRotation
    {
        get { return targetRotation; }
    }

    private void Update()
    {
        Turn();
    }

    void Turn()
    {
        turnInput = Input.GetAxis("Horizontal");
        targetRotation *= Quaternion.AngleAxis(rotateVelocity * turnInput * Time.deltaTime, Vector3.up);
        transform.rotation = targetRotation;
    }

    private void FixedUpdate()
    {
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    isInDirectMode = !isInDirectMode; // toggle mode
        //    currentDestination = transform.position;
        //}

        if (isInDirectMode)
        {
            ProcessDirectMovement();
        }
        else
        {
           // ProcessMouseMovement();
        }
    }

    private void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool crouch = Input.GetButton("Crouch");
        bool jump = false;

        // calculate camera relative direction to move:
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * camForward + h * Camera.main.transform.right;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        thirdPersonCharacter.Move(movement, crouch, jump);
    }

    private void ProcessMouseMovement()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    clickPoint = cameraRaycaster.hit.point;
        //    print("Cursor raycast hit " + cameraRaycaster.currentLayerHit);
        //    switch (cameraRaycaster.currentLayerHit)
        //    {
        //        case Layer.Walkable:
        //            currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
        //            break;
        //        case Layer.Enemy:
        //            currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
        //            break;
        //        default:
        //            print("shouldnt be here");
        //            return;
        //    }
        //}

        //WalkToDestination();
    }

    private float distanceToTargetAtPlayerLevel(Vector3 target)
    {
        target.y = transform.position.y;

        return Vector3.Distance(transform.position, target);
    }

    private void WalkToDestination()
    {
        var playerToClickPoint = currentDestination - transform.position;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius)
        {
            thirdPersonCharacter.Move(playerToClickPoint, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.1f);
        Gizmos.DrawSphere(clickPoint, .15f);

        // draw attach sphere
        Gizmos.color = new Color(255f, 0f, 0f, .5f);
        Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
    }
}

