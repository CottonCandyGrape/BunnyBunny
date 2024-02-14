using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPoolMgr : MonoBehaviour
{
    Transform MonsterPool = null;
    Transform BulletPool = null;

    public GameObject[] MonsterPrefabs = null;
    List<MonsterCtrl> MonCtrlPool = new List<MonsterCtrl>();
    [HideInInspector] public int ActiveMonsterCount = 0;

    public GameObject[] BulletPrefabs = null;
    List<BulletCtrl> BulletCtrlPool = new List<BulletCtrl>();

    int initPoolCount = 30;

    public static MemoryPoolMgr Inst = null;

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        MonsterPool = GameObject.Find("MonsterPool").GetComponentInChildren<Transform>();
        BulletPool = GameObject.Find("BulletPool").GetComponentInChildren<Transform>();

        //MonsterPool
        for (int i = 0; i < initPoolCount; i++)
        {
            GameObject mon = Instantiate(MonsterPrefabs[0], MonsterPool);
            mon.SetActive(false);
            MonCtrlPool.Add(mon.GetComponent<MonsterCtrl>());
        }

        //BulletPool
        for (int i = 0; i < initPoolCount; i++)
        {
            GameObject blt = Instantiate(BulletPrefabs[0], BulletPool);
            blt.SetActive(false);
            BulletCtrlPool.Add(blt.GetComponent<BulletCtrl>());
        }
    }

    //TODO : Normal Mon, Elite, Boss Mon 스폰 시키는 코드 추가하기
    public MonsterCtrl AddMonsterPool() //Pool에 norm 몬스터 추가 or Mon return
    {
        ActiveMonsterCount++;

        for (int i = 0; i < MonCtrlPool.Count; i++)
        {
            if (!MonCtrlPool[i].gameObject.activeSelf)
                return MonCtrlPool[i];
        }

        GameObject mon = Instantiate(MonsterPrefabs[0], MonsterPool);
        mon.SetActive(false);
        MonsterCtrl monCtrl = mon.GetComponent<MonsterCtrl>();
        MonCtrlPool.Add(monCtrl);

        return monCtrl;
    }

    public BulletCtrl AddBulletPool() //Pool에 bullet 추가 or bullet return
    {
        for (int i = 0; i < BulletCtrlPool.Count; i++)
        {
            if (!BulletCtrlPool[i].gameObject.activeSelf)
                return BulletCtrlPool[i];
        }

        GameObject blt = Instantiate(BulletPrefabs[0], BulletPool);
        blt.SetActive(false);
        BulletCtrl bltCtrl = blt.GetComponent<BulletCtrl>();
        BulletCtrlPool.Add(bltCtrl);

        return bltCtrl;
    }
}