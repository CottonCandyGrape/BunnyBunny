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
        { }
    }

    void Start() { }

    void Update()
    {
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        CheckOutLine();
        CalcLifeTime();
    }

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
}