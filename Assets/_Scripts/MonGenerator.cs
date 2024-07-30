using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonGenerator : MonoBehaviour
{
    Transform monsterPool = null;

    public GameObject[] EliteMonPrefs = null;
    public GameObject[] BossMonPrefs = null;

    int curStage = 0;
    float spawnTime = 0.0f;
    int monLimit = 30;
    public int MonLimit { get { return monLimit; } set { monLimit = value; } }
    public const int MaxMonCnt = 60;

    float shipSpawnTime = 5.0f;
    float shipSpawnTimer = 5.0f;
    Vector2[] shipDir = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    const float shipPosX = 7.5f;
    const float shipPosY = 8.5f;

    void Start()
    {
        spawnTime = Random.Range(0.1f, 0.3f);

        monsterPool = GameObject.Find("MonsterPool").GetComponent<Transform>();

        curStage = AllSceneMgr.Instance.CurStageNum;
    }

    void Update()
    {
        if (!GameMgr.Inst.hasRing) SpawnNormalMon();
    }

    void SpawnNormalMon()
    {
        spawnTime -= Time.deltaTime;

        if (spawnTime <= 0.0f && MemoryPoolMgr.Inst.ActiveMonsterCount < monLimit)
        {
            spawnTime = GetSpawnTime();
            MonsterCtrl monCtrl = MemoryPoolMgr.Inst.AddMonsterPool();
            monCtrl.gameObject.SetActive(true);
            monCtrl.transform.position = GameMgr.Inst.player.transform.position + GetMonSpawnPos();
        }

        if (AllSceneMgr.Instance.CurStageNum == 2)
        {
            shipSpawnTimer -= Time.deltaTime;

            if (shipSpawnTimer <= 0.0f && MemoryPoolMgr.Inst.ActiveMonsterCount < monLimit)
            {
                shipSpawnTimer = shipSpawnTime;
                Vector2 pos = Vector2.zero;
                Vector2 dir = Vector2.zero;
                GetShipSpawnPos(ref pos, ref dir);

                MonsterCtrl monCtrl = MemoryPoolMgr.Inst.AddSpaceshipPool();
                monCtrl.gameObject.SetActive(true);
                monCtrl.SpaceshipInit(pos, dir);
            }
        }
    }

    float GetSpawnTime()
    {
        float sTime = Random.Range(0.1f, 0.3f);
        switch (monLimit)
        {
            case 30:
                sTime = Random.Range(0.1f, 0.3f);
                break;
            case 40:
                sTime = Random.Range(0.1f, 0.2f);
                break;
            case 50:
                sTime = Random.Range(0.0f, 0.2f);
                break;
            case 60:
                sTime = Random.Range(0.0f, 0.1f);
                break;
        }

        if (AllSceneMgr.Instance.Difficulty == 1)
            return sTime + 0.15f;

        return sTime;
    }

    void GetShipSpawnPos(ref Vector2 pos, ref Vector2 dir)
    {
        int idx = Random.Range(0, shipDir.Length);

        if (idx == 0)
        {
            pos.x = Random.Range(-6.0f, 6.0f);
            pos.y = -shipPosY;
        }
        else if (idx == 1)
        { 
            pos.x = Random.Range(-6.0f, 6.0f);
            pos.y = shipPosY;
        }
        else if(idx == 2)
        {
            pos.x = shipPosX;
            pos.y = Random.Range(-7.0f, 7.0f);
        }
        else if(idx == 3)
        {
            pos.x = -shipPosX;
            pos.y = Random.Range(-7.0f, 7.0f);
        }

        dir = shipDir[idx];
    }

    public Vector3 GetMonSpawnPos() //MonsterCtrl 에서 쓸거라서 public
    {
        Vector3 pos = Vector3.zero;
        float radius = Random.Range(5.0f, 7.0f);
        float sinDeg = Random.Range(0f, 360f);
        float cosDeg = Random.Range(0f, 360f);

        pos.x = Mathf.Cos(cosDeg * Mathf.Deg2Rad);
        pos.y = Mathf.Sin(sinDeg * Mathf.Deg2Rad);
        pos *= radius;

        return pos;
    }

    public void SpawnEliteMon()
    {
        GameObject eliteMon = Instantiate(EliteMonPrefs[curStage], monsterPool);
        eliteMon.transform.position = GameMgr.Inst.player.transform.position + GetMonSpawnPos();
    }

    public void SpawnBossMon(Vector2 spawnPos)
    {
        GameObject bossMon = Instantiate(BossMonPrefs[curStage], monsterPool);
        bossMon.transform.position = spawnPos;
    }
}