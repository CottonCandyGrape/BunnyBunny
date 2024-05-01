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
    protected MonsterType monType = MonsterType.NormalMon;

    //이동 관련
    protected float moveSpeed = 1.0f;
    protected Vector3 moveDir = Vector3.zero;
    protected SpriteRenderer spRenderer = null;
    protected Rigidbody2D rigid = null;
    float slowTimer = 0.0f;
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
    float expVal = 0;
    //능력치 관련

    //UI 관련
    Vector3 dmgTxtOffset = new Vector3(0, 0.5f, 0);
    //UI 관련

    //애니메이션 관련
    protected Animator animator = null;
    //애니메이션 관련

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spRenderer = GetComponentInChildren<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        slowTimer = 0.0f;
        curHp = maxHp;
        SetExp();
    }

    void Start() { }

    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        if (0.0f <= slowTimer) SlowTimer();
    }

    void SlowTimer()
    {
        slowTimer -= Time.deltaTime;
        if (slowTimer < 0.0f) moveSpeed = 1.0f;
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("P_Bullet"))
        {
            TakeDamage(dftDmg);
            coll.gameObject.SetActive(false);

            if (monType == MonsterType.BossMon) return;

            if (WeaponMgr.Inst.MainType == MWType.Gun)
            {
                GameObject bltEft = MemoryPoolMgr.Inst.AddBulletEffectPool();
                bltEft.SetActive(true);
                bltEft.transform.position = transform.position;

                AnimEffect animEft = bltEft.GetComponent<AnimEffect>();
                if (animEft != null) animEft.Target = gameObject;

                if (GameMgr.Inst.player.AttackType == AtkType.Fire)
                {
                    //불은 추가 데미지
                    TakeDamage(10); //TODO : 데미지 기준 세우기
                }
                else if (GameMgr.Inst.player.AttackType == AtkType.Water)
                {
                    //물은 느려지기.
                    moveSpeed = 0.5f;
                    slowTimer = slowTime;
                }
            }
        }
        else if (coll.CompareTag("Player"))
        {
            float dmg = 10;
            if (monType == MonsterType.EliteMon)
                dmg = 20;
            else if (monType == MonsterType.BossMon)
                dmg = 30;

            GameMgr.Inst.player.TakeDamage(dmg);
        }
        else if (coll.CompareTag("Guard"))
        {
            isKnockBack = true;
            kbTarget = transform.position + moveDir * kbDist;
            TakeDamage((WeaponMgr.Inst.GuardiansCtrlSc.CurLv + 1) * 10);
        }
        else if (coll.CompareTag("Drill"))
        {
            TakeDamage((WeaponMgr.Inst.DrillCtrlSc.CurLv + 1) * 20);
        }
    }

    void SetExp() //TODO : Init()만들어서 monType으로 나뉘는 변수들 한번에 초기화 하기
    {
        expVal = 10;
        if (monType == MonsterType.EliteMon)
            expVal = 20;
    }

    protected void Move()
    {
        if (!isKnockBack)
            TracePlayer();
        else
            KnockBack();
    }

    void TracePlayer()
    {
        moveDir = GameMgr.Inst.player.transform.position - transform.position;
        moveDir.Normalize();

        if (moveDir.x < 0)
            spRenderer.flipX = false;
        else
            spRenderer.flipX = true;

        rigid.MovePosition(transform.position + moveDir * moveSpeed * Time.deltaTime);
    }

    void KnockBack()
    {
        kbTimer += kbSpeed * Time.deltaTime;
        Vector3 lerpVec = Vector3.Lerp(transform.position, kbTarget, kbTimer);
        rigid.MovePosition(lerpVec);

        if (1.0f <= kbTimer)
        {
            isKnockBack = false;
            kbTimer = 0.0f;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (curHp <= 0) return; //이미 0이하인 경우에도 들어오는 경우가 있어서 추가함.
        //boss일때만 그런건가?

        //1. Hp변수 깎기
        float dmgTxt = curHp < damage ? curHp : damage;
        curHp -= damage;
        //2. Dmg Txt 띄우기 
        GameMgr.Inst.SpawnDmgTxt(transform.position + dmgTxtOffset, dmgTxt, Color.red);

        if (curHp <= 0) MonsterDie();
    }

    protected virtual void MonsterDie()
    {
        if (monType == MonsterType.BossMon) return;

        MemoryPoolMgr.Inst.ActiveMonsterCount--;
        GameMgr.Inst.KillTxtUpdate(); //킬수 올리기
        GameMgr.Inst.AddExpVal(expVal); //경험치 올리기
        float exp = Random.Range(0.0f, 1.0f);
        if (exp <= 0.05f)
            ItemMgr.Inst.SpawnBomb(transform.position); //폭탄 스폰
        else
            ItemMgr.Inst.SpawnGold(transform.position, monType); //골드 스폰

        gameObject.SetActive(false);
    }
}