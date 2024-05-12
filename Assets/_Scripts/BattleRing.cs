using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRing : MonoBehaviour
{
    SpriteRenderer[] spRenders = null;

    float OffsetX = 4.5f; //left, right x축 거리
    float OffsetY = 5.0f; //up, down y축 거리

    float dmgTime = 0.5f;
    float dmgTimer = 0.0f;

    void Start()
    {
        spRenders = GetComponentsInChildren<SpriteRenderer>();

        SetRingOffset();
        StartCoroutine(BlinkBattleRing());
    }

    void FixedUpdate()
    {
        EscapeInColl();
    }

    //void Update() { }

    //OnCollision
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player") && GameMgr.Inst.hasBoss)
            EnterRing();
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player") && GameMgr.Inst.hasBoss)
            StayRing();
    }
    //OnCollision

    //OnTrigger
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player") && GameMgr.Inst.hasBoss)
            EnterRing();
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player") && GameMgr.Inst.hasBoss)
            StayRing();
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player") && GameMgr.Inst.hasBoss)
            GameMgr.Inst.player.TrapBossRing(true);
    }
    //OnTrigger

    void SetRingOffset()
    {
        if (GameMgr.Inst.MType == MapType.Ground)
            OffsetX = 4.5f;
        else if (GameMgr.Inst.MType == MapType.Vertical)
            OffsetX = 2.75f;

        OffsetY = 5.0f;
    }

    void EnterRing()
    {
        GameMgr.Inst.player.TakeDamage(5);
        dmgTimer = 0.0f;
    }

    void StayRing()
    {
        dmgTimer += Time.deltaTime;
        if (dmgTime <= dmgTimer)
        {
            GameMgr.Inst.player.TakeDamage(5);
            dmgTimer = 0.0f;
        }
    }

    void EscapeInColl()
    {
        if (GameMgr.Inst.player.transform.position.x <= transform.position.x - OffsetX ||
            GameMgr.Inst.player.transform.position.x >= transform.position.x + OffsetX ||
            GameMgr.Inst.player.transform.position.y <= transform.position.y - OffsetY ||
            GameMgr.Inst.player.transform.position.y >= transform.position.y + OffsetY)
            GameMgr.Inst.player.TrapBossRing(false);
    }

    IEnumerator BlinkBattleRing()
    {
        Color clr = Color.white;
        float speed = 2.5f;
        int blinkTimes = 3;

        for (int i = 0; i < blinkTimes; i++)
        {
            while (0.0f <= clr.a)
            {
                clr.a -= speed * Time.deltaTime;
                for (int j = 0; j < spRenders.Length; j++)
                    spRenders[j].color = clr;
                yield return null;
            }

            while (clr.a <= 1.0f)
            {
                clr.a += speed * Time.deltaTime;
                for (int j = 0; j < spRenders.Length; j++)
                    spRenders[j].color = clr;
                yield return null;
            }
        }
    }
}