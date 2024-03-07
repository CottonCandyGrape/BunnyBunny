using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCtrl : MonoBehaviour
{
    float degree = 0.0f;
    public float Degree
    {
        get { return degree; }
        set { degree = value; }
    }

    float rotSpeed = 150.0f; //player 주변 회전 속도
    float spinSpeed = 100.0f; //Guard 회전 속도
    float radius = 1.0f;
    Vector2 center = Vector2.zero;
    Vector2 guardPos = Vector2.zero;

    void Start() { }

    void Update()
    {
        RotateGuardians();
    }

    void RotateGuardians()
    {
        degree += rotSpeed * Time.deltaTime;
        if (360.0f < degree)
            degree -= 360.0f;

        center = GameMgr.Inst.player.transform.position;

        guardPos.x = Mathf.Sin(degree * Mathf.Deg2Rad);
        guardPos.y = Mathf.Cos(degree * Mathf.Deg2Rad);

        transform.position = center + guardPos * radius;

        transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime); //TODO : 없어도 될수도
    }
}