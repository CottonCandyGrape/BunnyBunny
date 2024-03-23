using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState { None, Walk, Run, Shot } //일단 Run까지만 구현하자

public class MeatSoldierCtrl : MonsterCtrl
{
    public ActionState actState = ActionState.None; //test 중 private으로 할 것.

    float bossHp = 1000000.0f; //test 중
    float collDmg = 30.0f;
    bool isDie = false;

    //Walk
    float walkTimer = 0.0f;
    float walkTime = 5.0f;
    float walkSpeed = 1.5f;
    //Walk

    //Run
    const int RunCount = 3;
    public int curRunCount = 0;

    float runTimer = 0.0f;
    float runTime = 1.0f;
    float runSpeed = 5.0f;
    bool isRun = false;
    Vector3 runTarget = Vector3.zero;
    const float TargetRange = 0.5f;
    //Run

    CapsuleCollider2D capColl = null;

    void Start()
    {
        capColl = GetComponent<CapsuleCollider2D>();

        InitBoss();
    }

    void FixedUpdate() //충돌이 있으니깐 움직이는 것을 여기서 구현
    {
        if (!GameMgr.Inst.hasBoss) return; //깜빡일때 안움직이기

        if (actState == ActionState.Walk)
            base.Move();
        else if (actState == ActionState.Run)
        {
            if (isRun) RunToPlayer();
        }
        //else if (actState == ActionState.Shot) // TODO : 행동 패턴 만들기
    }

    void Update() //타이머 같은것들은 여기서 구현
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
        moveSpeed = walkSpeed;
        capColl.enabled = false;

        StartCoroutine(BlinkBoss());
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
                        runTimer = runTime;

                        actState = ActionState.Run;
                        animator.SetBool("Run", true);
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
                        actState = ActionState.Walk;
                        animator.SetBool("Run", false);
                    }
                    break;
                }

                //case ActionState.Shot: //TODO : 보류 
                //    {
                //        actState = ActionState.Walk;
                //        break;
                //    }
        }
    }

    void RunToPlayer()
    {
        rigid.MovePosition(transform.position + moveDir * moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, runTarget) <= TargetRange)
            isRun = false;
    }

    void Flip()
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
        capColl.enabled = true;
    }
}