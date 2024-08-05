using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBiteMonCtrl : MonsterCtrl
{
    float SkullHp = 2500.0f;

    void OnEnable()
    {
        if (AllSceneMgr.Instance.Difficulty == 0)
            SkullHp = 1500.0f;
        else if (AllSceneMgr.Instance.Difficulty == 2)
            SkullHp = 3500.0f;

        curHp = SkullHp;
        slowTimer = 0.0f;
        base.Init();
    }

    void Start() { monType = MonsterType.EliteMon; }

    void FixedUpdate()
    {
        base.Move();
        RotateMonster();
    }
}