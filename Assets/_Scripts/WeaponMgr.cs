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
    //메인 무기
    [Header("------ Main Weapon ------")]
    public GameObject[] MWPrefabs = null;
    public Transform MainWeapon = null;
    public MWType MainType = MWType.Gun; //TODO : public 이어야 할지 잘 모르겠네

    GunCtrl gunCtrlSc = null;
    public GunCtrl GunCtrlSc
    {
        get { return gunCtrlSc; }
    }
    //TODO : bladeCtrl.cs 추가하기
    //메인 무기

    //수호자 관련
    [Header("------ Guardians ------")]
    public GameObject Guardians = null;
    GuardiansCtrl guardiansCtrlSc = null;
    public GuardiansCtrl GuardiansCtrlSc
    {
        get { return guardiansCtrlSc; }
    }
    //수호자 관련

    //로켓 관련
    [Header("------ Rockets ------")]
    public GameObject Rockets = null;
    RocketCtrl rocketCtrlSc = null;
    public RocketCtrl RocketCtrlSc
    {
        get { return rocketCtrlSc; }
    }
    //로켓 관련

    //드릴 관련
    [Header("------ Drills ------")]
    public GameObject Drills = null;
    DrillCtrl drillCtrlSc = null;
    public DrillCtrl DrillCtrlSc
    {
        get { return drillCtrlSc; }
    }
    //드릴 관련

    public static WeaponMgr Inst = null;

    void Awake()
    {
        Inst = this;

        SetMainWeapon(MainType);
    }

    void Start() { }

    //메인 무기 관련
    void SetMainWeapon(MWType mwType) //메인 무기 세팅 및 교체 함수
    {
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
    //메인 무기 관련

    //수호자 관련
    public void SetGuardians()
    {
        if (!Guardians.activeSelf)
        {
            Guardians.SetActive(true);
            guardiansCtrlSc = Guardians.GetComponent<GuardiansCtrl>();
        }
    }
    //수호자 관련

    //로켓 관련
    public void SetRockets()
    {
        if (!Rockets.activeSelf)
        {
            Rockets.SetActive(true);
            rocketCtrlSc = Rockets.GetComponent<RocketCtrl>();
        }
    }
    //로켓 관련

    //드릴 관련
    public void SetDrills()
    {
        if (!Drills.activeSelf)
        {
            Drills.SetActive(true);
            drillCtrlSc = Drills.GetComponent<DrillCtrl>();
        }
    }
    //드릴 관련
}