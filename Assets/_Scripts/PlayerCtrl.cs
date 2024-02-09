using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    //이동 관련
    [HideInInspector] public float h = 0.0f;
    [HideInInspector] public float v = 0.0f;
    float moveSpeed = 3.0f;

    Vector3 moveDir = Vector3.zero;
    Vector3 scale = Vector3.one;
    //Vector3 limitPos = Vector3.zero;
    //이동 관련

    //게임 관련
    float curHp = 100;
    float maxHp = 100;
    float attack = 10;
    float defense = 10;
    float curExp = 0;
    float nextExp = 100;
    //게임 관련

    //TODO : Skill

    void Start()
    {

    }

    void Update()
    {
        Move();
    }

    void Move()
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

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    /*
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
    */
}
