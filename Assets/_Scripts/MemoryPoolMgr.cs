using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPoolMgr : MonoBehaviour
{
    Transform norMonPool = null;
    Transform bulletPool = null;

    public GameObject[] NorMonPrefs1 = null;
    public GameObject[] NorMonPrefs2 = null;
    public GameObject[] NorMonPrefs3 = null;
    List<GameObject[]> norMonList;

    List<MonsterCtrl> MonCtrlPool = new List<MonsterCtrl>();
    [HideInInspector] public int ActiveMonsterCount = 0;

    public GameObject[] BulletPrefabs = null;
    List<BulletCtrl> BulletCtrlPool = new List<BulletCtrl>();

    int initMonCnt = 30;
    int initBltCnt = 10;
    int curStage = 0;

    public static MemoryPoolMgr Inst = null;

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        norMonPool = GameObject.Find("NorMonPool").GetComponent<Transform>();
        bulletPool = GameObject.Find("BulletPool").GetComponent<Transform>();

        //현재 스테이지 초기화 //TODO : 안전한가?
        curStage = GameMgr.Inst.StageNum;
        norMonList = new List<GameObject[]> { NorMonPrefs1, NorMonPrefs2, NorMonPrefs3 };

        //norMonPool
        for (int i = 0; i < initMonCnt; i++) 
        {
            int idx = Random.Range(0, norMonList[curStage].Length);
            GameObject mon = Instantiate(norMonList[curStage][idx], norMonPool);
            mon.SetActive(false);
            MonCtrlPool.Add(mon.GetComponent<MonsterCtrl>());
        }

        //BulletPool
        for (int i = 0; i < initBltCnt; i++)
        {
            GameObject blt = Instantiate(BulletPrefabs[0], bulletPool);
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
        GameObject mon = Instantiate(norMonList[curStage][idx], norMonPool);
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

        GameObject blt = Instantiate(BulletPrefabs[0], bulletPool);
        blt.SetActive(false);
        BulletCtrl bltCtrl = blt.GetComponent<BulletCtrl>();
        BulletCtrlPool.Add(bltCtrl);

        return bltCtrl;
    }
}