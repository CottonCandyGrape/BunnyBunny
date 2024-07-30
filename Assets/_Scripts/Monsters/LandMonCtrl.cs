using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMonCtrl : MonsterCtrl
{
    const float OffsetX = 0.08f;
    float landMonHp = 2000.0f;

    void OnEnable()
    {
        if (AllSceneMgr.Instance.Difficulty == 1)
            landMonHp = 1000.0f;
        else if (AllSceneMgr.Instance.Difficulty == 3)
            landMonHp = 3000.0f;

        curHp = landMonHp;
        slowTimer = 0.0f;
        base.Init();
    }

    void Start() { monType = MonsterType.EliteMon; }

    void FixedUpdate()
    {
        base.Move();
        SetCollPos();
    }

    void SetCollPos()
    {
        Vector2 tmp = Vector2.zero;
        tmp.x = moveDir.x < 0 ? -OffsetX : OffsetX;
        coll.offset = tmp;
    }
}