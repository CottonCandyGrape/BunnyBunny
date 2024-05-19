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
    [HideInInspector] public MWType MainType = MWType.Gun;
    [HideInInspector] public Weapon MainWp = null;
    public GameObject[] MWPrefabs = null; //아이템이 많아지면 다른 저장소에서 변수 하나로 받아와야겠다.
    public GameObject[] MWSkillUpBtns = null; //아이템이 많아지면 다른 저장소에서 변수 하나로 받아와야겠다.
    public Transform MainWeapon = null;

    //총
    GunCtrl gunCtrlSc = null;
    public GunCtrl GunCtrlSc { get { return gunCtrlSc; } }
    //총

    //TODO : bladeCtrl.cs 추가하기
    //메인 무기

    //수호자 관련
    [Header("------ Guardians ------")]
    public GameObject Guardians = null;
    GuardiansCtrl guardiansCtrlSc = null;
    public GuardiansCtrl GuardiansCtrlSc { get { return guardiansCtrlSc; } }
    //수호자 관련

    //로켓 관련
    [Header("------ Rockets ------")]
    public GameObject Rockets = null;
    RocketCtrl rocketCtrlSc = null;
    public RocketCtrl RocketCtrlSc { get { return rocketCtrlSc; } }
    //로켓 관련

    //드릴 관련
    [Header("------ Drills ------")]
    public GameObject Drills = null;
    DrillCtrl drillCtrlSc = null;
    public DrillCtrl DrillCtrlSc { get { return drillCtrlSc; } }
    //드릴 관련

    public static WeaponMgr Inst = null;

    void Awake()
    {
        Inst = this;

        SetMainWeapon(MainType);
        SetWeaponScripts();
    }

    void Start() { }

    //메인 무기 관련
    void SetMainWeapon(MWType mwType) //메인 무기 세팅 및 교체 함수
    {
        GameObject obj = Instantiate(MWPrefabs[(int)mwType], MainWeapon);
        if (mwType == MWType.Gun)
        {
            gunCtrlSc = obj.GetComponent<GunCtrl>();
            MainWp = GunCtrlSc;
        }
        else if (mwType == MWType.Blade)
        {
            //TODO : BladeCtrl.cs 만들고 추가해야함.
        }
    }
    //메인 무기 관련

    void SetWeaponScripts()
    {
        guardiansCtrlSc = Guardians.GetComponent<GuardiansCtrl>();
        rocketCtrlSc = Rockets.GetComponent<RocketCtrl>();
        drillCtrlSc = Drills.GetComponent<DrillCtrl>();
    }
}