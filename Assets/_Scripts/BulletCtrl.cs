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
    float drlOffset = 0.3f;

    Vector3 moveDir = Vector3.one;
    public Vector3 MoveDir
    {
        get { return moveDir; }
        set { moveDir = value; }
    }

    void OnEnable()
    {
        if (BltType == BulletType.Gun)
        {
            moveSpeed = 10.0f;
            lifeTime = 5.0f;
        }
        else if (BltType == BulletType.Rocket)
        {
            moveSpeed = 5.0f;
            lifeTime = 8.0f;
        }
        else if (BltType == BulletType.Drill)
        {
            if (!WeaponMgr.Inst.DrillCtrlSc.IsEvolve)
            {
                moveSpeed = 8.0f;
                lifeTime = 2.0f;
            }
            else
            {
                moveSpeed = 15.0f;
                lifeTime = 5.0f;
            }
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

        CalcLifeTime();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (BltType == BulletType.Rocket)
        {
            //Rocket만 여기서 구현한 이유 : Guardian, Drill은 Tag로 충돌체크함.
            //반면 Rocket은 bulletType이 필요한데 여기서 체크하기가 더 편함.
            if (coll.tag.Contains("Monster")) //모든 타입 몬스터들 포함하기 위해 Contains
            {
                WeaponMgr.Inst.RocketCtrlSc.ExploseRocket(WeaponMgr.Inst.RocketCtrlSc.IsEvolve, gameObject);
            }
        }

        if (gameObject.CompareTag("E_Bullet") && coll.CompareTag("Player"))
        {
            if (gameObject.name.Contains("MeatBullet")) //MeatSoldier의 총알
            {
                GameMgr.Inst.player.TakeDamage(10.0f);
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
        if (transform.position.x - drlOffset < ScreenMgr.CurScMin.x)
            moveDir.x = Mathf.Abs(moveDir.x); //-> 양
        else if (ScreenMgr.CurScMax.x < transform.position.x + drlOffset)
            moveDir.x = -Mathf.Abs(moveDir.x); //-> 음

        if (transform.position.y - drlOffset < ScreenMgr.CurScMin.y)
            moveDir.y = Mathf.Abs(moveDir.y);
        else if (ScreenMgr.CurScMax.y < transform.position.y + drlOffset)
            moveDir.y = -Mathf.Abs(moveDir.y);

        moveDir.Normalize();

        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    /*
    void WallBounding() //Drill일 경우 튕기기.
    {
    //이 코드는 방향을 바꾼후에도 조건에 들어오는 경우가 있어서 
    //화면 밖에 머물러서 계속 방향을 바꾸는 버그가 있었다(jittering?)
    //이렇게 짜지 말자
        if (transform.position.x - drlOffset < ScreenMgr.CurScMin.x ||
            ScreenMgr.CurScMax.x < transform.position.x + drlOffset) 
            moveDir.x *= -1;

        if (transform.position.y - drlOffset < ScreenMgr.CurScMin.y ||
            ScreenMgr.CurScMax.y < transform.position.y + drlOffset)
            moveDir.y *= -1;
        moveDir.Normalize();
    }
    */
}