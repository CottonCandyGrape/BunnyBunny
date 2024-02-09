using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoolMgr : MonoBehaviour
{
    public GameObject[] MonsterPrefabs = null;
    List<MonsterCtrl> MonCtrlPool = new List<MonsterCtrl>();

    int initPoolCount = 30;

    float spawnTime = 0.0f;

    void Start()
    {
        for (int i = 0; i < initPoolCount; i++)
        {
            GameObject mon = Instantiate(MonsterPrefabs[0], transform);
            mon.SetActive(false);
            MonCtrlPool.Add(mon.GetComponent<MonsterCtrl>());
        }
    }

    void Update()
    {
        spawnTime -= Time.deltaTime;

        if (spawnTime <= 0.0f)
        {
            spawnTime = Random.Range(0.1f, 0.3f);

            for (int i = 0; i < MonCtrlPool.Count; i++)
            {
                if (!MonCtrlPool[i].gameObject.activeSelf)
                {
                    MonCtrlPool[i].gameObject.SetActive(true);
                    MonCtrlPool[i].transform.position 
                        = GameMgr.inst.player.transform.position + GetSpawnPos();
                    break;
                }
            }
        }
    }

    Vector3 GetSpawnPos()
    {
        Vector3 pos = Vector3.one;
        float r = Random.Range(3.0f, 5.0f);
        float sDeg = Random.Range(0f, 360f);
        float cDeg = Random.Range(0f, 360f);

        pos.x = Mathf.Cos(cDeg * Mathf.Deg2Rad);
        pos.y = Mathf.Sin(sDeg * Mathf.Deg2Rad);
        pos *= r;

        return pos;
    }

    MonsterCtrl AddMonsterPool() //풀에 norm 몬스터 추가 하기
    {
        for (int i = 0; i < MonCtrlPool.Count; i++)
        {
            if (!MonCtrlPool[i].gameObject.activeSelf)
                return MonCtrlPool[i];
        }

        GameObject mon = Instantiate(MonsterPrefabs[0], transform);
        mon.SetActive(false);
        MonsterCtrl monCtrl = mon.GetComponent<MonsterCtrl>();
        MonCtrlPool.Add(monCtrl);

        return monCtrl;
    }
}