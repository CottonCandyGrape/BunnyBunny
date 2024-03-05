using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCtrl : MonoBehaviour
{
    [HideInInspector] public static int level = 0;
    [HideInInspector] public static bool evolve = false;

    public Transform MonsterPool = null;
    public Transform RocketPool = null;
    public GameObject RocketPrefab = null;
    List<BulletCtrl> rocketList = new List<BulletCtrl>();

    const int RocketInitCount = 5;
    const float RocketOffset = 0.35f;
    float bombRadius = 0.0f;

    void Start()
    {
        bombRadius = (ScreenMgr.InitScMax.x - ScreenMgr.InitScMin.x) / 5.0f;

        InitRockets();
    }

    void InitRockets()
    {
        for (int i = 0; i < RocketInitCount; i++)
        {
            GameObject rocket = Instantiate(RocketPrefab, RocketPool);
            rocket.SetActive(false);
            rocketList.Add(rocket.GetComponent<BulletCtrl>());
        }
    }

    Vector3 GetCloseTarget() {
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

        for (int i = 0; i < RocketPool.childCount; i++)
        {
            GameObject rkt = RocketPool.GetChild(i).gameObject;
            if (!rkt.activeSelf)
            {
                rkt.SetActive(true);

                BulletCtrl rocketBlt = rkt.GetComponent<BulletCtrl>();
                rocketBlt.MoveDir = dir;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                rocketBlt.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                rocketBlt.transform.position = pos + dir * RocketOffset;
                return;
            }
            else continue;
        }
        // 발사주기를 생각했을때 꺼진게 없으면 안되는데(5개 까지도 필요없음)
        // 일단 꺼진게 없으면 발사 안함. TODO : 어케 해야할지 생각하기 
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
                if (monCtrl != null)
                    monCtrl.TakeDamage(40); //TODO : 로켓 데미지 정하기
            }
            else continue;
        }

        rocketObj.SetActive(false);
    }
}