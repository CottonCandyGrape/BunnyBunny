using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillCtrl : Weapon
{
    public Transform DrillPool = null;
    public GameObject DrillPrefab = null;

    Vector3 randDir = Vector3.zero;
    WaitForSeconds fireTerm = new WaitForSeconds(0.2f);
    Coroutine DrillCo = null;

    const int DrillInitCount = 2;
    const float DrillOffset = 0.35f;

    void Start()
    {
        InitDrills();
    }

    //void Update() { }

    void InitDrills() //2개부터 시작
    {
        for (int i = 0; i < DrillInitCount; i++)
        {
            GameObject drill = Instantiate(DrillPrefab, DrillPool);
            drill.SetActive(false);
        }
    }

    IEnumerator LoadDrills()
    {
        Vector3 pos = GameMgr.Inst.player.transform.position;

        for (int i = 0; i < DrillPool.childCount; i++)
        {
            GameObject rkt = DrillPool.GetChild(i).gameObject;
            if (!rkt.activeSelf)
            {
                rkt.SetActive(true);

                BulletCtrl rocketBlt = rkt.GetComponent<BulletCtrl>();
                randDir.x = Random.Range(-1.0f, 1.0f);
                randDir.y = Random.Range(-1.0f, 1.0f);
                randDir.Normalize();
                rocketBlt.MoveDir = randDir;

                float angle = Mathf.Atan2(randDir.y, randDir.x) * Mathf.Rad2Deg;
                rocketBlt.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                rocketBlt.transform.position = pos + randDir * DrillOffset;
            }
            //else continue; //TODO : 예외처리

            yield return fireTerm;
        }
    }

    public void FireDrills()
    {
        for (int i = 0; i < DrillPool.childCount; i++)
        {
            if (DrillPool.GetChild(i).gameObject.activeSelf)
                return;
        }

        if (DrillCo != null)
            StopCoroutine(DrillCo);
        DrillCo = StartCoroutine(LoadDrills());
    }

    public override void LevelUpWeapon()
    {
        if (MaxLevel <= curLevel)
        {
            EvolveWeapon();
            return;
        }

        GameObject drill = Instantiate(DrillPrefab, DrillPool);
        drill.SetActive(false);

        curLevel++;
    }

    public override void EvolveWeapon()
    {
        isEvolve = true;
    }
}