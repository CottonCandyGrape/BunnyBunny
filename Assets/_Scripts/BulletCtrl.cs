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

    //Monster 관련
    float dftDmg = 30.0f;
    WaitForSeconds delay = new WaitForSeconds(0.1f);
    //Monster 관련

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
        /*
        if (coll.tag.Contains("Monster"))
        {
            MonsterCtrl monCtrl = coll.gameObject.GetComponent<MonsterCtrl>();

            if (monCtrl.monType == MonsterType.BossMon && !GameMgr.Inst.hasBoss) return; //보스에겐 효과 안줌.

            if (CompareTag("P_Bullet"))
            {
                if (!name.Contains("_Ev")) //일반 총알
                {
                    monCtrl.TakeDamage(dftDmg);
                    gameObject.SetActive(false);

                    if (monCtrl.monType == MonsterType.BossMon) return; //보스에겐 효과 안줌.

                    if (!coll.gameObject.activeSelf) return; //몬스터 죽으면 효과 안줘도 됨.

                    if (WeaponMgr.Inst.MainType == MWType.Gun) //Tag가 P_Bullet(총알)이니깐 당연히 Gun인가?
                    {
                        GameObject bltEft = MemoryPoolMgr.Inst.AddBulletEffectPool();
                        bltEft.SetActive(true);
                        bltEft.transform.position = coll.transform.position;

                        AnimEffect animEft = bltEft.GetComponent<AnimEffect>();
                        if (animEft != null) animEft.Target = coll.gameObject;


                        if (GameMgr.Inst.player.AttackType == AtkType.Fire)
                        {
                            monCtrl.AdditiveFireDmg(); //불은 추가 데미지
                        }
                        else if (GameMgr.Inst.player.AttackType == AtkType.Water)
                        {
                            //물은 느려지기.
                            moveSpeed = 0.5f;
                            monCtrl.SlowTimer = monCtrl.SlowTime;
                        }
                    }
                }
                else if (coll.name.Contains("_Ev"))//진화 총알
                {
                    monCtrl.TakeDamage(dftDmg * 2);
                    gameObject.SetActive(false);

                    if (WeaponMgr.Inst.MainType == MWType.Gun)
                    {
                        GameObject bltEft = MemoryPoolMgr.Inst.AddEvSupBulletPool();
                        bltEft.SetActive(true);
                        bltEft.transform.position = coll.transform.position;
                    }
                }
            }

            SoundMgr.Instance.PlaySfxSound("attacked");
        }
        else if (coll.CompareTag("Player") && gameObject.CompareTag("E_Bullet"))
        */

        if (coll.CompareTag("Player") && CompareTag("E_Bullet"))
        {
            if (gameObject.name.Contains("MeatBullet")) //MeatSoldier의 총알
            {
                SoundMgr.Instance.PlaySfxSound("eBullet");
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

    void DrillSfx(bool isEvolve)
    {
        if (isEvolve)
            SoundMgr.Instance.PlaySfxSound("arrowHead");
        else
            SoundMgr.Instance.PlaySfxSound("drill");
    }

    void WallBounding() //Drill일 경우 튕기기.
    {
        Vector3 tmp = moveDir;

        if (transform.position.x - drlOffset < ScreenMgr.CurScMin.x)
            moveDir.x = Mathf.Abs(moveDir.x); //-> 양
        else if (ScreenMgr.CurScMax.x < transform.position.x + drlOffset)
            moveDir.x = -Mathf.Abs(moveDir.x); //-> 음

        if (transform.position.y - drlOffset < ScreenMgr.CurScMin.y)
            moveDir.y = Mathf.Abs(moveDir.y);
        else if (ScreenMgr.CurScMax.y < transform.position.y + drlOffset)
            moveDir.y = -Mathf.Abs(moveDir.y);

        moveDir.Normalize();

        if (tmp.x * moveDir.x < 0 || tmp.y * moveDir.y < 0)
            DrillSfx(WeaponMgr.Inst.DrillCtrlSc.IsEvolve);

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