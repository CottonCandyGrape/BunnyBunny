using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    //이동 관련
    [HideInInspector] public float h = 0.0f;
    [HideInInspector] public float v = 0.0f;
    float moveSpeed = 3.0f;

    public Vector3 moveDir = Vector3.zero;
    Vector3 scale = Vector3.one;
    //Vector3 limitPos = Vector3.zero;

    public GameObject DirArrow = null;
    public Vector3 arrowDir = Vector3.up;
    float arrowAngle = 0.0f;
    float angleOffset = 90.0f;
    const float cstArrowDist = 0.7f;
    //이동 관련

    //능력치 관련
    int curHp = 100;
    int maxHp = 100;
    int attack = 10;
    int defense = 10;
    int curExp = 0;
    int nextExp = 100;
    //능력치 관련

    //공격 관련
    public Vector3 bulletDir = Vector3.up;
    const float cstBulletDist = 0.3f;
    float bulletTimer = 0.0f;
    float bltTime = 0.2f;
    int fireCnt = 5;
    int curFire = 0;
    //공격 관련

    //TODO : Skill

    void Start()
    {
        //curHp = maxHp;
        curHp = int.MaxValue; //TODO : Test 용임. 정리하기
    }

    void Update()
    {
        Move();
        DirectionArrow();
        LoadBullet();
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

    void DirectionArrow()
    {
        if (moveDir.normalized != Vector3.zero)
            arrowDir = moveDir.normalized;

        arrowAngle = Mathf.Atan2(arrowDir.normalized.y, arrowDir.normalized.x) * Mathf.Rad2Deg;
        DirArrow.transform.rotation = Quaternion.AngleAxis(arrowAngle - angleOffset, Vector3.forward);
        DirArrow.transform.position = transform.position + arrowDir.normalized * cstArrowDist;
    }

    void LoadBullet()
    {
        bulletTimer -= Time.deltaTime;

        if(bulletTimer <= 0.0f)
        {
            bulletTimer = bltTime;

            if (fireCnt < curFire)
            {
                FanFire(arrowDir);
                curFire = 0;
            }
            else
            {
                FireBullet(arrowDir);
                curFire++;
            }
        }
    }

    void FireBullet(Vector3 bltDir)
    {
        bltDir.Normalize();
        bulletDir = bltDir;

        BulletCtrl bltCtrl = MemoryPoolMgr.Inst.AddBulletPool();
        bltCtrl.gameObject.SetActive(true);
        bltCtrl.transform.position = transform.position + bltDir * cstBulletDist;
        float angle = Mathf.Atan2(bltDir.y, bltDir.x) * Mathf.Rad2Deg;
        bltCtrl.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void FanFire(Vector3 midDir)
    {
        int deg = 20;// TODO : 하드코딩 바꾸기 
        for (int cnt = -2; cnt < 3; cnt++)
        {
            Vector3 dir = Quaternion.AngleAxis(cnt * deg, Vector3.forward) * midDir;
            FireBullet(dir);
        }
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;

        if (curHp <= 0)
        {
            PlayerDie();
        }

        //TODO : UI 데미지 표시
        //TODO : 피 게이지바 표시
    }

    void PlayerDie()
    {
        Time.timeScale = 0.0f;
        return;
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
