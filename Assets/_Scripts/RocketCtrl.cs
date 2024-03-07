using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCtrl : Weapon
{
    public Transform MonsterPool = null;
    public Transform RocketPool = null;
    public GameObject RocketPrefab = null;
    GameObject rocket = null;

    const float RocketOffset = 0.35f;
    const float BombScaler = 1.1f;

    float bombRadius = 0.0f;

    void Start()
    {
        bombRadius = (ScreenMgr.InitScMax.x - ScreenMgr.InitScMin.x) / 5.0f;

        InitRockets();
    }

    void InitRockets()
    {
        if (rocket == null)
        {
            rocket = Instantiate(RocketPrefab, RocketPool);
            rocket.SetActive(false);
        }
    }

    Vector3 GetCloseTarget()
    {
        Vector3 target = Vector3.up;

        MonsterCtrl[] monsters = MonsterPool.GetComponentsInChildren<MonsterCtrl>();
        if (monsters.Length > 0) //TODO : monsters 개수가 많아지면 어떻게 할까? 최적화?
        {
            Vector3 playerPos = GameMgr.Inst.player.transform.position;
            target = monsters[0].transform.position;

            float dist = Vector3.Distance(playerPos, target);
            for (int i = 1; i < monsters.Length; i++)
            {
                float tmp = Vector3.Distance(playerPos, monsters[i].transform.position);
                if (tmp < dist)
                {
                    dist = tmp;
                    target = monsters[i].transform.position;
                }
            }
        }

        return target;
    }

    public void FireRocket()
    {
        Vector3 target = GetCloseTarget();
        Vector3 pos = GameMgr.Inst.player.transform.position;
        Vector3 dir = (target - pos).normalized;

        if (!rocket.activeSelf)
        {
            rocket.SetActive(true);

            BulletCtrl rocketBlt = rocket.GetComponent<BulletCtrl>();
            rocketBlt.MoveDir = dir;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rocketBlt.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            rocketBlt.transform.position = pos + dir * RocketOffset;
        }
    }

    public void ExploseRocket(GameObject rocketObj)
    {
        //TODO : 폭발 이펙트 추가 (시각, 소리) 
        Collider2D[] colls = Physics2D.OverlapCircleAll(rocketObj.transform.position, bombRadius);
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].tag.Contains("Monster"))
            {
                MonsterCtrl monCtrl = colls[i].GetComponent<MonsterCtrl>();
                monCtrl.TakeDamage((CurLevel + 1) * 30);
            }
            else continue;
        }

        rocketObj.SetActive(false);
    }

    public override void LevelUpWeapon()
    {
        if (MaxLevel <= CurLevel) return;

        CurLevel++;
        bombRadius *= BombScaler;
    }

    public override void EvolveWeapon()
    {

    }
}