using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPoolMgr : MonoBehaviour
{
    Transform monsterPool = null;
    Transform spaceshipPool = null;
    Transform bulletPool = null;
    Transform meatBulletPool = null;
    Transform evBulletPool = null;
    Transform evSupBulletPool = null;
    Transform bulletEffectPool = null;

    public GameObject[] NorMons = null; //Basic Mon
    public GameObject[] NorMonPrefs1 = null; //Stage_1
    public GameObject[] NorMonPrefs2 = null; //Stage_2
    public GameObject[] NorMonPrefs3 = null; //Stage_3 
    public GameObject[] curNorMonArr = null;
    List<GameObject[]> norMonList;

    List<MonsterCtrl> MonCtrlPool = new List<MonsterCtrl>(); //Normal Monster
    List<MonsterCtrl> SpaceshipPool = new List<MonsterCtrl>(); //Spaceship Monster
    [HideInInspector] public int ActiveMonsterCount = 0;

    public GameObject[] BulletPrefabs = null; //총알
    List<BulletCtrl> BulletCtrlPool = new List<BulletCtrl>();

    public GameObject[] EvBulletPrefabs = null; //진화 총알
    List<BulletCtrl> EvBulletCtrlPool = new List<BulletCtrl>();

    public GameObject[] EvSupBulletPrefabs = null; //진화된 총알 추가 공격 
    List<GameObject> EvSupBulletCtrlPool = new List<GameObject>();

    public GameObject[] BulletEffectPrefabs = null; //총알 이펙트
    List<GameObject> BulletEffectCtrlPool = new List<GameObject>();

    public GameObject MeatBulletPrefabs = null; //MeatBullet
    List<BulletCtrl> MeatBulletCtrlPool = new List<BulletCtrl>();

    int initMonCnt = 30;
    int initShipCnt = 5;
    int initBltCnt = 10;
    int initEvBltCnt = 5;
    int initEvSupBltCnt = 5;
    int initBltEftCnt = 10;
    int initMeatBltCnt = 20;
    int curStage = 0;

    public static MemoryPoolMgr Inst = null;

    void Awake() { Inst = this; }

    void Start()
    {
        monsterPool = GameObject.Find("MonsterPool").GetComponent<Transform>();
        spaceshipPool = GameObject.Find("SpaceshipPool").GetComponent<Transform>();
        bulletPool = GameObject.Find("BulletPool").GetComponent<Transform>();
        meatBulletPool = GameObject.Find("MeatBulletPool").GetComponent<Transform>();
        evBulletPool = GameObject.Find("EvBulletPool").GetComponent<Transform>();
        evSupBulletPool = GameObject.Find("EvSupBulletPool").GetComponent<Transform>();
        bulletEffectPool = GameObject.Find("BulletEffectPool").GetComponent<Transform>();

        curStage = AllSceneMgr.Instance.CurStageNum;
        norMonList = new List<GameObject[]> { NorMonPrefs1, NorMonPrefs2, NorMonPrefs3 };

        if (curStage == 2)
        {
            curNorMonArr = NorMons;
            for (int i = 0; i < initShipCnt; i++)
            {
                int idx = Random.Range(0, NorMonPrefs3.Length);
                GameObject mon = Instantiate(NorMonPrefs3[idx], spaceshipPool);
                mon.SetActive(false);
                SpaceshipPool.Add(mon.GetComponent<MonsterCtrl>());
            }
        }
        else
            curNorMonArr = NorMons.Concat(norMonList[curStage]).ToArray();

        //norMonPool
        for (int i = 0; i < initMonCnt; i++)
        {
            int idx = Random.Range(0, curNorMonArr.Length);
            GameObject mon = Instantiate(curNorMonArr[idx], monsterPool);
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

    public MonsterCtrl AddSpaceshipPool() //Pool에 Spaceship 추가 or return
    {
        ActiveMonsterCount++;

        for (int i = 0; i < SpaceshipPool.Count; i++)
        {
            if (!SpaceshipPool[i].gameObject.activeSelf)
                return SpaceshipPool[i];
        }

        int idx = Random.Range(0, curNorMonArr.Length);
        GameObject mon = Instantiate(curNorMonArr[idx], monsterPool);
        mon.SetActive(false);
        MonsterCtrl monCtrl = mon.GetComponent<MonsterCtrl>();
        SpaceshipPool.Add(monCtrl);

        return monCtrl;
    }

    public MonsterCtrl AddMonsterPool() //Pool에 norm 몬스터 추가 or Mon return
    {
        ActiveMonsterCount++;

        for (int i = 0; i < MonCtrlPool.Count; i++)
        {
            if (!MonCtrlPool[i].gameObject.activeSelf)
                return MonCtrlPool[i];
        }

        int idx = Random.Range(0, curNorMonArr.Length);
        GameObject mon = Instantiate(curNorMonArr[idx], monsterPool);
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

    public void InitEvBulletPool()
    {
        for (int i = 0; i < initEvBltCnt; i++)
        {
            GameObject evBlt = Instantiate(EvBulletPrefabs[(int)GameMgr.Inst.player.AttackType], evBulletPool);
            evBlt.SetActive(false);
            EvBulletCtrlPool.Add(evBlt.GetComponent<BulletCtrl>());
        }
    }

    public BulletCtrl AddEvBulletPool() //Pool에 EvBullet 추가 or EvBullet return
    {
        for (int i = 0; i < EvBulletCtrlPool.Count; i++)
        {
            if (!EvBulletCtrlPool[i].gameObject.activeSelf)
                return EvBulletCtrlPool[i];
        }

        GameObject evBlt = Instantiate(EvBulletPrefabs[(int)GameMgr.Inst.player.AttackType], evBulletPool);
        evBlt.SetActive(false);
        BulletCtrl evBltCtrl = evBlt.GetComponent<BulletCtrl>();
        EvBulletCtrlPool.Add(evBltCtrl);

        return evBltCtrl;
    }

    public void InitEvSupBulletPool()
    {
        for (int i = 0; i < initEvSupBltCnt; i++)
        {
            GameObject evSupBlt = Instantiate(EvSupBulletPrefabs[(int)GameMgr.Inst.player.AttackType], evSupBulletPool);
            evSupBlt.SetActive(false);
            EvSupBulletCtrlPool.Add(evSupBlt);
        }
    }

    public GameObject AddEvSupBulletPool() //Pool에 EvSupBullet 추가 or EvSupBullet return
    {
        for (int i = 0; i < EvSupBulletCtrlPool.Count; i++)
        {
            if (!EvSupBulletCtrlPool[i].gameObject.activeSelf)
                return EvSupBulletCtrlPool[i];
        }

        GameObject evSupBlt = Instantiate(EvSupBulletCtrlPool[(int)GameMgr.Inst.player.AttackType], evSupBulletPool);
        evSupBlt.SetActive(false);
        EvSupBulletCtrlPool.Add(evSupBlt);

        return evSupBlt;
    }

    public void InitMeatBulletPool()
    {
        for (int i = 0; i < initMeatBltCnt; i++)
        {
            GameObject meatBlt = Instantiate(MeatBulletPrefabs, meatBulletPool);
            meatBlt.SetActive(false);
            MeatBulletCtrlPool.Add(meatBlt.GetComponent<BulletCtrl>());
        }
    }

    public BulletCtrl AddMeatBulletPool() //Pool에 MeatBullet 추가 or MeatBullet return
    {
        for (int i = 0; i < MeatBulletCtrlPool.Count; i++)
        {
            if (!MeatBulletCtrlPool[i].gameObject.activeSelf)
                return MeatBulletCtrlPool[i];
        }

        GameObject meatBlt = Instantiate(MeatBulletPrefabs, meatBulletPool);
        meatBlt.SetActive(false);
        BulletCtrl meatBltCtrl = meatBlt.GetComponent<BulletCtrl>();
        MeatBulletCtrlPool.Add(meatBltCtrl);

        return meatBltCtrl;
    }

    public void DestroyAllNorMon()
    {
        for (int i = 0; i < monsterPool.childCount; i++)
        {
            GameObject mon = monsterPool.GetChild(i).gameObject;
            Destroy(mon);
        }

        ActiveMonsterCount = 0;
    }
}