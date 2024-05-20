using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSceneMgr : MonoBehaviour
{
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
    int unLockStageNum = 1; //TODO : UserInfo로 넘겨야 할듯.

    void Start()
    {
        Time.timeScale = 1.0f; //인게임에서 죽으면 0.0f 되기 때문에 다시 맞춰줌

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

        InitAtkObjects();
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
        //string sceneName = "InGame_" + (stageNum + 1).ToString();
        //if (!IsExistScene(sceneName)) 

        if (unLockStageNum < stageNum)
        {
            AllSceneMgr.Instance.InitMsgPopUp("아직 도전할 수 없습니다.");
            return;
        }

        if (AllSceneMgr.Instance.user.diaNum < GameDia)
        {
            AllSceneMgr.Instance.InitMsgPopUp("보유 다이아가 부족합니다.");
            return;
        }

        AllSceneMgr.Instance.SubDia(GameDia);
        AllSceneMgr.Instance.CurStageNum = stageNum;
        AllSceneMgr.Instance.AtkTypeNum = atkTypeNum;
        SceneManager.LoadScene("InGame");
    }

    //bool IsExistScene(string name) //BuildSetting에 존재하는 Scene인지 확인하는 함수
    //{
    //    EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes; //EditorBuildSettings에서 빌드할때 에러남
    //    for (int i = 0; i < scenes.Length; i++)
    //    {
    //        if (scenes[i].path.Contains(name)) return true;
    //    }

    //    return false;
    //}

    void AtkLeftBtnClick()
    {
        atkTypeNum--;
        if (atkTypeNum < 0)
            atkTypeNum = (int)AtkType.Count - 1;

        SetAtkType();
    }

    void AtkRightBtnClick()
    {
        atkTypeNum++;
        if ((int)AtkType.Count <= atkTypeNum)
            atkTypeNum = 0;

        SetAtkType();
    }

    void StageLeftBtnClick()
    {
        if (stageNum <= MinStageNum) return;

        stageNum--;
        StageNum_Txt.text = (stageNum + 1).ToString();

        SetLockImage();
    }

    void StageRightBtnClick()
    {
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