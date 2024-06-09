using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlingAlienCtrl : BossMonCtrl
{
    float crawlingHp = 4500.0f;

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
                    WalkAction(ActionState.Run);
                    break;
                }

            case ActionState.Run:
                {
                    RunAction();
                    break;
                }
        }
    }
}