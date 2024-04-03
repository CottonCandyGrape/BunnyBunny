using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllSceneMgr : G_Singleton<AllSceneMgr>
{
    [HideInInspector] public int CurStageNum = 0;
    [HideInInspector] public string PlayerInfoJson = "";

    public UserInfo user;
    string filePath = "Assets/UserData/";
    string[] fileList;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "InGame_1") //바로 InGame_1 에서 시작했을때 UpLowUI 부르지 않기
            return;

        if (SceneManager.GetActiveScene().name != "UpLowUI" &&
         SceneManager.loadedSceneCount == 1) //InGame이 아닌 씬에서 시작했을때 UpLowUI를 먼저 불러오기
        {
            SceneManager.LoadScene("UpLowUI");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Additive);
        }

        fileList = Directory.GetFiles(filePath, "*.json");
        if (fileList.Length == 0) //저장된 유저 정보가 없으면 새로 저장
        {
            UserInit();
        }
        else //있으면 불러오기
        {
            LoadUserInfo();
        }
    }

    void UserInit()
    {
        user = new UserInfo();
        user.NickName = "닉네임";
        user.level = 1;
        user.exp = 0.0f;
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

    //void Update() { }
}