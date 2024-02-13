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

    float moveSpeed = 1.0f;
    Vector3 moveDir = Vector3.one;
    Vector3 scale = Vector3.one;

    int maxHp = 100;
    int curHp = 100;
    //int defense = 10;
    //int attack = 10;
    int dftDmg = 30;

    void OnEnable()
    {
        curHp = maxHp;
    }

    void Start()
    {
        
    }

    void Update()
    {
        Move();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.Contains("P_Bullet"))
        {
            GetDamage(dftDmg);
        }
        else if (coll.tag.Contains("Player"))
        {
            int dmg = 10;
            if (monType == MonsterType.EliteMon)
                dmg = 20;
            else if (monType == MonsterType.BossMon)
                dmg = 30;

            GameMgr.inst.player.GetDamage(dmg);
        }
    }

    void Move()
    {
        moveDir = GameMgr.inst.player.transform.position - transform.position;
        moveDir.Normalize();

        if (moveDir.x < 0)
            scale.x = 1;
        else
            scale.x = -1;

        transform.localScale = scale;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    void GetDamage(int damage)
    {
        curHp -= damage;

        if (curHp <= 0)
        {
            MonsterDie();
            return;
        }

        //TODO : 데미지 UI 표시 하기
    }

    void MonsterDie()
    {
        gameObject.SetActive(false);
    }
}