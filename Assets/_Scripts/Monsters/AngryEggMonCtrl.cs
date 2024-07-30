using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryEggMonCtrl : MonsterCtrl
{
    float EggHp = 3500.0f;

    void OnEnable()
    {
        if (AllSceneMgr.Instance.Difficulty == 1)
            EggHp = 2500.0f;
        else if (AllSceneMgr.Instance.Difficulty == 3)
            EggHp = 4500.0f;

        curHp = EggHp;
        slowTimer = 0.0f;
        base.Init();
    }

    void Start() { }

    void FixedUpdate()
    {
        base.Move();
        RotateMonster();
    }
}