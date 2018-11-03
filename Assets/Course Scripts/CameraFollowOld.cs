using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {


    private GameObject player;

    private Vector3 offset;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        offset = transform.position - player.transform.position;
        print(player);
    }
    private void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
