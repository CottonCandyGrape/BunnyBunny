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

    const int MinStageNum = 0;
    const int MaxStageNum = 2;
    const int GameDia = 5;

    int atkTypeNum = 0;
    int stageNum = 0;
    int unLockStageNum = 0;

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
            StageNum_Txt.text = (stageNum + 1).ToString();

        if (Start_Btn)
            Start_Btn.onClick.AddListener(StartGame);

        unLockStageNum = AllSceneMgr.Instance.user.unLockStageNum;

        SoundMgr.Instance.PlayBGM("UIScene");

        InitAtkObjects();
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

        AllSceneMgr.Instance.SubDia(GameDia);
        AllSceneMgr.Instance.CurStageNum = stageNum;
        AllSceneMgr.Instance.AtkTypeNum = atkTypeNum;
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