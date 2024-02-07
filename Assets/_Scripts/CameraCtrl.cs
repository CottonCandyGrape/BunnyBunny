using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    Transform player = null;
    Vector3 playerPos = Vector3.zero;

    void Start()
    {
        if (!player)
            player = GameObject.Find("PlayerRoot").GetComponent<Transform>();
    }

    void LateUpdate()
    {
        MoveCamera();
    }

    //Infinte Map 전용
    void MoveCamera()
    {
        playerPos = player.position;
        playerPos.z = transform.position.z;
        transform.position = playerPos;
    }
}
