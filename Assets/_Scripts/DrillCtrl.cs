using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillCtrl : MonoBehaviour
{
    [HideInInspector] public static int level = 0;
    [HideInInspector] public static bool evolve = false;

    public Transform DrillPool = null;
    public GameObject DrillPrefab = null;
    List<BulletCtrl> drillList = new List<BulletCtrl>();

    const int DrillInitCount = 5;
    const float DrillOffset = 0.35f;

    Vector3 randDir = Vector3.zero;

    void Start()
    {
        InitDrills();
        FireDrill();
    }

    void Update()
    {
        
    }

    void InitDrills()
    {
        GameObject drill = Instantiate(DrillPrefab, DrillPool);
        drill.SetActive(false);
        drillList.Add(drill.GetComponent<BulletCtrl>());
    }

    void FireDrill()
    {
        Vector3 pos = GameMgr.Inst.player.transform.position;

        GameObject rkt = DrillPool.GetChild(0).gameObject;
        if (!rkt.activeSelf)
        {
            rkt.SetActive(true);

            BulletCtrl rocketBlt = rkt.GetComponent<BulletCtrl>();
            randDir.x = Random.Range(-1.0f, 1.0f);
            randDir.y = Random.Range(-1.0f, 1.0f);
            randDir.Normalize();
            rocketBlt.MoveDir = randDir;
            Debug.Log("Drill Dir : " + randDir);
            float angle = Mathf.Atan2(randDir.y, randDir.x) * Mathf.Rad2Deg;
            rocketBlt.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            rocketBlt.transform.position = pos + randDir * DrillOffset;
        }
    }
}