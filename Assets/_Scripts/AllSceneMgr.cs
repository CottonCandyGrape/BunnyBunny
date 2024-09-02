using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AllSceneMgr : G_Singleton<AllSceneMgr>
{
    public Text Debug_Txt = null;

    [HideInInspector] public int CurStageNum = 0;
    [HideInInspector] public int AtkTypeNum = 0;
    [HideInInspector] public int Difficulty = 0;

    //유저 정보 관련
    [HideInInspector] public string PlayerInfoJson = "";
    [HideInInspector] public UserInfo user = new UserInfo();
    [HideInInspector] public LanguageMgr langMgr = null;
    string filePath;
    string fileName;
    //유저 정보 관련

    //팝업창 관련
    Canvas mCanvas = null;
    public GameObject[] PopUpPrefabs = null;
    //팝업창 관련

    //로딩 애니메이션 관련
    public GameObject LoadingAnim_Canvas = null;
    public GameObject Bunny = null;
    const float PosX = 480.0f;
    const float moveSpeed = 960.0f; //로딩 1초 걸리게 하기 위해서
    Vector2 bunnyPos = Vector2.zero;
    Vector2 bunnyOriginPos = Vector2.zero;
    //로딩 애니메이션 관련

    //마우스 포인터
    public GameObject MousePtr = null;
    public bool MousePtrOn = false;
    //마우스 포인터

    //광고 관련
    public AdsMgr adsMgr = null;
    //광고 관련

    UpLowUIMgr ulMgr = null;
    BattleSceneMgr battleMgr = null;

    protected override void Init()
    {
        base.Init();
        filePath = Application.persistentDataPath + "/"; //에디터나 android에서나 상관없게 하려고.

        //유저 정보 관리
        fileName = "PlayerInfo" + ".json";
        if (!File.Exists(filePath + fileName)) //저장된 유저 정보가 없으면 새로 저장
            UserInit();
        else //있으면 불러오기
            LoadUserInfo();
        //유저 정보 관리

        bunnyOriginPos = Bunny.transform.localPosition;

        langMgr = GetComponentInChildren<LanguageMgr>();
    }

    void Start()
    {
        StartCoroutine(LoadScene("Battle"));
    }

    void Update() { MousePointer(); }

    void UserInit()
    {
        user.DiaNum = 30;
        user.Gold = 30000;

        string jsonStr = JsonUtility.ToJson(user);
        File.WriteAllText(filePath + fileName, jsonStr);
    }

    void LoadUserInfo()
    {
        string fromJson = File.ReadAllText(filePath + fileName);
        user = JsonUtility.FromJson<UserInfo>(fromJson);
    }

    public void WriteUserInfo()
    {
        string jsonStr = JsonUtility.ToJson(user);
        File.WriteAllText(filePath + fileName, jsonStr);
    }

    PopUpBox GetPopUpbox(PopUpType pType)
    {
        if (mCanvas == null)
            mCanvas = FindObjectsOfType<Canvas>()[0]; //맨위에있는 Mask Canvas임

        GameObject pop = Instantiate(PopUpPrefabs[(int)pType], mCanvas.transform);
        PopUpBox box = pop.GetComponent<PopUpBox>();
        if (box != null)
            return box;
        else
            return null;
    }

    public void InitMsgPopUp(string txt)
    {
        PopUpBox box = GetPopUpbox(PopUpType.Msg);
        if (box != null) box.SetMsgText(txt);
        //if (box != null)
        //{
        //    box.SetMsgText(txt);
        //    if (txt == "보유 다이아가 부족합니다.")//나중에 좀 더 general하게 짜자
        //        box.SetRewardBtnPos();
        //}
    }

    public void InitReinPopUp(ReinCellButton reinCell)
    {
        PopUpBox box = GetPopUpbox(PopUpType.Reinforce);
        if (box != null) box.SetReinInfo(reinCell);
    }

    public void InitInvenPopUp(InvenButton invBtn)
    {
        PopUpBox box = GetPopUpbox(PopUpType.Inventory);
        if (box != null) box.SetInvenComp(invBtn);
    }

    public void InitSettingPopUp()
    {
        PopUpBox box = GetPopUpbox(PopUpType.Setting);
        if (box != null) box.SetSettingInfo();
    }

    public void RefreshTopUI()
    {
        if (ulMgr == null)
            ulMgr = FindObjectOfType<UpLowUIMgr>();

        ulMgr.RefreshTopUI();
    }

    public void GetDia(int stockNum, int stockPrice)
    {
        user.DiaNum += stockNum;
        user.Gold -= stockPrice;
        WriteUserInfo();
        RefreshTopUI();
        InitMsgPopUp("purchaseSuccess");
    }

    public void ReinSuccess(ReinType rType, int rGold, int rVal, ReinCellButton reinCell)
    {
        user.Gold -= rGold;
        switch (rType)
        {
            case ReinType.Attack:
                user.Attack += rVal;
                break;
            case ReinType.Defense:
                user.Defense += rVal;
                break;
            case ReinType.Heal:
                user.Heal += rVal;
                user.Heal = user.Heal > 100 ? 100 : user.Heal;
                break;
            case ReinType.Hp:
                user.Hp += rVal;
                break;
        }

        user.ReinCursor++;
        WriteUserInfo();
        RefreshTopUI();
        InitMsgPopUp("reinSuccess");

        reinCell.SetAlpha();
    }

    public void SubDia(int dNum)
    {
        user.DiaNum -= dNum;
        WriteUserInfo();
        RefreshTopUI();
    }

    public void GetInGameResult(int gold, float exp, bool clear)
    {
        user.Gold += gold;
        user.CurExp += exp;
        while (user.NextExp <= user.CurExp && user.Level < UserInfo.MaxLevel)
        {
            user.Level++;
            user.PrevExp = user.NextExp;
            user.NextExp *= user.IncRatio;
            user.NextExp = (int)user.NextExp;
        }

        if (clear)
        {
            if (user.unLockStageNum == CurStageNum)
                user.unLockStageNum++;

            if (battleMgr == null)
                battleMgr = FindObjectOfType<BattleSceneMgr>();

            if (!user.StageStars[CurStageNum].Stars[Difficulty])
                user.StageStars[CurStageNum].Stars[Difficulty] = true;

            //3탄까지만 가능 
            user.unLockStageNum = user.unLockStageNum > 2 ? 2 : user.unLockStageNum;
        }

        WriteUserInfo();
    }

    public IEnumerator LoadScene(string sceneName)
    {
        SoundMgr.Instance.TurnOffBgm();
        LoadingAnim_Canvas.SetActive(true);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        Bunny.transform.localPosition = bunnyOriginPos;
        while (!async.isDone)
        {
            bunnyPos = Bunny.transform.localPosition;
            bunnyPos.x += moveSpeed * Time.unscaledDeltaTime;
            bunnyPos.x = PosX <= bunnyPos.x ? PosX : bunnyPos.x;
            Bunny.transform.localPosition = bunnyPos;

            if (PosX <= Bunny.transform.localPosition.x && 0.9f <= async.progress)
                async.allowSceneActivation = true;

            yield return null;
        }
    }

    public IEnumerator LoadUpLowUIScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("UpLowUI", LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }

        LoadingAnim_Canvas.SetActive(false);
    }

    public void SetDebugTxt(string txt)
    {
        Debug_Txt.text += txt + '\n';
    }

    void MousePointer()
    {
        if (!MousePtrOn || MousePtr == null) return;

        Vector2 mousePos = Input.mousePosition;

        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            MousePtr.SetActive(true);
            MousePtr.transform.position = mousePos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            MousePtr.SetActive(false);
        }
    }
}