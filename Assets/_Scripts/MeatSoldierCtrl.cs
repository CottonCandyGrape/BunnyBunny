using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatSoldierCtrl : BossMonCtrl
{
    ActionState actState = ActionState.None;

    public GameObject MeatBullet = null;
    float MeatSolHp = 1500.0f;

    //ShotTimer

    void Start()
    {
        if (coll == null)
            coll = GetComponent<CapsuleCollider2D>();

        InitBoss();
    }

    void FixedUpdate() //충돌이 있으니깐 움직이는 것을 여기서 구현
    {
        if (!GameMgr.Inst.hasBoss) return; //깜빡일때 안움직이기

        if (actState == ActionState.Walk)
            base.Move();
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
        bossHp = MeatSolHp;
        curHp = MeatSolHp;
        moveSpeed = walkSpeed;

        MemoryPoolMgr.Inst.InitMeatBulletPool();

        base.InitBoss();
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
                        ShotTimer = ShotTime;
                        actState = ActionState.Shot;
                    }
                    break;
                }

            case ActionState.Shot:
                {
                    Flip();

                    ShotTimer -= Time.deltaTime;
                    if (ShotTimer < 0.0f)
                    {
                        curShotCount++;

                        if (ShotCount < curShotCount)
                        {
                            curShotCount = 0;
                            walkTimer = walkTime;
                            actState = ActionState.Walk;
                            return;
                        }

                        ShotMeatBullets();
                        ShotTimer = ShotTime;
                    }
                    break;
                }
        }
    }

    void ShotMeatBullets()
    {
        for (int i = 0; i < 10; i++)
        {
            BulletCtrl blt = MemoryPoolMgr.Inst.AddMeatBulletPool();
            blt.gameObject.SetActive(true);

            float rad = (i * 36) * Mathf.Deg2Rad;
            Vector3 vec = blt.MoveDir;
            vec.x = Mathf.Cos(rad); vec.y = Mathf.Sin(rad); vec.z = 0;
            blt.MoveDir = vec.normalized;

            float angle = Mathf.Atan2(blt.MoveDir.y, blt.MoveDir.x) * Mathf.Rad2Deg;
            blt.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            blt.transform.position = transform.position;
        }
    }
}