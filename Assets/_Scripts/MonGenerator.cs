using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonGenerator : MonoBehaviour
{
    float spawnTime = 0.0f;
    int monLimit = 20;

    void Start()
    {
        spawnTime = Random.Range(0.1f, 0.3f);
    }

    void Update()
    {
        spawnTime -= Time.deltaTime;

        if (spawnTime <= 0.0f && MemoryPoolMgr.Inst.ActiveMonsterCount < monLimit)
        {
            spawnTime = Random.Range(0.1f, 0.3f);
            MonsterCtrl monCtrl = MemoryPoolMgr.Inst.AddMonsterPool(GameMgr.Inst.StageNum); //TODO : 꼭! StageNum 안전한지 확인해야함.
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
}