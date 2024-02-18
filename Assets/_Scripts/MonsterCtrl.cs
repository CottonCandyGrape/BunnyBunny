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
    int maxHp = 100;
    int curHp = 100;
    //int defense = 10;
    //int attack = 10;
    int dftDmg = 30;
    //능력치 관련

    //UI 관련
    Vector3 dmgTxtOffset = new Vector3(0, 0.5f, 0);
    //UI 관련

    //Gold 관련
    public GameObject GoldPrefab = null;
    //Gold 관련

    void OnEnable()
    {
        curHp = maxHp;
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
            int dmg = 10;
            if (monType == MonsterType.EliteMon)
                dmg = 20;
            else if (monType == MonsterType.BossMon)
                dmg = 30;

            GameMgr.Inst.player.TakeDamage(dmg);
        }
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

    void TakeDamage(int damage)
    {
        //1. Hp변수 깎기
        curHp -= damage;
        //2. Dmg Txt 띄우기 
        GameMgr.Inst.SpawnDmgTxt(transform.position + dmgTxtOffset, damage);

        if (curHp <= 0)
        {
            MonsterDie();
            return;
        }

        //TODO : 데미지 UI 표시 하기
    }

    void SpawnGold()
    {
        GameObject gold = Instantiate(GoldPrefab);
        gold.transform.position = transform.position;

        ItemCtrl item = gold.GetComponent<ItemCtrl>();
        if (monType == MonsterType.NormalMon)
            item.GoldVal = 10;
        else if (monType == MonsterType.EliteMon)
            item.GoldVal = 50;
        else if (monType == MonsterType.BossMon)
            item.GoldVal = 100;
    }

    void MonsterDie()
    {
        MemoryPoolMgr.Inst.ActiveMonsterCount--;
        GameMgr.Inst.KillTxtUpdate();
        SpawnGold();

        gameObject.SetActive(false);
    }
}