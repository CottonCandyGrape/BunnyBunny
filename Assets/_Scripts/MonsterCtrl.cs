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
    float moveSpeed = 1.0f;
    Vector3 moveDir = Vector3.zero;
    SpriteRenderer spRenderer = null;
    //이동 관련

    //넉백 관련 
    bool isKnockBack = false;
    float kbDist = -2.0f;
    float kbSpeed = 3.0f;
    float kbTime = 1.0f;
    float kbTimer = 0.0f;
    Vector3 kbTarget = Vector3.zero;
    //넉백 관련 

    //능력치 관련
    float maxHp = 100;
    float curHp = 100;
    //float defense = 10;
    //float attack = 10;
    float dftDmg = 30;
    float expVal = 0;
    //능력치 관련

    //UI 관련
    Vector3 dmgTxtOffset = new Vector3(0, 0.5f, 0);
    //UI 관련

    void Awake()
    {
        spRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        curHp = maxHp;
        SetExp();
    }

    void Start() { }

    void Update()
    {
        Move();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("P_Bullet"))
        {
            TakeDamage(dftDmg);
            coll.gameObject.SetActive(false);
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
        }
    }
    
    void SetExp() //TODO : Init()만들어서 monType으로 나뉘는 변수들 한번에 초기화 하기
    {
        expVal = 10;
        if (monType == MonsterType.EliteMon)
            expVal = 20;
        else if (monType == MonsterType.BossMon)
            expVal = 30;
    }

    void Move()
    {
        if (!isKnockBack)
        {
            moveDir = GameMgr.Inst.player.transform.position - transform.position;
            moveDir.Normalize();

            if (moveDir.x < 0)
                spRenderer.flipX = false;
            else
                spRenderer.flipX = true;

            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        else 
        {
            kbTimer += kbSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, kbTarget, kbTimer / kbTime);

            if (1.0f <= (kbTimer / kbTime))
            {
                isKnockBack = false;
                kbTimer = 0.0f;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        //1. Hp변수 깎기
        float dmgTxt = curHp < damage ? curHp : damage;
        curHp -= damage;
        //2. Dmg Txt 띄우기 
        GameMgr.Inst.SpawnDmgTxt(transform.position + dmgTxtOffset, dmgTxt, Color.red);

        if (curHp <= 0)
        {
            MonsterDie();
            return;
        }
    }

    void MonsterDie()
    {
        MemoryPoolMgr.Inst.ActiveMonsterCount--;
        GameMgr.Inst.KillTxtUpdate(); //킬수 올리기
        GameMgr.Inst.AddExpVal(expVal); //경험치 올리기
        ItemMgr.Inst.SpawnGold(transform.position, monType); //골드 스폰

        gameObject.SetActive(false);
    }
}