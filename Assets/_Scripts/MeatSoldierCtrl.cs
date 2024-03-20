using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatSoldierCtrl : MonsterCtrl
{
    float bossHp = 100;
    float collDmg = 30;

    void Start()
    {
        InitBoss();
    }

    //void FixedUpdate() { }

    void Update()
    {
        //base.Move(); //TODO : 얘도 Collision이 생기니깐 fixed에서 할까?
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {//Boss일때는 player collier에 isTrigger false되기 때문에 여기서 구현   
            GameMgr.Inst.player.TakeDamage(collDmg);
        }
    }

    void InitBoss()
    {
        monType = MonsterType.BossMon;
        curHp = bossHp;
    }
}