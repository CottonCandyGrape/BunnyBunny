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

    public GameObject NuclearPrefab = null;
    public GameObject Ev_ExpEffectPrefab = null;
    GameObject ev_ExpEffect = null;
    GameObject nuclear = null;

    Vector3 NuclearExpOffset = Vector3.up * 2.0f;
    const float RocketOffset = 0.35f;
    const float ExpScaler = 1.3f;
    const float NuExpRadius = 2.5f;
    float ExpRadius = 0.9f;

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

        if (expEffect == null)
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

    public void FireNuclear()
    {
        Vector3 target = GetCloseTarget();

        if (!nuclear.activeSelf)
        {
            nuclear.SetActive(true);

            BulletCtrl rocketBlt = nuclear.GetComponent<BulletCtrl>();
            rocketBlt.MoveDir = Vector3.down;
            nuclear.transform.position = target + Vector3.up * 5.0f;
        }
    }

    public void ExploseRocket(bool isEvole, GameObject rocketObj)
    {
        float radius = isEvole ? NuExpRadius : ExpRadius;
        int dmg = isEvole ? 50 : 30;

        Collider2D[] colls = Physics2D.OverlapCircleAll(rocketObj.transform.position, radius);
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].tag.Contains("Monster"))
            {
                MonsterCtrl monCtrl = colls[i].GetComponent<MonsterCtrl>();
                monCtrl.TakeDamage((curLevel + 1) * dmg);
            }
            else continue;
        }

        if (isEvole)
        {
            ev_ExpEffect.SetActive(true);
            ev_ExpEffect.transform.position = rocketObj.transform.position - NuclearExpOffset;
            SoundMgr.Instance.PlaySfxSound("nuclear");
        }
        else
        {
            expEffect.SetActive(true);
            expEffect.transform.position = rocketObj.transform.position;
            SoundMgr.Instance.PlaySfxSound("rocket");
        }

        rocketObj.SetActive(false);
    }

    public override void LevelUpWeapon()
    {
        expEffect.transform.localScale *= ExpScaler;
        ExpRadius *= ExpScaler;

        curLevel++;
    }

    public override void EvolveWeapon()
    {
        isEvolve = true;

        //nuclaer 초기화
        if (nuclear == null)
        {
            nuclear = Instantiate(NuclearPrefab, RocketPool);
            nuclear.SetActive(false);
        }

        //ev_ExpEffect 초기화
        if (ev_ExpEffect == null)
        {
            ev_ExpEffect = Instantiate(Ev_ExpEffectPrefab, RocketPool);
            ev_ExpEffect.SetActive(false);
        }
    }

    public override string GetExplainText()
    {
        if (curLevel == 0)
            return "적에게 날아가 폭발.";
        else if (curLevel == 1 || curLevel == 2)
            return "폭발 범위 30% 증가.";
        else if (curLevel == 3)
            return "핵폭발. 폭발 범위 대폭 증가.";
        return string.Empty;
    }
}