using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonGenerator : MonoBehaviour
{
    Transform eliteBossPool = null;

    public GameObject[] EliteMonPrefs = null;
    public GameObject[] BossMonPrefs = null;

    float spawnTime = 0.0f;

    int monLimit = 20;
    int curStage = 0;

    void Start()
    {
        spawnTime = Random.Range(0.1f, 0.3f);

        eliteBossPool = GameObject.Find("EliteBossPool").GetComponent<Transform>();

        //현재 스테이지 초기화 //TODO : 안전한가?
        curStage = GameMgr.Inst.StageNum;
    }

    void Update()
    {
        spawnTime -= Time.deltaTime;

        if (spawnTime <= 0.0f && MemoryPoolMgr.Inst.ActiveMonsterCount < monLimit)
        {
            spawnTime = Random.Range(0.1f, 0.3f);
            MonsterCtrl monCtrl = MemoryPoolMgr.Inst.AddMonsterPool(curStage);
            monCtrl.gameObject.SetActive(true);
            monCtrl.transform.position = GameMgr.Inst.player.transform.position + GetMonSpawnPos();
        }
    }

    Vector3 GetMonSpawnPos()
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
        GameObject eliteMon = Instantiate(EliteMonPrefs[curStage], eliteBossPool);
        eliteMon.transform.position = GameMgr.Inst.player.transform.position + GetMonSpawnPos();
    }

    void OffAllEliteMon()
    {
        //혹시 나중에 Elite Mon이 추가 됐을 경우를 대비해 for로 작성
        for (int i = 0; i < eliteBossPool.childCount; i++)
        {
            GameObject elite = eliteBossPool.GetChild(i).gameObject;
            if (elite.activeSelf)
                elite.SetActive(false);
        }
    }

    public void SpawnBossMon(Vector2 spawnPos)
    {
        //현재 Elite Mon 다 끄고
        OffAllEliteMon();

        //Boss Mon Spawn 시키기
        GameObject bossMon = Instantiate(BossMonPrefs[curStage], eliteBossPool);
        bossMon.transform.position = spawnPos;
    }
}