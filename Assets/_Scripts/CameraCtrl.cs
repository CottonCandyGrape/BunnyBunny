using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    Transform player = null;
    Vector3 playerPos = Vector3.zero;
    Camera cam = null;

    float[] BossCamSize = { 8.5f, 5.3f }; //Ground, Vertical Type 순서
    float[] BossCamSpeed = { 3.0f, 0.5f }; 

    void Start()
    {
        cam = Camera.main;

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
        if (GameMgr.Inst.MType == MapType.Ground)
        {
            playerPos = player.position;
            playerPos.z = transform.position.z;
            transform.position = playerPos;
        }
        else if (GameMgr.Inst.MType == MapType.Vertical)
        {
            playerPos = player.position;
            playerPos.z = transform.position.z;
            playerPos.x = 0;
            transform.position = playerPos;
        }
    }

    public void ZoomOut()
    {
        StartCoroutine(ZoomOutCo());
    }

    IEnumerator ZoomOutCo()
    {
        float tSize = BossCamSize[(int)GameMgr.Inst.MType];
        float speed = BossCamSpeed[(int)GameMgr.Inst.MType];

        while (cam.orthographicSize < tSize)
        {
            cam.orthographicSize += speed * Time.deltaTime;
            if (cam.orthographicSize >= tSize)
            {
                cam.orthographicSize = tSize;
                yield break;
            }
            else
                yield return null;
        }
    }
}
