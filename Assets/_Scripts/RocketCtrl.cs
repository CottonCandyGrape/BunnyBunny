using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCtrl : BulletCtrl
{
    [HideInInspector] public static int level = 0;
    [HideInInspector] public static bool evolve = false;

    float bombRadius = 0.0f;

    void OnEnable()
    {
        moveSpeed = 5.0f;
        lifeTime = 8.0f;
        bombRadius = (ScreenMgr.InitScMax.x - ScreenMgr.InitScMin.x) / 5.0f;
    }

    void Start() { }

    protected override void Update()
    {
        base.Update();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.Contains("Monster"))
        {
            //TODO : 폭발 이펙트 추가 (시각, 소리) 
            Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, bombRadius);
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i].tag.Contains("Monster"))
                {
                    MonsterCtrl monCtrl = colls[i].GetComponent<MonsterCtrl>();
                    monCtrl.TakeDamage(40); //TODO : 로켓 데미지 정하기
                }
                else continue;
            }

            gameObject.SetActive(false);
        }
    }
}