using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCtrl : MonoBehaviour
{
    [HideInInspector] public static int level = 0;
    [HideInInspector] public static bool evolve = false;

    float degree = 0.0f;
    public float Degree
    {
        get { return degree; }
        set { degree = value; }
    }

    float rotSpeed = 150.0f; //player 주변 회전 속도
    float radius = 1.0f;
    Vector2 center = Vector2.zero;
    Vector2 guardPos = Vector2.zero;

    void Start() { }

    void Update()
    {
        UpdateGuardPos();
    }

    void OnTriggerEnter2D(Collider2D coll) { }

    void UpdateGuardPos()
    {
        degree += rotSpeed * Time.deltaTime;
        if (360.0f < degree)
            degree -= 360.0f;

        center = GameMgr.Inst.player.transform.position;

        guardPos.x = Mathf.Sin(degree * Mathf.Deg2Rad);
        guardPos.y = Mathf.Cos(degree * Mathf.Deg2Rad);

        transform.position = center + guardPos * radius;
    }
}