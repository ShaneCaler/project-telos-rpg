using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

    [SerializeField] Texture2D walkCursor;
    [SerializeField] Texture2D attackCursor;
    [SerializeField] Texture2D unknownCursor;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0); 

    CameraRaycaster cameraRaycaster;

    void Awake () {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.onLayerChange += OnLayerChangeHandler; // register delegate
	}


	void OnLayerChangeHandler (Layer newLayer) {
        switch (newLayer)
        {
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.ForceSoftware);
                break;
            case Layer.Enemy:
                Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.ForceSoftware);
                break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.ForceSoftware);
                break;
            default:
                Debug.LogError("Unsure of which cursor to show");
                return;
        }
    }
}
