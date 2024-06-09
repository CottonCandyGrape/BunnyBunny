using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBiteMonCtrl : MonsterCtrl
{
    float SkullHp = 2500.0f;

    void OnEnable()
    {
        slowTimer = 0.0f;
        curHp = SkullHp;
        base.Init();
    }

    void Start() { }

    void FixedUpdate()
    {
        base.Move();
        RotateSkull();
    }

    void RotateSkull()
    {
        moveDir.Normalize();
        float angle = Mathf.Atan(moveDir.y / moveDir.x) * Mathf.Rad2Deg;
        spRenderer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}