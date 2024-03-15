using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPoolMgr : MonoBehaviour
{
    Transform MonsterPool = null;
    Transform BulletPool = null;

    public GameObject[] NorMonPref1 = null;
    public GameObject[] NorMonPref2 = null;
    public GameObject[] NorMonPref3 = null;
    List<GameObject[]> norMonList;

    List<MonsterCtrl> MonCtrlPool = new List<MonsterCtrl>();
    [HideInInspector] public int ActiveMonsterCount = 0;

    public GameObject[] BulletPrefabs = null;
    List<BulletCtrl> BulletCtrlPool = new List<BulletCtrl>();

    int initPoolCount = 30;
    int curStage = 0;

    public static MemoryPoolMgr Inst = null;

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        MonsterPool = GameObject.Find("MonsterPool").GetComponentInChildren<Transform>();
        BulletPool = GameObject.Find("BulletPool").GetComponentInChildren<Transform>();

        //현재 스테이지 초기화 //TODO : 안전한가?
        curStage = GameMgr.Inst.StageNum;
        norMonList = new List<GameObject[]> { NorMonPref1, NorMonPref2, NorMonPref3 };

        //MonsterPool
        for (int i = 0; i < initPoolCount; i++) 
        {
            int idx = Random.Range(0, norMonList[curStage].Length);
            GameObject mon = Instantiate(norMonList[curStage][idx], MonsterPool);
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

    public MonsterCtrl AddMonsterPool(int stage) //Pool에 norm 몬스터 추가 or Mon return
    {
        ActiveMonsterCount++;

        for (int i = 0; i < MonCtrlPool.Count; i++)
        {
            if (!MonCtrlPool[i].gameObject.activeSelf)
                return MonCtrlPool[i];
        }

        int idx = Random.Range(0, norMonList[curStage].Length);
        GameObject mon = Instantiate(norMonList[curStage][idx], MonsterPool);
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