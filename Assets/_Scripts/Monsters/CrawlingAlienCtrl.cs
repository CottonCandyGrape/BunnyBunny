using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlingAlienCtrl : BossMonCtrl
{
    ActionState actState = ActionState.None;

    float crawlingHp = 3000.0f;

    void Start()
    {
        InitBoss();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (!GameMgr.Inst.hasBoss) return; //깜빡일때 안움직이기

        if (actState == ActionState.Walk)
            base.Move();
        else if (actState == ActionState.Run)
        {
            if (isRun) RunToPlayer();
        }
    }

    void Update()
    {
        UpdateActionState();
    }

    protected override void InitBoss()
    {
        bossHp = crawlingHp;
        curHp = crawlingHp;
        moveSpeed = walkSpeed;

        base.InitBoss();
    }

    protected override IEnumerator BossDie()
    {
        animator.SetBool("Dead", true);
        AnimatorStateInfo animInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!animInfo.IsName("CrawlingAlienMon_Defeated"))
        {
            animInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        yield return StartCoroutine(GameMgr.Inst.ExploseEffect(transform.position));
        yield return delay;

        GameMgr.Inst.GameOver(GameMgr.Inst.stageClear);
    }

    void UpdateActionState()
    {
        if (isDead) return; //죽으면 return

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
                        runTimer = runTime;

                        actState = ActionState.Run;
                    }
                    break;
                }

            case ActionState.Run:
                {
                    if (!isRun) //안달릴때(달리기 준비) 타이머 돌리기
                    {
                        Flip();

                        runTimer -= Time.deltaTime;
                        if (runTimer < 0.0f)
                        {
                            runTimer = runTime;
                            runTarget = GameMgr.Inst.player.transform.position;
                            moveDir = runTarget - transform.position;
                            moveDir.Normalize();

                            moveSpeed = runSpeed;
                            animator.speed = 2.0f;
                            curRunCount++;
                            isRun = true;
                        }
                    }

                    if (RunCount < curRunCount)
                    {
                        curRunCount = 0;
                        isRun = false;
                        isKnockBack = false; //run중에 Guard 맞으면 초기화 돼서 Walk 되자마자 넉백발동 됨.
                        moveSpeed = walkSpeed;
                        animator.speed = 1.0f;
                        actState = ActionState.Walk;
                    }
                    break;
                }
        }
    }
}