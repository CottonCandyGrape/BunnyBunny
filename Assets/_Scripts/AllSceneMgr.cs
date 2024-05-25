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

    //유저 정보 관련
    [HideInInspector] public string PlayerInfoJson = "";
    [HideInInspector] public UserInfo user = new UserInfo();
    string filePath;
    string[] fileList;
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

    UpLowUIMgr ulMgr = null;

    protected override void Init()
    {
        base.Init();
        filePath = Application.persistentDataPath + "/"; //에디터나 android에서나 상관없게 하려고.

        //유저 정보 관리
        fileList = Directory.GetFiles(filePath, "*.json");
        if (fileList.Length == 0) //저장된 유저 정보가 없으면 새로 저장
            UserInit();
        else //있으면 불러오기
            LoadUserInfo();
        //유저 정보 관리

        bunnyOriginPos = Bunny.transform.localPosition;
    }

    void Start()
    {
        StartCoroutine(LoadScene("Battle"));
    }

    //void Update() { }

    void UserInit()
    {
        user.NickName = "닉네임";
        user.Level = 1;
        user.CurExp = 0.0f;
        user.Hp = 100.0f; //TODO : 인게임 시작시 player에 넣어줘야한다.
        user.Attack = 100.0f;
        user.Defense = 100.0f;
        user.DiaNum = 990;
        user.Gold = 10000;

        string jsonStr = JsonUtility.ToJson(user);
        File.WriteAllText(filePath + "PlayerInfo" + ".json", jsonStr);
    }

    void LoadUserInfo()
    {
        string fileName = fileList[0]; //일단 첫번째 user 정보만 활용할 것임
        string fromJson = File.ReadAllText(fileName);
        user = JsonUtility.FromJson<UserInfo>(fromJson);
    }

    public void WriteUserInfo()
    {
        string jsonStr = JsonUtility.ToJson(user);
        File.WriteAllText(filePath + "PlayerInfo" + ".json", jsonStr);
    }

    PopUpBox GetPopUpbox(PopUpType pType)
    {
        if (mCanvas == null)
            mCanvas = FindObjectsOfType<Canvas>()[0]; //1번째 Scene의 Canvas에 올릴 것이기 때문에 [0]

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
        InitMsgPopUp("구매 성공.");
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
                break;
            case ReinType.Hp:
                user.Hp += rVal;
                break;
        }

        user.ReinCursor++;
        WriteUserInfo();
        RefreshTopUI();
        InitMsgPopUp("강화 성공.");

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
        while (user.NextExp <= user.CurExp)
        {
            user.Level++;
            user.PrevExp = user.NextExp;
            user.NextExp *= user.IncRatio;
            user.NextExp = (int)user.NextExp;
        }

        if (clear)
            user.unLockStageNum = CurStageNum + 1;

        WriteUserInfo();
    }

    public IEnumerator LoadScene(string sceneName)
    {
        SetDebugTxt("LoadScene: " + sceneName);
        SetDebugTxt("TimeScale: " + Time.timeScale);
        LoadingAnim_Canvas.SetActive(true);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        Bunny.transform.localPosition = bunnyOriginPos;
        while (!async.isDone)
        {
            bunnyPos = Bunny.transform.localPosition;
            bunnyPos.x += moveSpeed * Time.deltaTime;
            bunnyPos.x = PosX <= bunnyPos.x ? PosX : bunnyPos.x;
            Bunny.transform.localPosition = bunnyPos;

            if (PosX <= Bunny.transform.localPosition.x && 0.9f <= async.progress)
                async.allowSceneActivation = true;

            yield return null;
        }
    }

    public IEnumerator LoadUpLowUIScene()
    {
        SetDebugTxt("UpLowUI");
        AsyncOperation async = SceneManager.LoadSceneAsync("UpLowUI", LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return null;
        }

        LoadingAnim_Canvas.SetActive(false);
    }

    void SetDebugTxt(string txt)
    {
        Debug_Txt.text += txt + '\n';
    }
}