using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Gun,
    Rocket,
    Drill,
}

public class BulletCtrl : MonoBehaviour
{
    public BulletType BltType = BulletType.Gun;

    float moveSpeed = 0.0f;
    float lifeTime = 0.0f;
    float outLine = 3.0f;
    float drlOffset = 0.5f;

    Vector3 moveDir = Vector3.one;
    public Vector3 MoveDir
    {
        set { moveDir = value; }
    }

    void OnEnable()
    {
        if(BltType == BulletType.Gun)
        {
            moveSpeed = 10.0f;
            lifeTime = 5.0f;
        }
        else if(BltType == BulletType.Rocket)
        {
            moveSpeed = 5.0f;
            lifeTime = 8.0f;
        }
        else if(BltType == BulletType.Drill)
        { 
            moveSpeed = 8.0f;
            lifeTime = 100.0f;
        }
    }

    void Start() { }

    void Update()
    {
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        if (BltType == BulletType.Drill)
            WallBounding();
        else
            CheckOutLine();

        //if (BltType != BulletType.Drill)
        //    CheckOutLine();

        CalcLifeTime();
    }

    //void LateUpdate()
    //{
    //    if (BltType == BulletType.Drill)
    //        WallBounding();

    //    transform.position += moveDir * moveSpeed * Time.deltaTime;
    //}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (BltType == BulletType.Rocket)
        {
            if (coll.tag.Contains("Monster"))
            {
                WeaponMgr.Inst.RocketCtrlSc.ExploseRocket(gameObject);
            }
        }
    }

    void CalcLifeTime() //lifeTime 계산
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0.0f)
            gameObject.SetActive(false);
    }

    void CheckOutLine() //밖으로 나갔는지 체크
    {
        if (ScreenMgr.CurScMax.x + outLine < transform.position.x ||
            transform.position.x < ScreenMgr.CurScMin.x - outLine ||
            ScreenMgr.CurScMax.y + outLine < transform.position.y ||
            transform.position.y < ScreenMgr.CurScMin.y - outLine)
        {
            gameObject.SetActive(false);
        }
    }

    void WallBounding() //Drill일 경우 튕기기.
    {
        if (transform.position.x - drlOffset < ScreenMgr.CurScMin.x ||
            ScreenMgr.CurScMax.x < transform.position.x + drlOffset) 
        {
            moveDir.x *= -1;
            Debug.Log("x팅김 : " + moveDir);
        }

        if (transform.position.y - drlOffset < ScreenMgr.CurScMin.y ||
            ScreenMgr.CurScMax.y < transform.position.y + drlOffset)
        {
            moveDir.y *= -1;
            Debug.Log("y팅김 : " + moveDir);
        }
        moveDir.Normalize();

        Debug.Log("normalized moveDir : " + moveDir);

        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}