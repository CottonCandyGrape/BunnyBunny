using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardiansCtrl : MonoBehaviour
{
    const int MaxLevel = 3;
    [HideInInspector] public static int level = 0; // 굳이 public, static 이어야 하는지?
    [HideInInspector] public static bool evolve = false;

    const int GuardInitCount = 3;
    const int MaxGuardCount = 6;
    public GameObject GuardPrefab = null;
    public Transform Guardians = null;

    void Start()
    {
        InitGuardians(); 
    }

    //void Update() { }

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
        GuardCtrl[] guards = Guardians.GetComponentsInChildren<GuardCtrl>();
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
}