using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour {

    [SerializeField] LayerMask collisionLayer;
    [SerializeField] float collisionSpaceSize = 3.41f;
    [HideInInspector] public bool colliding = false;
    [HideInInspector] public Vector3[] adjustedCameraClipPoints;
    [HideInInspector] public Vector3[] desireCameraClipPoints;

    Camera cam;

    public void Initialize(Camera c)
    {
        cam = c;
        adjustedCameraClipPoints = new Vector3[5]; // 4 clip points on the nearClip plane and the cam position
        desireCameraClipPoints = new Vector3[5];
    }

    public void UpdateCameraClipPoints(Vector3 camPos, Quaternion atRotation, ref Vector3[] intoArray)
    {
        if (!cam)
            return;
        // clear contents of intoArray
        intoArray = new Vector3[5];
        float z = cam.nearClipPlane;
        float x = Mathf.Tan(cam.fieldOfView / collisionSpaceSize) * z;
        float y = x / cam.aspect;

        // find top left clipPoint
        intoArray[0] = (atRotation * new Vector3(-x, y, z)) + camPos; // added and rotated the point relative to camera
        // top right
        intoArray[1] = (atRotation * new Vector3(x, y, z)) + camPos;
        // bottom left
        intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + camPos;
        // bottom right
        intoArray[3] = (atRotation * new Vector3(x, y, z)) + camPos;
        // camera position
        intoArray[4] = camPos - cam.transform.forward;
    }

    bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
    {
        for(int i = 0; i < clipPoints.Length; i++)
        {
            Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition); 
            float distance = Vector3.Distance(clipPoints[i], fromPosition);
            if(Physics.Raycast(ray, distance, collisionLayer)) // if the ray hits a collision layer, return true
            {
                return true;
            }
        }
        return false;
    }


    public float GetAdjustedDistanceWithRayFrom(Vector3 from)
    {
        float adjustedDistance = -1;

        for(int i = 0; i < desireCameraClipPoints.Length; i++)
        {
            // find the shortest distance between clip point collisions
            Ray ray = new Ray(from, desireCameraClipPoints[i] - from);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit)) // if this is true, place the collision point info in the var hit
            {
                if (adjustedDistance == -1)
                {
                    adjustedDistance = hit.distance;
                }
                else
                {
                    if(hit.distance < adjustedDistance)
                    {
                        adjustedDistance = hit.distance;
                    }
                }
            }
        }

        // once we have the shortest distance, return the adjusted distance or 0
        if (adjustedDistance == -1)
            return 0;
        else
            return adjustedDistance;
    }

    public void CheckColliding(Vector3 targetPos)
    {
        if (CollisionDetectedAtClipPoints(desireCameraClipPoints, targetPos))
        {
            colliding = true;
        }
        else
        {
            colliding = false;
        }
    }
}
