using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllSceneMgr : G_Singleton<AllSceneMgr>
{
    [HideInInspector] public int CurStageNum = 0;

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
    }

    //void Update() { }
}