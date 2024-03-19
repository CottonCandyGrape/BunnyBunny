using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRing : MonoBehaviour
{
    SpriteRenderer[] spRenders = null; //64개. o.k.

    void Start()
    {
        spRenders = GetComponentsInChildren<SpriteRenderer>();
        StartCoroutine(BlinkBattleRing());
    }

    //void Update() { }

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