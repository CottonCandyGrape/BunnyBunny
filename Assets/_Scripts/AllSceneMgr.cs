using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllSceneMgr : G_Singleton<AllSceneMgr>
{
    [HideInInspector] public int CurStageNum = 0;

    //유저 정보 관련
    [HideInInspector] public string PlayerInfoJson = "";
    [HideInInspector] public UserInfo user = new UserInfo();
    string filePath = "Assets/UserData/";
    string[] fileList;
    //유저 정보 관련

    //팝업창 관련
    Canvas mCanvas = null;
    public GameObject[] PopUpPrefabs = null;
    //팝업창 관련

    UpLowUIMgr ulMgr = null;

    void Start()
    {
        //Scene 순서 관리
        if (SceneManager.GetActiveScene().name == "InGame_1") //바로 InGame_1 에서 시작했을때 UpLowUI 부르지 않기
            return;

        if (SceneManager.GetActiveScene().name != "UpLowUI" &&
         SceneManager.loadedSceneCount == 1) //InGame이 아닌 씬에서 시작했을때 UpLowUI를 먼저 불러오기
        {
            SceneManager.LoadScene("UpLowUI");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Additive);
        }
        //Scene 순서 관리

        //유저 정보 관리
        fileList = Directory.GetFiles(filePath, "*.json");
        if (fileList.Length == 0) //저장된 유저 정보가 없으면 새로 저장
        {
            UserInit();
        }
        else //있으면 불러오기
        {
            LoadUserInfo();
        }
        //유저 정보 관리
    }

    //void Update() { }

    void UserInit()
    {
        user.NickName = "닉네임";
        user.level = 1;
        user.exp = 0.0f;
        user.hp = 100.0f;
        user.attack = 100.0f;
        user.defense = 100.0f;
        user.diaNum = 10;
        user.gold = 10000;

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
            mCanvas = FindObjectsOfType<Canvas>()[1]; //2번째 Scene의 Canvas에 올릴 것이기 때문에 [1]

        GameObject pop = Instantiate(PopUpPrefabs[(int)pType], mCanvas.transform);
        PopUpBox box = pop.GetComponent<PopUpBox>();
        if (box != null)
            return box;
        else
            return null;
    }

    public void InitStorePopUp(string txt)
    {
        PopUpBox box = GetPopUpbox(PopUpType.Store); 
        if (box != null) box.SetMsgText(txt);
    }

    public void InitReinPopUp(ReinType rType)
    {
        PopUpBox box = GetPopUpbox(PopUpType.Reinforce);
        if (box != null) box.SetReinInfo(rType);
    }

    public void RefreshTopUI()
    {
        if (ulMgr == null)
            ulMgr = FindObjectOfType<UpLowUIMgr>();

        ulMgr.RefreshTopUI();
    }
}