using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MWType //Main Weapon Type
{
    Gun,
    Blade,
}

public class WeaponMgr : MonoBehaviour
{
    [Header("------ Main Weapon ------")]
    //메인 무기
    public GameObject[] MWPrefabs = null;
    public Transform MainWeapon = null; 
    public MWType MainType = MWType.Gun; //TODO : public 이어야 할지 잘 모르겠네

    GunCtrl gunCtrlSc = null;
    public GunCtrl GunCtrlSc
    {
        get
        {
            if (gunCtrlSc != null)
                return gunCtrlSc;
            else
                return null;
        }
    }
    //TODO : bladeCtrl.cs 추가하기
    //메인 무기

    [Header("------ Guardians ------")]
    //수호자 관련
    public Transform Guardians = null;
    public GameObject GuardPrefab = null;
    const int MaxGuardCount = 6;
    const int GuardInitCount = 3;
    //수호자 관련

    /*
    [Header("------ Rockets ------")]
    //로켓관련
    public Transform Rockets = null;
    public GameObject RocketPrefab = null;
    List<RocketCtrl> rocketPool = new List<RocketCtrl>();
    const int RocketInitCount = 5;
    const float RocketOffset = 0.35f;
    //로켓관련
    */

    void Awake()
    {
        SetMainWeapon(MainType);
    }

    void Start() { }

    //void Update() { }

    void SetMainWeapon(MWType mwType) //메인 무기 세팅 및 교체 함수
    {
        for (int i = 0; i < MainWeapon.childCount; i++) //교체를 위해 이전 오브젝트 삭제. 
            Destroy(MainWeapon.GetChild(i)); //교체가 필요할진 모르겠네.

        GameObject obj = Instantiate(MWPrefabs[(int)mwType], MainWeapon);
        if (mwType == MWType.Gun)
        {
            gunCtrlSc = obj.GetComponent<GunCtrl>();
        }
        else if (mwType == MWType.Blade)
        {
            //TODO : BladeCtrl.cs 만들고 추가해야함.
        }
    }

    //수호자 관련
    public void InitGuardians() //3개로 시작
    {
        Guardians.gameObject.SetActive(true);

        for (int i = 0; i < GuardInitCount; i++)
        {
            GameObject guardObj = Instantiate(GuardPrefab, Guardians);
            GuardCtrl guard = guardObj.GetComponent<GuardCtrl>();
            guard.Degree = (360 / GuardInitCount) * i;
        }
    }

    public void LevelUpGuardiands() //수호자 개수 늘리기
    {
        if (Guardians.childCount >= MaxGuardCount) return;

        GameObject guardObj = Instantiate(GuardPrefab, Guardians);
        GuardCtrl[] guards = Guardians.GetComponentsInChildren<GuardCtrl>();
        for (int i = 0; i < guards.Length; i++)
            guards[i].Degree = (360 / guards.Length) * i;
    }
    //수호자 관련

    /*
    //로켓 관련
    public void InitRockets()
    {
        for (int i = 0; i < RocketInitCount; i++)
        {
            GameObject rocket = Instantiate(RocketPrefab, Rockets);
            rocket.SetActive(false);
            rocketPool.Add(rocket.GetComponent<RocketCtrl>());
        }
    }

    public void FireRocket(Vector3 pos, Vector3 dir)
    {
        for (int i = 0; i < Rockets.childCount; i++)
        {
            GameObject rkt = Rockets.GetChild(i).gameObject;
            if (!rkt.activeSelf)
            {
                rkt.SetActive(true);

                RocketCtrl rktCtrl = rkt.GetComponent<RocketCtrl>();
                rktCtrl.MoveDir = dir; //dir normalized 돼서 넘어옴
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                rktCtrl.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                rktCtrl.transform.position = pos + dir * RocketOffset;
                return;
            }
            else continue;
        }
        // 발사주기를 생각했을때 꺼진게 없으면 안되는데(5개 까지도 필요없음)
        // 일단 꺼진게 없으면 발사 안함. TODO : 어케 해야할지 생각하기 
    }
    //로켓 관련
    */

    //public void Drills() { }
}