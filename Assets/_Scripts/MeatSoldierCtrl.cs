using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatSoldierCtrl : MonsterCtrl
{
    float bossHp = 1000;
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

    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        if (!GameMgr.Inst.hasBoss) return; // 깜빡거릴때는 안맞기

        base.OnTriggerEnter2D(coll);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!GameMgr.Inst.hasBoss) return;

        //Boss일때는 player collier에 isTrigger false되기 때문에 여기서 구현   
        if (coll.gameObject.CompareTag("Player"))
            GameMgr.Inst.player.TakeDamage(collDmg);
    }

    void InitBoss()
    {
        monType = MonsterType.BossMon;
        curHp = bossHp;
        StartCoroutine(BlinkBoss());
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        GameMgr.Inst.UpdateBossHpBar(curHp / bossHp);

        if (curHp <= 0) MonsterDie();
    }

    protected override void MonsterDie()
    {
        GameMgr.Inst.GameOver();
    }

    IEnumerator BlinkBoss()
    {
        Color clr = Color.white;
        float speed = 2.5f;
        int blinkTimes = 3;

        for (int i = 0; i < blinkTimes; i++)
        {
            while (0.0f <= clr.a)
            {
                clr.a -= speed * Time.deltaTime;
                spRenderer.color = clr;
                yield return null;
            }

            while (clr.a <= 1.0f)
            {
                clr.a += speed * Time.deltaTime;
                spRenderer.color = clr;
                yield return null;
            }
        }

        GameMgr.Inst.hasBoss = true;
    }
}