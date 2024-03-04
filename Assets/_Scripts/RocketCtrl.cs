using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCtrl : MonoBehaviour
{
    [HideInInspector] public static int level = 0;
    [HideInInspector] public static bool evolve = false;

    float bombRadius = 0.0f;

    public Transform RocketPool = null;
    public GameObject RocketPrefab = null;
    List<RocketCtrl> rocketList = new List<RocketCtrl>();
    const int RocketInitCount = 5;
    const float RocketOffset = 0.35f;

    void OnEnable()
    {
        bombRadius = (ScreenMgr.InitScMax.x - ScreenMgr.InitScMin.x) / 5.0f;
    }

    void Start()
    {
        InitRockets();
    }

    //void Update() { }

    public void ExploseRocket(GameObject rocketObj)
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

        rocketObj.SetActive(false);
    }

    void InitRockets()
    {
        for (int i = 0; i < RocketInitCount; i++)
        {
            GameObject rocket = Instantiate(RocketPrefab, RocketPool);
            rocket.SetActive(false);
            rocketList.Add(rocket.GetComponent<RocketCtrl>());
        }
    }

    public void FireRocket(Vector3 pos, Vector3 dir)
    {
        for (int i = 0; i < RocketPool.childCount; i++)
        {
            GameObject rkt = RocketPool.GetChild(i).gameObject;
            if (!rkt.activeSelf)
            {
                rkt.SetActive(true);

                BulletCtrl rocket = rkt.GetComponent<BulletCtrl>();
                rocket.MoveDir = dir;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                rocket.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                rocket.transform.position = pos + dir * RocketOffset;
                return;
            }
            else continue;
        }
        // 발사주기를 생각했을때 꺼진게 없으면 안되는데(5개 까지도 필요없음)
        // 일단 꺼진게 없으면 발사 안함. TODO : 어케 해야할지 생각하기 
    }
}