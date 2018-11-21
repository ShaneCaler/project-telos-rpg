using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;

    [System.Serializable]
    public class PositionSettings
    {
        [SerializeField] public Vector3 targetPosOffset = new Vector3(0f, 3.4f, 0f);         // origin for offset
        [SerializeField] public float lookSmooth = 100f;
        [SerializeField] public float distanceFromTarget = -8f; // modified by zooming
        [SerializeField] public float zoomSmooth = 100f; // how fast to zoom into target
        [SerializeField] public float maxZoom = -2f;
        [SerializeField] public float minZoom = -15f;
        [SerializeField] public bool smoothFollow = true;
        [SerializeField] public float smooth = 0.05f;

        [HideInInspector] public float newDistance = -8; // set by zoom input
        [HideInInspector] public float adjustmentDistance = -8;
    }

    [System.Serializable]
    public class OrbitSettings
    {
        [SerializeField] public float xRotation = -20f;
        [SerializeField] public float yRotation = -180f;
        [SerializeField] public float maxXRotation = 25f;
        [SerializeField] public float minXRotation = -85f;
        [SerializeField] public float vOrbitSmooth = 150f;
        [SerializeField] public float hOrbitSmooth = 150f;
        [SerializeField] public float yOrbitSmooth = 0.5f; // test
    }

    [System.Serializable]
    public class InputSettings
    {
        [SerializeField] public string MOUSE_ORBIT = "MouseOrbit";
        [SerializeField] public string ORBIT_HORIZONTAL_SNAP = "OrbitHorizontalSnap";
        [SerializeField] public string ORBIT_HORIZONTAL = "OrbitHorizontal";
        [SerializeField] public string ORBIT_VERTICAL = "OrbitVertical";
        [SerializeField] public string ZOOM = "Mouse ScrollWheel";
    }

    [System.Serializable]
    public class DebugSettings
    {
        [SerializeField] public bool drawDesiredCollisionLines = true;
        [SerializeField] public bool drawAdjustedCollisionLines = true;
    }

    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();
    public DebugSettings debug = new DebugSettings();
    public CollisionHandler collision;

    Vector3 targetPos = Vector3.zero;
    Vector3 destination = Vector3.zero;
    Vector3 adjustedDestination = Vector3.zero; // taking control from destination when colliding
    Vector3 camVel = Vector3.zero; // cameras velocity if using smoothing option
    PlayerMovement player;
    float vOrbitInput, hOrbitInput, zoomInput, hOrbitSnapInput, mouseOrbitInput;
    Vector3 previousMousePos = Vector3.zero;
    Vector3 currentMousePos = Vector3.zero;
    bool rightClicked = false;

    // Use this for initialization
    void Start () {
        SetCameraTarget(target);
        vOrbitInput = hOrbitInput = zoomInput = 0;
        collision = GetComponent<CollisionHandler>();
        collision.Initialize(Camera.main);
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desireCameraClipPoints); // camera position is desination here
        MoveToTarget();

        previousMousePos = currentMousePos = Input.mousePosition;
    }

    // use this to change camera target from other scripts
    public void SetCameraTarget(Transform tran)
    {
        target = tran;

        if(target != null)
        {
            if (target.GetComponent<PlayerMovement>())
            {
                player = target.GetComponent<PlayerMovement>();
            }
            else
            {
                Debug.LogError("Camera's target requires a character controller");
            }
        }
        else
        {
            Debug.LogError("Camera requires a target");
        }
    }

    void GetInput()
    {
        hOrbitInput = Input.GetAxis(input.ORBIT_HORIZONTAL);
        vOrbitInput = Input.GetAxis(input.ORBIT_VERTICAL);
        hOrbitSnapInput = Input.GetAxis(input.ORBIT_HORIZONTAL_SNAP);
        zoomInput = Input.GetAxis(input.ZOOM);
        mouseOrbitInput = Input.GetAxis("Mouse X");
    }

    void Update()
    {
        GetInput();
        if (Input.GetMouseButtonDown(1))
        {
            rightClicked = true;
            Cursor.visible = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            rightClicked = false;
            Cursor.visible = true;
        }
        MouseOrbitTarget();
        ZoomInOnTarget();
    }

    void FixedUpdate()
    {
        // moving
        MoveToTarget();
        // rotating
        LookAtTarget();
        // player input orbit
        OrbitTarget();
        
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desireCameraClipPoints);

        // draw the debug lines
        for(int i = 0; i < 5; i++) // loop through clip points
        {
            if (debug.drawDesiredCollisionLines)
            {
                Debug.DrawLine(targetPos, collision.desireCameraClipPoints[i], Color.white);
            }
            if (debug.drawAdjustedCollisionLines)
            {
                Debug.DrawLine(targetPos, collision.adjustedCameraClipPoints[i], Color.green);
            }
        }

        collision.CheckColliding(targetPos); // using raycasts here
        position.adjustmentDistance = collision.GetAdjustedDistanceWithRayFrom(targetPos);
    }

    void MoveToTarget()
    {
        targetPos = target.position + Vector3.up * position.targetPosOffset.y + Vector3.forward * position.targetPosOffset.z + transform.TransformDirection(Vector3.right * position.targetPosOffset.x); ;
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += targetPos;
        transform.position = destination;
        if (collision.colliding)
        {
            adjustedDestination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * Vector3.forward * position.adjustmentDistance;
            adjustedDestination += targetPos;
            if (position.smoothFollow)
            {
                // use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, adjustedDestination, ref camVel, position.smooth);
            }
            else
            {
                transform.position = adjustedDestination;
            }
        }
        else
        {
            if (position.smoothFollow)
            {
                // use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref camVel, position.smooth);
            }
            else
            {
                transform.position = destination;
            }
        }
    }

    void LookAtTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, position.lookSmooth * Time.deltaTime);

        player.transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

    }

    void OrbitTarget()
    {
        if (hOrbitSnapInput > 0)
        {
            orbit.yRotation = -180f; // place camera behind target
        }

        orbit.xRotation += vOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;
        orbit.yRotation += hOrbitInput * orbit.hOrbitSmooth * Time.deltaTime;


        if (orbit.xRotation > orbit.maxXRotation)
        {
            orbit.xRotation = orbit.maxXRotation;
        }
        if (orbit.xRotation < orbit.minXRotation)
        {
            orbit.xRotation = orbit.minXRotation;
        }

    }

    void ZoomInOnTarget()
    {
        position.distanceFromTarget += zoomInput * position.zoomSmooth * Time.deltaTime;
        if(position.distanceFromTarget > position.maxZoom)
        {
            position.distanceFromTarget = position.maxZoom;
        }
        if(position.distanceFromTarget < position.minZoom)
        {
            position.distanceFromTarget = position.minZoom;
        }
    }

    void MouseOrbitTarget()
    {
        previousMousePos = currentMousePos;
        currentMousePos = Input.mousePosition;
        if (rightClicked)
        {
            if (mouseOrbitInput != 0)
            {
                orbit.yRotation += (currentMousePos.x - previousMousePos.x) * orbit.yOrbitSmooth;
            }
        }
    }
}
