using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState { None, Walk, Run, Shot } //TODO : Shot은 보류

public class BossMonCtrl : MonsterCtrl
{
    protected Collider2D coll = null;
    protected float collDmg = 30.0f;
    protected float bossHp = 10.0f;
    protected bool isDie = false;

    //Walk
    protected float walkTimer = 0.0f;
    protected float walkTime = 5.0f;
    protected float walkSpeed = 1.5f;
    //Walk

    //Run
    protected const int RunCount = 3;
    protected int curRunCount = 0;
    protected float runTimer = 0.0f;
    protected float runTime = 1.0f;
    protected float runSpeed = 5.0f;
    protected bool isRun = false;
    protected Vector3 runTarget = Vector3.zero;
    const float TargetRange = 0.5f;
    //Run

    //OnTrigger
    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        if (!GameMgr.Inst.hasBoss) return; // 깜빡일때 안맞기

        base.OnTriggerEnter2D(coll);
    }
    //OnTrigger

    //OnCollision
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!GameMgr.Inst.hasBoss) return;

        //보통 보스전일때 player의 collier는 isTrigger.false기 때문에 여기서(Collsion) 구현하지만
        //isTrigger.true일 경우도 있는데 base.OnTriggerEnter2D()에 구현되어 있다.
        if (coll.gameObject.CompareTag("Player"))
            GameMgr.Inst.player.TakeDamage(collDmg);
        else if (coll.gameObject.CompareTag("BattleRing") && isRun)
            isRun = false;
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        //1. Player가 Ring Collider에 계속 갇혀 있으면
        //Enter에서 isRun=false 해주는것 만으로는
        //RunToPlayer()에서 탈출시키기에 부족함.

        //2. 1번 조건으로는 보스만 벽에 박았을 때 바로 false 되면서 Run 횟수가 바로 줄어드는 문제가 있음.
        //정확하진 않지만 벽과 보스가 끼어있는 경우를 벽과 보스 사이의 거리가 TargetRange보다 작을때로 보고
        //이때 false 하기로 함
        float dist = Vector3.Distance(transform.position, GameMgr.Inst.player.transform.position);
        if (coll.gameObject.CompareTag("BattleRing") && isRun && dist <= TargetRange)
            isRun = false;
    }
    //OnCollision

    protected virtual void InitBoss()
    {
        monType = MonsterType.BossMon;

        StartCoroutine(BlinkBoss());
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

        BattleSetting();
    }

    void BattleSetting()
    {
        GameMgr.Inst.hasBoss = true;
        coll.enabled = true;
    }

    protected void RunToPlayer()
    {
        rigid.MovePosition(transform.position + moveDir * moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, runTarget) <= TargetRange)
            isRun = false;
    }

    protected void Flip()
    {
        if (transform.position.x > GameMgr.Inst.player.transform.position.x)
            spRenderer.flipX = false;
        else
            spRenderer.flipX = true;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        GameMgr.Inst.UpdateBossHpBar(curHp / bossHp);

        if (curHp <= 0) MonsterDie();
    }

    protected override void MonsterDie()
    {
        isDie = true;
        GameMgr.Inst.GameOver();
    }
}