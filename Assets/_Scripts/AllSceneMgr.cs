using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllSceneMgr : G_Singleton<AllSceneMgr>
{
    [HideInInspector] public int CurStageNum = 0;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "InGame_1") //바로 InGame_1 에서 시작했을때 예외 처리
            return;

        if (SceneManager.GetActiveScene().name != "UpLowUI" &&
         SceneManager.loadedSceneCount == 1)
            SceneManager.LoadScene("UpLowUI", LoadSceneMode.Additive);
    }

    //void Update() { }
}