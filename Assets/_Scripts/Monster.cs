using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    NormalMon = 0,
    EliteMon,
    BossMon,
}

public class Monster : MonoBehaviour
{
    MonsterType monType = MonsterType.NormalMon;

    float moveSpeed = 1.0f;
    Vector3 moveDir = Vector3.one;
    Vector3 scale = Vector3.one;

    Vector3 spawnPos = Vector3.zero;

    float hp = 100;
    float defense = 10;
    float attack = 10;

    void Start()
    {
        
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        moveDir = GameMgr.inst.player.transform.position - transform.position;
        moveDir.Normalize();

        if (moveDir.x < 0)
            scale.x = 1;
        else
            scale.x = -1;

        transform.localScale = scale;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}