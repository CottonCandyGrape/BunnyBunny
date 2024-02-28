using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCtrl : BulletCtrl
{
    [HideInInspector] public static int level = 0;
    [HideInInspector] public static bool evolve = false;

    void OnEnable()
    {
        moveSpeed = 5.0f;
        lifeTime = 8.0f;
    }

    void Start() { }

    protected override void Update()
    {
        base.Update();
    }
}