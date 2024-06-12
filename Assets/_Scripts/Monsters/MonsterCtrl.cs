using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    NormalMon = 0,
    EliteMon,
    BossMon,
}

public class MonsterCtrl : MonoBehaviour
{
    public MonsterType monType = MonsterType.NormalMon;

    //이동 관련
    protected float moveSpeed = 1.0f;
    protected Vector3 moveDir = Vector3.zero;
    protected SpriteRenderer spRenderer = null;
    protected Rigidbody2D rigid = null;
    protected Collider2D coll = null;
    protected float slowTimer = 0.0f;
    float slowTime = 3.0f;
    //이동 관련

    //넉백 관련 
    protected bool isKnockBack = false;
    float kbDist = -2.0f;
    float kbSpeed = 3.0f;
    float kbTimer = 0.0f;
    Vector3 kbTarget = Vector3.zero;
    //넉백 관련 

    //능력치 관련
    protected float curHp = 100;
    float maxHp = 100;
    float dftDmg = 30;
    float bumpDmg = 10;
    float expVal = 10;
    int goldVal = 10;
    float sRan = 1.0f, eRan = 2.0f;
    //능력치 관련

    //UI 관련
    Vector3 dmgTxtOffset = new Vector3(0, 0.5f, 0);
    WaitForSeconds delay = new WaitForSeconds(0.1f);
    //UI 관련

