using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillCtrl : Weapon
{
    public Transform DrillPool = null;
    public GameObject DrillPrefab = null;
    public GameObject ArrowHeadPrefab = null;
    GameObject arrowHead = null;

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

        ev_Name = "화살촉";
    }

    void SetBulletTransform(BulletCtrl blt)
    {
        Vector3 randDir = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
        randDir.Normalize();
        blt.MoveDir = randDir;

        float angle = Mathf.Atan2(randDir.y, randDir.x) * Mathf.Rad2Deg;
        blt.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        blt.transform.position = GameMgr.Inst.player.transform.position + randDir * DrillOffset;
    }

    IEnumerator LoadDrills()
    {
        for (int i = 0; i < DrillPool.childCount; i++)
        {
            GameObject rkt = DrillPool.GetChild(i).gameObject;
            if (!rkt.activeSelf)
            {
                rkt.SetActive(true);

                BulletCtrl rocketBlt = rkt.GetComponent<BulletCtrl>();
                SetBulletTransform(rocketBlt);
            }

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

    public void FireArrowHead()
    {
        if (arrowHead == null)
        {
            arrowHead = Instantiate(ArrowHeadPrefab, DrillPool);
            arrowHead.SetActive(false);
        }

        //Vector3 pos = GameMgr.Inst.player.transform.position;

        if (!arrowHead.activeSelf)
        {
            arrowHead.SetActive(true);

            BulletCtrl rocketBlt = arrowHead.GetComponent<BulletCtrl>();
            SetBulletTransform(rocketBlt);
        }
    }

    public override void LevelUpWeapon()
    {
        GameObject drill = Instantiate(DrillPrefab, DrillPool);
        drill.SetActive(false);

        curLevel++;
    }

    public override void EvolveWeapon()
    {
        isEvolve = true;
    }

    public override string GetExplainText()
    {
        if (curLevel == 0)
            return "드릴 2개.";
        else if (curLevel == 1 || curLevel == 2)
            return "드릴 추가 1개.";
        else if (curLevel == 3)
            return "대화살촉. 속도 증가.";
        return string.Empty;
    }
}