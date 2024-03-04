using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Bullet,
    Rocket,
    Drill,
}

public class BulletCtrl : MonoBehaviour
{
    public BulletType BltType = BulletType.Bullet;

    protected float moveSpeed = 10.0f;
    protected float lifeTime = 0.0f;
    float outLine = 3.0f;

    protected Vector3 moveDir = Vector3.one;
    public Vector3 MoveDir
    {
        set { moveDir = value; }
    }

    void OnEnable()
    {
        lifeTime = 5.0f;
    }

    void Start() { }

    protected virtual void Update()
    {
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        CheckOutLine();

        CalcLifeTime();
    }

    protected void CalcLifeTime() //lifeTime 계산
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0.0f)
            gameObject.SetActive(false);
    }

    protected void CheckOutLine() //밖으로 나갔는지 체크
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