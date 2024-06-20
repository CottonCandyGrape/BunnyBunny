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

    const float LimitPosX = 3.7f;
    const float LimitPosY = 3.0f;

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
        else if (GameMgr.Inst.MType == MapType.FixedGround)
        {
            playerPos = player.position;
            playerPos.z = transform.position.z;

            if (playerPos.x <= -LimitPosX) playerPos.x = -LimitPosX;
            else if (playerPos.x >= LimitPosX) playerPos.x = LimitPosX;

            if (playerPos.y <= -LimitPosY) playerPos.y = -LimitPosY;
            else if (playerPos.y >= LimitPosY) playerPos.y = LimitPosY;

            transform.position = playerPos;
        }
    }

    public void ZoomOut()
    {
        if (GameMgr.Inst.MType != MapType.FixedGround)
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
