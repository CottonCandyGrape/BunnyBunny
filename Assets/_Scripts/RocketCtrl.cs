using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCtrl : Weapon
{
    public Transform MonsterPool = null;
    public Transform RocketPool = null;
    public GameObject RocketPrefab = null;
    public GameObject ExpEffectPrefab = null;
    GameObject rocket = null;
    GameObject expEffect = null;

    const float RocketOffset = 0.35f;
    const float BombScaler = 1.3f;

    void Start()
    {
        InitRockets();
    }

    void InitRockets()
    {
        if (rocket == null)
        {
            rocket = Instantiate(RocketPrefab, RocketPool);
            rocket.SetActive(false);
        }

        if(expEffect == null)
        {
            expEffect = Instantiate(ExpEffectPrefab, RocketPool);
            expEffect.SetActive(false);
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
        else //TODO : Elite, Boss는 어떻게 할까? normon없을때는 up이군.
        { }

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
        float radius = Mathf.Max(expEffect.transform.localScale.x, expEffect.transform.localScale.y) / 2.0f;
        Collider2D[] colls = Physics2D.OverlapCircleAll(rocketObj.transform.position, radius);
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].tag.Contains("Monster"))
            {
                MonsterCtrl monCtrl = colls[i].GetComponent<MonsterCtrl>();
                monCtrl.TakeDamage((curLevel + 1) * 30);
            }
            else continue;
        }

        rocketObj.SetActive(false);
        expEffect.SetActive(true);
        expEffect.transform.position = rocketObj.transform.position;
    }

    public override void LevelUpWeapon()
    {
        if (MaxLevel <= curLevel) 
        {
            EvolveWeapon();
            return;
        }

        curLevel++;
        expEffect.transform.localScale *= BombScaler;
    }

    public override void EvolveWeapon()
    {
        isEvolve = true;
    }
}