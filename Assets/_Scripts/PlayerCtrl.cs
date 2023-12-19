using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    float h = 0.0f;
    float v = 0.0f;
    float moveSpeed = 0.5f;

    //Vector3 moveDir = Vector3.zero;
    Vector2 moveDir = Vector2.zero;
    Vector3 scale = Vector3.one;
    Vector3 limitPos = Vector3.zero;

    SpriteRenderer bgRender = null;
    Vector2 bgOffset = Vector2.zero;

    void Start()
    {
        bgRender = GameObject.Find("Background").GetComponent<SpriteRenderer>();
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

        moveDir = (Vector2.up * v) + (Vector2.right * h);
        if (1.0f < moveDir.magnitude)
            moveDir.Normalize();

        bgOffset += moveDir * moveSpeed * Time.deltaTime;
        bgRender.material.mainTextureOffset = bgOffset;
        //transform.position += moveDir * moveSpeed * Time.deltaTime;

        //MoveLimit();
    }

    //TODO : player sprite 크기 고려하기.
    //offset 사용한다면 float 제한 해주기
    void MoveLimit() 
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
