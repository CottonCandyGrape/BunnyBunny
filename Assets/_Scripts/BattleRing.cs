using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRing : MonoBehaviour
{
    SpriteRenderer[] spRenders = null;

    const float OffsetX = 4.5f;
    const float OffsetY = 5.0f;

    float dmgTime = 0.5f;
    float dmgTimer = 0.0f;

    void Start()
    {
        spRenders = GetComponentsInChildren<SpriteRenderer>();

        StartCoroutine(BlinkBattleRing());
    }

    void FixedUpdate()
    {
        EscapeInColl();
    }

    //void Update() { }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player") && GameMgr.Inst.hasBoss)
        {
            GameMgr.Inst.player.TakeDamage(5);
            dmgTimer = 0.0f;
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player") && GameMgr.Inst.hasBoss)
        {
            dmgTimer += Time.deltaTime;
            if (dmgTime <= dmgTimer)
            {
                GameMgr.Inst.player.TakeDamage(5);
                dmgTimer = 0.0f;
            }
        }
    }
    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player") && GameMgr.Inst.hasBoss)
            GameMgr.Inst.player.TrapBossRing(true);
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