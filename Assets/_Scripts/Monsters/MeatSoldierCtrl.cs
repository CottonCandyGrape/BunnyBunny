using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatSoldierCtrl : BossMonCtrl
{
    public GameObject MeatBullet = null;
    float MeatSolHp = 5000.0f;

    void Start()
    {
        InitBoss();
    }

    void FixedUpdate() //충돌이 있으니깐 움직이는 것을 여기서 구현
    {
        if (isDead) return;

        if (!GameMgr.Inst.hasBoss) return; //깜빡일때 안움직이기

        if (actState == ActionState.Walk)
            base.Move();
        else if (actState == ActionState.Run)
        {
            if (isRun) RunToPlayer();
        }
        else if (actState == ActionState.Shot)
        {
            //지금은 멈춰있기
        }
    }

    void Update() //타이머 같은것들은 여기서 구현
    {
        UpdateActionState();
    }

    protected override void InitBoss()
    {
        if (AllSceneMgr.Instance.Difficulty == 1)
            MeatSolHp = 4000.0f;
        else if (AllSceneMgr.Instance.Difficulty == 3)
            MeatSolHp = 7500.0f;

        bossHp = MeatSolHp;
        curHp = MeatSolHp;
        moveSpeed = walkSpeed;

        MemoryPoolMgr.Inst.InitMeatBulletPool();

        base.InitBoss();
    }

    protected override IEnumerator BossDie()
    {
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
                    int ran = Random.Range(2, 4);
                    WalkAction((ActionState)ran);
                    break;
                }
            case ActionState.Run:
                {
                    RunAction();
                    break;
                }
            case ActionState.Shot:
                {
                    ShotAction();
                    break;
                }
        }
    }
}