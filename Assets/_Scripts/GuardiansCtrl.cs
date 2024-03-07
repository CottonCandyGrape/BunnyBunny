using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardiansCtrl : Weapon
{
    const int GuardInitCount = 3;
    const int MaxGuardCount = 6;

    public GameObject GuardPrefab = null;
    public Transform Guardians = null;

    float lifeTimer = 0.0f;
    float lifeTime = 3.0f;
    float waitTimer = 0.0f;
    float waitTime = 1.0f;

    bool IsOn = true;

    void Start()
    {
        InitGuardians();
    }

    void Update()
    {
        OnOffGuardians();
    }

    void InitGuardians() //3개로 시작
    {
        for (int i = 0; i < GuardInitCount; i++)
        {
            GameObject guardObj = Instantiate(GuardPrefab, Guardians);
            GuardCtrl guard = guardObj.GetComponent<GuardCtrl>();
            guard.Degree = (360 / GuardInitCount) * i;
        }
    }

    public void LevelUpGuardiands() //수호자 개수 늘리기
    {
        if (MaxGuardCount <= Guardians.childCount) return;

        GameObject guardObj = Instantiate(GuardPrefab, Guardians);
        if (!IsOn) guardObj.SetActive(false);

        GuardCtrl[] guards = Guardians.GetComponentsInChildren<GuardCtrl>(true);
        for (int i = 0; i < guards.Length; i++)
            guards[i].Degree = (360 / guards.Length) * i;

        //if (MaxLevel <= level)
        //{
        //    Debug.Log("MaxLevel 입니다.");
        //    //evolve = true;
        //    return;
        //}
        //else
        //    level++;
    }

    //TODO : 개수 늘릴때 onoff 상태가 있어야 하고 추가 될때 그 상태에 맞게 추가되어야 한다.
    //켜질때 개수에 맞게 다시 위치 리셋 해줘야한다. 
    void OnOffGuardians()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0.0f)
        {
            if (IsOn)
            {
                for (int i = 0; i < Guardians.childCount; i++)
                    Guardians.GetChild(i).gameObject.SetActive(false);
                IsOn = false;
            }

            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0.0f)
            {
                for (int i = 0; i < Guardians.childCount; i++)
                    Guardians.GetChild(i).gameObject.SetActive(true);

                lifeTimer = lifeTime;
                waitTimer = waitTime;
                IsOn = true;
            }
        }
    }
}