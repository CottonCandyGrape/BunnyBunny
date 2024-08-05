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

    [Header("------ Difficulty Block ------")]
    public Image Cursor_Img = null;
    public Button[] Stars_Btn = null;
    public Sprite FillStar_Img = null;
    public Sprite EmptyStar_Img = null;
    int difficulty = 0;
    Vector2[] cursorPos = {
        new Vector2(-170.0f, 40.0f),
        new Vector2(10.0f, 40.0f),
        new Vector2(190.0f, 40.0f)
    };

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

        for (int i = 0; i < Stars_Btn.Length; i++)
        {
            int idx = i;
            Stars_Btn[idx].onClick.AddListener(() => StarBtnClick(Stars_Btn[idx]));
        }

        if (StageLeft_Btn)
            StageLeft_Btn.onClick.AddListener(StageLeftBtnClick);

        if (StageRight_Btn)
            StageRight_Btn.onClick.AddListener(StageRightBtnClick);

        if (StageNum_Txt) //StageNum 초기화
            StageNum_Txt.text = (PlayerPrefs.GetInt("StageNumCursor", 0) + 1).ToString();

        if (Start_Btn)
            Start_Btn.onClick.AddListener(StartGame);

        unLockStageNum = AllSceneMgr.Instance.user.unLockStageNum;
        stageNum = PlayerPrefs.GetInt("StageNumCursor", 0);

        SoundMgr.Instance.PlayBGM("UIScene");

        InitAtkObjects();
        SetUpLowScene();
        RefreshStars();
        SetStartBtn();

        AllSceneMgr.Instance.adsMgr.OffBannerView();
        PreparedAds();
    }

    void StarBtnClick(Button btn)
    {
        float posX = btn.transform.localPosition.x;
        int idx = 0;
        if (posX < -10.0f)
            idx = 0;
        else if (-1.0f < posX && posX < 1.0f)
            idx = 1;
        else if (10.0f < posX)
            idx = 2;

        difficulty = idx;
        Cursor_Img.transform.localPosition = cursorPos[idx];
    }

    void RefreshStars()
    {
        bool[] curStar;
        if (AllSceneMgr.Instance.user.StageStars[stageNum] == null)
            curStar = new bool[3];
        else
            curStar = AllSceneMgr.Instance.user.StageStars[stageNum].Stars;

        for (int i = 0; i < curStar.Length; i++)
        {
            Image img = Stars_Btn[i].GetComponent<Image>();
            if (curStar[i])
                img.sprite = FillStar_Img;
            else
                img.sprite = EmptyStar_Img;
        }

        Cursor_Img.transform.localPosition = cursorPos[0];
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
        Ad_Img.gameObject.SetActive(!onOff);
        Ad_Txt.gameObject.SetActive(!onOff);
    }

    void PreparedAds()
    {
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
            if (AllSceneMgr.Instance.adsMgr.RewardAd == null ||
                !AllSceneMgr.Instance.adsMgr.RewardAd.CanShowAd())
                AllSceneMgr.Instance.InitMsgPopUp("adLoading");
            else
                AllSceneMgr.Instance.adsMgr.ShowRewardedAd();

            return;
        }

        if (unLockStageNum < stageNum)
        {
            AllSceneMgr.Instance.InitMsgPopUp("notYetStart");
            return;
        }

        if (AllSceneMgr.Instance.user.DiaNum < GameDia)
        {
            AllSceneMgr.Instance.InitMsgPopUp("notEnoughDia");
            return;
        }

        if (AllSceneMgr.Instance.user.IsEquiped[(int)InvenType.Weapon] == string.Empty)
        {
            AllSceneMgr.Instance.InitMsgPopUp("notEquiped");
            return;
        }

        AllSceneMgr.Instance.SubDia(GameDia);
        AllSceneMgr.Instance.AtkTypeNum = atkTypeNum;
        AllSceneMgr.Instance.CurStageNum = stageNum;
        AllSceneMgr.Instance.Difficulty = difficulty;

        PlayerPrefs.SetInt("StageNumCursor", stageNum);

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
        RefreshStars();
    }

    void StageRightBtnClick()
    {
        SoundMgr.Instance.PlaySfxSound("btnClick");

        if (MaxStageNum <= stageNum) return;

        stageNum++;
        StageNum_Txt.text = (stageNum + 1).ToString();

        SetLockImage();
        RefreshStars();
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