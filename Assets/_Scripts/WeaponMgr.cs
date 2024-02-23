using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMgr : MonoBehaviour
{
    //수호자 관련
    public Transform Guardians = null;
    public GameObject GuardPrefab = null;
    const int MaxGuardCount = 6;
    //수호자 관련

    public static WeaponMgr Inst = null;

    void Awake()
    {
        Inst = this;
    }

    void Start() { }

    //void Update() { }

    public void SetGuardians() //3개로 시작
    {
        int initCnt = 3;

        Guardians.gameObject.SetActive(true);

        for (int i = 0; i < initCnt; i++)
        {
            GameObject guardObj = Instantiate(GuardPrefab, Guardians);
            GuardCtrl guard = guardObj.GetComponent<GuardCtrl>();
            guard.Degree = (360 / initCnt) * i;
        }
    }

    public void LevelUpGuardiands() 
    {
        if (Guardians.childCount >= MaxGuardCount) return;

        GameObject guardObj = Instantiate(GuardPrefab, Guardians);
        GuardCtrl[] guards = Guardians.GetComponentsInChildren<GuardCtrl>();
        for (int i = 0; i < guards.Length; i++)
            guards[i].Degree = (360 / guards.Length) * i;
    }

    //public void Rockets() { }

    //public void Drills() { }
}