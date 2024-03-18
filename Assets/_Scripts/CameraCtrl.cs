using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    Transform player = null;
    Vector3 playerPos = Vector3.zero;
    Camera cam = null;

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
        playerPos = player.position;
        playerPos.z = transform.position.z;
        transform.position = playerPos;
    }

    public void ZoomOut()
    {
        StartCoroutine(ZoomOutCo());
    }

    IEnumerator ZoomOutCo()
    {
        float size = 8.5f;
        float zTimer = 0.0f;
        float zTime = 1.0f;
        float speed = 0.5f; //TODO : 추후 조정

        while (zTimer <= 1.0f)
        {
            zTimer += speed * Time.deltaTime;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, size, (zTimer / zTime));
            yield return null;
        }
    }
}