    //애니메이션 관련
    protected Animator animator = null;
    //애니메이션 관련

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spRenderer = GetComponentInChildren<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        isKnockBack = false; //넉백하다가 죽으면 바로 다시 태어났을때 바로 넉백이기 때문에 그 자리로 순간이동 한다.
        slowTimer = 0.0f;
        curHp = maxHp;
        Init();
    }

    void Start() { }

    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        if (0.0f <= slowTimer) SpeedSlowTimer();
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.CompareTag("Area"))
        {
            transform.position = GameMgr.Inst.player.transform.position +
                                    GameMgr.Inst.MonGen.GetMonSpawnPos();
        }
    }

    protected void Init()
    {
        if (monType == MonsterType.NormalMon)
        { 
            bumpDmg = 10;
            expVal = 10;
            goldVal = 10;
            sRan = 1.0f;
            eRan = 1.5f;
        }
        else if (monType == MonsterType.EliteMon)
        {
            bumpDmg = 20;
            expVal = 20;
            goldVal = 50;
            sRan = 0.2f;
            eRan = 1.6f;
        }
        else if (monType == MonsterType.BossMon)
        {
            bumpDmg = 30;
            expVal = 50;
            goldVal = 100;
            sRan = 0.0f;
            eRan = 0.2f;
        }
    }    

    void SpeedSlowTimer()
    {
        slowTimer -= Time.deltaTime;
        if (slowTimer < 0.0f) moveSpeed = 1.0f;
    }

    IEnumerator DelayTakeDamage(float dmg)
    {
        yield return delay;
        TakeDamage(dmg);
    }

    public void AdditiveFireDmg()
    {
        StopAllCoroutines();
        StartCoroutine(DelayTakeDamage(10)); //TODO : 데미지 기준 세우기
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("P_Bullet"))
        {
            if (!coll.name.Contains("_Ev")) //일반 총알
            {
                TakeDamage(dftDmg);
                coll.gameObject.SetActive(false);

                if (monType == MonsterType.BossMon) return; //보스에겐 효과 안줌.

                if (!gameObject.activeSelf) return; //몬스터 죽으면 효과 안줘도 됨.

                if (WeaponMgr.Inst.MainType == MWType.Gun) //Tag가 P_Bullet(총알)이니깐 당연히 Gun인가?
                {
                    GameObject bltEft = MemoryPoolMgr.Inst.AddBulletEffectPool();
                    bltEft.SetActive(true);
                    bltEft.transform.position = transform.position;

                    AnimEffect animEft = bltEft.GetComponent<AnimEffect>();
                    if (animEft != null) animEft.Target = gameObject;


                    if (GameMgr.Inst.player.AttackType == AtkType.Fire)
                    {
                        SoundMgr.Instance.PlaySfxSound("flame");
                        AdditiveFireDmg(); //불은 추가 데미지
                    }
                    else if (GameMgr.Inst.player.AttackType == AtkType.Water)
                    {
                        SoundMgr.Instance.PlaySfxSound("slower");
                        //물은 느려지기.
                        moveSpeed = 0.5f;
                        slowTimer = slowTime;
                    }
                }
            }
            else if (coll.name.Contains("_Ev"))//진화 총알
            {
                TakeDamage(dftDmg * 2);
                coll.gameObject.SetActive(false);

                if (WeaponMgr.Inst.MainType == MWType.Gun)
                {
                    GameObject bltEft = MemoryPoolMgr.Inst.AddEvSupBulletPool();
                    bltEft.SetActive(true);
                    bltEft.transform.position = transform.position;
                }
            }
        }
        else if (coll.gameObject.CompareTag("Sup_Bullet"))
        {
            TakeDamage(dftDmg * 2);
        }
        else if (coll.gameObject.CompareTag("Player"))
        {
            GameMgr.Inst.player.TakeDamage(bumpDmg);
        }
        else if (coll.gameObject.CompareTag("Guard"))
        {
            isKnockBack = true;
            coll.isTrigger = true;
            kbDist = -Random.Range(sRan, eRan);
            kbTarget = transform.position + moveDir.normalized * kbDist;
            TakeDamage((WeaponMgr.Inst.GuardiansCtrlSc.CurLv + 1) * 10);
        }
        else if (coll.gameObject.CompareTag("Drill"))
        {
            TakeDamage((WeaponMgr.Inst.DrillCtrlSc.CurLv + 1) * 20);
        }
        else if (coll.gameObject.CompareTag("Rocket"))
        {
            WeaponMgr.Inst.RocketCtrlSc.ExploseRocket(WeaponMgr.Inst.RocketCtrlSc.IsEvolve, coll.gameObject);
        }
    }

    protected void Move()
    {
        Vector3 target = Vector3.zero;

        if (!isKnockBack)
            target = TracePlayer();
        else
            target = KnockBack();

        if (GameMgr.Inst.MType == MapType.Vertical) LimitXPos(target);

        if (GameMgr.Inst.hasBoss) TrapRing(target);
    }

    Vector3 TracePlayer()
    {
        Vector3 target = Vector3.zero;

        moveDir = GameMgr.Inst.player.transform.position - transform.position;
        moveDir.Normalize();

        spRenderer.flipX = moveDir.x < 0 ? false : true;

        target = transform.position + moveDir * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(target);

        return target;
    }

    Vector3 KnockBack()
    {
        Vector3 target = Vector3.zero;

        kbTimer += kbSpeed * Time.fixedDeltaTime;
        target = Vector3.Lerp(transform.position, kbTarget, kbTimer);
        rigid.MovePosition(target);

        if (1.0f <= kbTimer)
        {
            isKnockBack = false;
            kbTimer = 0.0f;
            coll.isTrigger = false;
        }

        return target;
    }

    void LimitXPos(Vector2 pos)
    {
        float spSize = spRenderer.bounds.size.x / 2.0f;

        if (pos.x >= ScreenMgr.CurScMax.x - spSize)
            pos.x = ScreenMgr.CurScMax.x - spSize;
        else if (pos.x <= ScreenMgr.CurScMin.x + spSize)
            pos.x = ScreenMgr.CurScMin.x + spSize;

        rigid.MovePosition(pos);
    }

    void TrapRing(Vector2 pos)
    {
        float spSizeX = spRenderer.bounds.size.x / 2.0f;
        float spSizeY = spRenderer.bounds.size.y / 2.0f;

        Transform ring = GameMgr.Inst.BattleRing.transform;
        float offsetX = GameMgr.Inst.MType == MapType.Ground ? 4.75f : 2.6f; //5-0.25, 2.85-0.25
        float offsetY = 4.75f;

        if (pos.x >= ring.position.x + offsetX - spSizeX)
            pos.x = ring.position.x + offsetX - spSizeX;
        else if (pos.x <= ring.position.x - offsetX + spSizeX)
            pos.x = ring.position.x - offsetX + spSizeX;

        if (pos.y >= ring.position.y + offsetY - spSizeY)
            pos.y = ring.position.y + offsetY - spSizeY;
        else if (pos.y <= ring.position.y - offsetY + spSizeY)
            pos.y = ring.position.y - offsetY + spSizeY;

        rigid.MovePosition(pos);
    }

    public virtual void TakeDamage(float damage)
    {
        if (curHp <= 0) return; //이미 0이하인 경우에도 들어오는 경우가 있어서 추가함.
        //boss일때만 그런건가?

        damage *= GameMgr.Inst.player.Atk;
        //1. Hp변수 깎기
        float dmgTxt = curHp < damage ? curHp : damage;
        curHp -= damage;
        //2. Dmg Txt 띄우기 
        GameMgr.Inst.SpawnDmgTxt(transform.position + dmgTxtOffset, dmgTxt, Color.red);

        if (curHp <= 0) MonsterDie();
    }

    void MonsterDie()
    {
        if (monType == MonsterType.BossMon) return;

        MemoryPoolMgr.Inst.ActiveMonsterCount--;
        GameMgr.Inst.KillTxtUpdate(); //킬수 올리기
        GameMgr.Inst.AddExpVal(expVal); //경험치 올리기
        float exp = Random.Range(0.0f, 1.0f);
        if (exp <= 0.01f)
            ItemMgr.Inst.SpawnBomb(transform.position); //폭탄 스폰
        else
            ItemMgr.Inst.SpawnGold(transform.position, goldVal); //골드 스폰

        gameObject.SetActive(false);
    }
}