using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    float h = 0.0f;
    float v = 0.0f;
    float moveSpeed = 5.0f;

    Vector3 moveDir = Vector3.zero;
    Vector3 scale = Vector3.one;
    Vector3 limitPos = Vector3.zero;

    void Start()
    {

    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        //좌우 방향 바뀔때마다 flip
        if (0.0f < h)
            scale.x = 1;
        else if (h < 0.0f)
            scale.x = -1;
        transform.localScale = scale;

        moveDir = (Vector3.up * v) + (Vector3.right * h);
        if (1.0f < moveDir.magnitude)
            moveDir.Normalize();

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        MoveLimit();
    }

    void MoveLimit() //TODO : player sprite 크기 고려하기.
    {
        limitPos = transform.position;

        if (ScreenMgr.ScMin.x > transform.position.x)
            limitPos.x = ScreenMgr.ScMin.x;
        if (ScreenMgr.ScMax.x < transform.position.x)
            limitPos.x = ScreenMgr.ScMax.x;
        if (ScreenMgr.ScMin.y > transform.position.y)
            limitPos.y = ScreenMgr.ScMin.y;
        if (ScreenMgr.ScMax.y < transform.position.y)
            limitPos.y = ScreenMgr.ScMax.y;

        transform.position = limitPos;
    }
}
