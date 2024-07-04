using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSceneMgr : MonoBehaviour
{
    public Canvas canvas = null;

    [Header("------ Setting Block ------")]
    public Button Setting_Btn = null;
    public GameObject SettingPopUp = null;

    [Header("------ AttackType Block ------")]
    public Button AtkLeft_Btn = null;
    public Button AtkRight_Btn = null;
    public Transform AtkTypePool = null;
    public RectTransform AtkTypePos = null;
    public GameObject[] AtkObjects = null;
    List<GameObject> AtkList = new List<GameObject>();

    [Header("------ Stage Block ------")]
    public Button StageLeft_Btn = null;
    public Button StageRight_Btn = null;
    public GameObject Lock = null;
    public Text StageNum_Txt = null;

    [Header("------ Start Block ------")]
    public Button Start_Btn = null;
    public Text Start_Txt = null;
    public Image Ad_Img = null;
    public Text Ad_Txt = null;

    const int MinStageNum = 0;
    const int MaxStageNum = 2;
    const int GameDia = 5;

    int atkTypeNum = 0;
    int unLockStageNum = 0;
    int stageNum;

    void Start()
    {
        Time.timeScale = 1.0f;

        if (Setting_Btn)
            Setting_Btn.onClick.AddListener(SettingBtnClick);

        if (AtkLeft_Btn)
            AtkLeft_Btn.onClick.AddListener(AtkLeftBtnClick);

        if (AtkRight_Btn)
            AtkRight_Btn.onClick.AddListener(AtkRightBtnClick);

        if (StageLeft_Btn)
            StageLeft_Btn.onClick.AddListener(StageLeftBtnClick);

        if (StageRight_Btn)
            StageRight_Btn.onClick.AddListener(StageRightBtnClick);

        if (StageNum_Txt) //StageNum 초기화
            StageNum_Txt.text = (AllSceneMgr.Instance.user.StageNumCursor + 1).ToString();

        if (Start_Btn)
            Start_Btn.onClick.AddListener(StartGame);

        unLockStageNum = AllSceneMgr.Instance.user.unLockStageNum;
        stageNum = AllSceneMgr.Instance.user.StageNumCursor;

        SoundMgr.Instance.PlayBGM("UIScene");

        InitAtkObjects();
        SetUpLowScene();
        SetStartBtn();

        AllSceneMgr.Instance.adsMgr.OffBannerView();
        PreparedAds();
    }

    public void SetStartBtn()
    {
        if (AllSceneMgr.Instance.user.DiaNum < 5)
            ToggleStartBtnAd(false);
        else
            ToggleStartBtnAd(true);
    }

    void ToggleStartBtnAd(bool onOff)
    {
        Start_Txt.gameObject.SetActive(onOff);
        //Ad_Img.gameObject.SetActive(!onOff); //TODO : 광고 넣으면 다시 살리기
        Ad_Txt.gameObject.SetActive(!onOff);
    }

    void PreparedAds()
    {
        if (!AllSceneMgr.Instance.adOn) return;

        AllSceneMgr.Instance.adsMgr.LoadInterstitialAd();
        if (AllSceneMgr.Instance.user.DiaNum < 5)
            AllSceneMgr.Instance.adsMgr.LoadRewardedAd();
    }

    void SetUpLowScene()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scn = SceneManager.GetSceneAt(i);
            if (scn.name == "UpLowUI") return;
        }

        StartCoroutine(AllSceneMgr.Instance.LoadUpLowUIScene());
    }

    void SettingBtnClick()
    {
        AllSceneMgr.Instance.InitSettingPopUp();
    }

    void InitAtkObjects()
    {
        for (int i = 0; i < AtkObjects.Length; i++)
        {
            GameObject atk = Instantiate(AtkObjects[i], AtkTypePool);
            AtkList.Add(atk);
            atk.SetActive(false);
        }

        SetAtkType();
    }

    //void Update() { }

    void StartGame()
    {
        SoundMgr.Instance.PlaySfxSound("startClick");

        if (!Start_Txt.gameObject.activeSelf) //Dia 부족시
        {
            /* TODO : 광고 넣으면 다시 살리기
            if (AllSceneMgr.Instance.adsMgr.RewardAd == null ||
                !AllSceneMgr.Instance.adsMgr.RewardAd.CanShowAd())
                AllSceneMgr.Instance.InitMsgPopUp("광고가 로드되고 있습니다. 잠시 후 다시 시도해 주세요.");
            else
                AllSceneMgr.Instance.adsMgr.ShowRewardedAd();
            */

            AllSceneMgr.Instance.user.DiaNum += 5;
            AllSceneMgr.Instance.WriteUserInfo();
            AllSceneMgr.Instance.RefreshTopUI();
            SetStartBtn();

            return;
        }

        if (unLockStageNum < stageNum)
        {
            AllSceneMgr.Instance.InitMsgPopUp("아직 도전할 수 없습니다.");
            return;
        }

        if (AllSceneMgr.Instance.user.DiaNum < GameDia)
        {
            AllSceneMgr.Instance.InitMsgPopUp("보유 다이아가 부족합니다.");
            return;
        }

        if (AllSceneMgr.Instance.user.IsEquiped[(int)InvenType.Weapon] == string.Empty)
        {
            AllSceneMgr.Instance.InitMsgPopUp("메인 무기를 장착하지 않았습니다.\n 장비탭에서 장착해주세요.");
            return;
        }

        AllSceneMgr.Instance.SubDia(GameDia);
        AllSceneMgr.Instance.AtkTypeNum = atkTypeNum;
        AllSceneMgr.Instance.CurStageNum = stageNum;
        AllSceneMgr.Instance.user.StageNumCursor = stageNum;
        AllSceneMgr.Instance.WriteUserInfo();

        PreparedAds();

        StartCoroutine(AllSceneMgr.Instance.LoadScene("InGame"));
    }

    void AtkLeftBtnClick()
    {
        SoundMgr.Instance.PlaySfxSound("btnClick");

        atkTypeNum--;
        if (atkTypeNum < 0)
            atkTypeNum = (int)AtkType.Count - 1;

        SetAtkType();
    }

    void AtkRightBtnClick()
    {
        SoundMgr.Instance.PlaySfxSound("btnClick");

        atkTypeNum++;
        if ((int)AtkType.Count <= atkTypeNum)
            atkTypeNum = 0;

        SetAtkType();
    }

    void StageLeftBtnClick()
    {
        SoundMgr.Instance.PlaySfxSound("btnClick");

        if (stageNum <= MinStageNum) return;

        stageNum--;
        StageNum_Txt.text = (stageNum + 1).ToString();

        SetLockImage();
    }

    void StageRightBtnClick()
    {
        SoundMgr.Instance.PlaySfxSound("btnClick");

        if (MaxStageNum <= stageNum) return;

        stageNum++;
        StageNum_Txt.text = (stageNum + 1).ToString();

        SetLockImage();
    }

    void SetLockImage()
    {
        if (stageNum <= unLockStageNum)
            Lock.gameObject.SetActive(false);
        else
            Lock.gameObject.SetActive(true);
    }

    void SetAtkType()
    {
        for (int i = 0; i < AtkList.Count; i++)
        {
            if (AtkList[i].activeSelf)
                AtkList[i].SetActive(false);
        }

        AtkList[atkTypeNum].SetActive(true);
    }
}