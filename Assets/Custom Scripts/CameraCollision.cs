using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {

    [SerializeField] float minDistance = 1f;
    [SerializeField] float maxDistance = 4f;
    [SerializeField] float smooth = 10f;
    [SerializeField] float distance;
    [SerializeField] Transform offset;

    Vector3 dollyDir;
    bool inCTMview = false;
    void Awake()
    {
        distance = transform.localPosition.magnitude;
    }
    // Update is called once per frame
    void FixedUpdate ()
    {

        if (Input.GetButtonDown("Switch View"))
        {
            inCTMview = !inCTMview;
        }

        if (inCTMview)
        {
            ProcessView(offset);
        }
        else
        {
            ProcessView(transform.parent);

        }

    }

    private void ProcessView(Transform camOffset)
    {
        Vector3 desiredCameraPos = camOffset.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast(camOffset.position, desiredCameraPos, out hit))
        {
            distance = Mathf.Clamp((hit.distance * .5f), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }

        transform.localPosition = Vector3.Lerp(camOffset.localPosition, dollyDir * distance, Time.deltaTime * smooth);
        transform.localRotation = camOffset.localRotation;
    }
}
