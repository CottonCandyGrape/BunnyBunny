using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AllSceneMgr : G_Singleton<AllSceneMgr>
{
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "UpLowUI" &&
         SceneManager.GetAllScenes().Length == 1)
            SceneManager.LoadScene("UpLowUI", LoadSceneMode.Additive);
    }

    //void Update() { }
}