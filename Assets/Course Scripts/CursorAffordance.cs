using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

    [SerializeField] Texture2D walkCursor;
    [SerializeField] Texture2D attackCursor;
    [SerializeField] Texture2D unknownCursor;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0); 

    CameraRaycaster cameraRayCaster;
    // Use this for initialization
    void Start () {
        cameraRayCaster = FindObjectOfType<CameraRaycaster>();
	}


	// Update is called once per frame
	void LateUpdate () {
        switch (cameraRayCaster.currentLayerHit)
        {
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.Enemy:
                Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                return;
        }
    }
}
