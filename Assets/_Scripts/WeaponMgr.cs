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

    float mAtkTimer = 0.0f;
    float mAtkTime = 0.2f;
    int fireCnt = 5;
    int curFire = 0;
    //메인 무기

    [Header("------ Guardians ------")]
    //수호자 관련
    public Transform Guardians = null;
    public GameObject GuardPrefab = null;
    const int MaxGuardCount = 6;
    const int GuardInitCount = 3;
    //수호자 관련

    [Header("------ Rockets ------")]
    //로켓관련
    public Transform Rockets = null;
    public GameObject RocketPrefab = null;
    List<RocketCtrl> rocketPool = new List<RocketCtrl>();
    const int RocketInitCount = 5;
    float rktTime = 2.0f;
    float rktTimer = 0.0f;
    //로켓관련

    public static WeaponMgr Inst = null;

    void Awake()
    {
        Inst = this;

        SetMainWeapon(MainType);
    }

    void Start() { }

    void Update()
    {
        CalcWeaponTimer();
    }

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

    public void LevelUpGuardiands() 
    {
        if (Guardians.childCount >= MaxGuardCount) return;

        GameObject guardObj = Instantiate(GuardPrefab, Guardians);
        GuardCtrl[] guards = Guardians.GetComponentsInChildren<GuardCtrl>();
        for (int i = 0; i < guards.Length; i++)
            guards[i].Degree = (360 / guards.Length) * i;
    }
    //수호자 관련

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

    //로켓 관련

    void CalcWeaponTimer()
    {
        mAtkTimer -= Time.deltaTime;
        if (mAtkTimer <= 0.0f)
        {
            mAtkTimer = mAtkTime;
            if (fireCnt < curFire)
            {
                GunCtrlSc.FanFire(GameMgr.Inst.player.arrowDir);
                curFire = 0;
            }
            else
            {
                GunCtrlSc.FireBullet(GameMgr.Inst.player.arrowDir);
                curFire++;
            }
        }
    }

    //public void Drills() { }
}