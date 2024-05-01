using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPoolMgr : MonoBehaviour
{
    Transform norMonPool = null;
    Transform bulletPool = null;
    Transform bulletEffectPool = null;

    public GameObject[] NorMonPrefs1 = null; //Stage_1
    public GameObject[] NorMonPrefs2 = null; //Stage_2
    public GameObject[] NorMonPrefs3 = null; //Stage_3
    List<GameObject[]> norMonList;

    List<MonsterCtrl> MonCtrlPool = new List<MonsterCtrl>();
    [HideInInspector] public int ActiveMonsterCount = 0;

    public GameObject[] BulletPrefabs = null;
    List<BulletCtrl> BulletCtrlPool = new List<BulletCtrl>();

    public GameObject[] BulletEffectPrefabs = null;
    List<GameObject> BulletEffectCtrlPool = new List<GameObject>();

    int initMonCnt = 30;
    int initBltCnt = 10;
    int initBltEftCnt = 10;
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
        bulletEffectPool = GameObject.Find("BulletEffectPool").GetComponent<Transform>();

        //현재 스테이지 초기화 //TODO : 안전한가?
        curStage = AllSceneMgr.Instance.CurStageNum;
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
            GameObject blt = Instantiate(BulletPrefabs[(int)GameMgr.Inst.player.AttackType], bulletPool);
            blt.SetActive(false);
            BulletCtrlPool.Add(blt.GetComponent<BulletCtrl>());
        }

        //BulletEffectPool
        for (int i = 0; i < initBltEftCnt; i++)
        {
            GameObject bltEft = Instantiate(BulletEffectPrefabs[(int)GameMgr.Inst.player.AttackType], bulletEffectPool);
            bltEft.SetActive(false);
            BulletEffectCtrlPool.Add(bltEft);
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

        GameObject blt = Instantiate(BulletPrefabs[(int)GameMgr.Inst.player.AttackType], bulletPool);
        blt.SetActive(false);
        BulletCtrl bltCtrl = blt.GetComponent<BulletCtrl>();
        BulletCtrlPool.Add(bltCtrl);

        return bltCtrl;
    }

    public GameObject AddBulletEffectPool() //Pool에 bulletEffect 추가 or bulletEffect return
    {
        for (int i = 0; i < BulletEffectCtrlPool.Count; i++)
        {
            if (!BulletEffectCtrlPool[i].gameObject.activeSelf)
                return BulletEffectCtrlPool[i];
        }

        GameObject bltEft = Instantiate(BulletEffectPrefabs[(int)GameMgr.Inst.player.AttackType], bulletEffectPool);
        bltEft.SetActive(false);
        BulletEffectCtrlPool.Add(bltEft);

        return bltEft;
    }

    public void OffAllNorMon()
    {
        for (int i = 0; i < norMonPool.childCount; i++)
        {
            GameObject mon = norMonPool.GetChild(i).gameObject;
            if (mon.activeSelf)
                mon.SetActive(false);
        }

        ActiveMonsterCount = 0;
    }
}