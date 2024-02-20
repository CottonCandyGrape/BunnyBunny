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
    Vector3 moveDir = Vector3.one;
    Vector3 scale = Vector3.one;
    //이동 관련

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
        if (coll.tag.Contains("P_Bullet"))
        {
            TakeDamage(dftDmg);
        }
        else if (coll.tag.Contains("Player"))
        {
            float dmg = 10;
            if (monType == MonsterType.EliteMon)
                dmg = 20;
            else if (monType == MonsterType.BossMon)
                dmg = 30;

            GameMgr.Inst.player.TakeDamage(dmg);
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
        moveDir = GameMgr.Inst.player.transform.position - transform.position;
        moveDir.Normalize();

        if (moveDir.x < 0)
            scale.x = 1;
        else
            scale.x = -1;

        transform.localScale = scale;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    void TakeDamage(float damage)
    {
        //1. Hp변수 깎기
        curHp -= damage;
        //2. Dmg Txt 띄우기 
        GameMgr.Inst.SpawnDmgTxt(transform.position + dmgTxtOffset, damage, Color.red);

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
        ItemMgr.Inst.SpawnGold(transform.position, monType); //골드 스폰
        GameMgr.Inst.AddExpVal(expVal); //경험치 올리기

        gameObject.SetActive(false);
    }
}