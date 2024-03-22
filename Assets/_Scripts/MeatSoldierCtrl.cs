using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState { None, Walk, Run, Shot } //일단 Run까지만 구현하자

public class MeatSoldierCtrl : MonsterCtrl
{
    public ActionState actState = ActionState.None;

    float bossHp = 1000;
    float collDmg = 30;
    float walkTimer = 0.0f;
    float walkTime = 5.0f;

    //float runWaitTimer = 0.0f; 
    //float runWaitTime = 0.5f;

    bool isDie = false;

    //public bool isRun = false;

    CapsuleCollider2D capColl = null;

    //WaitForSeconds waitRunSec = new WaitForSeconds(0.5f);
    //Coroutine runCo = null;

    void Start()
    {
        capColl = GetComponent<CapsuleCollider2D>();

        InitBoss();
    }

    void FixedUpdate()
    {
        if (!GameMgr.Inst.hasBoss) return; //깜빡일때 안움직이기

        if (actState == ActionState.Walk)
            base.Move();
        //else if (actState == ActionState.Run) //TODO :
        //{
        //    if (runCo == null && !isRun)
        //        runCo = StartCoroutine(RunAction());
        //}
        //else if (actState == ActionState.Shot)
        // TODO : 행동 패턴 만들기
    }

    void Update()
    {
        UpdateActionState();
    }

    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        if (!GameMgr.Inst.hasBoss) return; // 깜빡일때 안맞기

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
        capColl.enabled = false;

        runCo = StartCoroutine(BlinkBoss());
    }

    void UpdateActionState()
    {
        if (isDie) return; //죽으면 return

        if (!GameMgr.Inst.hasBoss) return; //깜빡일 땐 return 

        if (actState == ActionState.None)
        {
            actState = ActionState.Walk;
            walkTimer = 5.0f;
        }

        switch (actState)
        {
            case ActionState.Walk:
                {
                    walkTimer -= Time.deltaTime;
                    if (walkTimer < 0.0f)
                    {
                        walkTimer = walkTime;

                        actState = ActionState.Run;
                        animator.SetBool("Run", true);
                    }
                    break;
                }

            case ActionState.Run:
                {
                    //TODO : 3번 추격 끝나고 나서 상태 바꾸기
                    //if (runCo != null && !isRun)
                    //{
                    //    runCo = null;
                    //    //actState = ActionState.Shot;
                    //    actState = ActionState.Walk;
                    //    animator.SetBool("Run", false);
                    //}
                    break;
                }

                //case ActionState.Shot: //TODO : 보류 
                //    {
                //        actState = ActionState.Walk;
                //        break;
                //    }
        }
    }

    //IEnumerator RunAction() //TODO 
    //{
    //    int dashCount = 3;
    //    moveSpeed = 3.0f;
    //    isRun = true;

    //    for (int i = 0; i < dashCount; i++)
    //    {
    //        yield return waitRunSec;

    //        //TODO : flip 도 해야함
    //        Vector3 target = GameMgr.Inst.player.transform.position;
    //        moveDir = target - transform.position;
    //        moveDir.Normalize();

    //        float lVal = 0.0f;
    //        while (lVal <= 1.0f)
    //        {
    //            yield return null;
    //            lVal += Time.deltaTime;
    //            //rigid.MovePosition(transform.position + moveDir * moveSpeed * Time.deltaTime);
    //            rigid.MovePosition(transform.position + moveDir * Time.deltaTime);
    //            //transform.position = Vector3.Lerp(transform.position, target, lVal);
    //        }
    //    }

    //    isRun = false;
    //}

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
        if (runCo != null)
            runCo = null;
    }

    void BattleSetting()
    {
        GameMgr.Inst.hasBoss = true;
        capColl.enabled = true;
    }
}