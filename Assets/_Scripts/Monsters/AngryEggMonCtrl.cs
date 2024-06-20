using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryEggMonCtrl : MonsterCtrl
{
    float EggHp = 3500.0f;

    void OnEnable()
    {
        slowTimer = 0.0f;
        curHp = EggHp;
        base.Init();
    }

    void Start() { }

    void FixedUpdate()
    {
        base.Move();
        RotateMonster();
    }
}