using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSceneMgr : MonoBehaviour
{
    public Button Left_Btn = null;
    public Button Right_Btn = null;
    public Text StageNum_Txt = null;
    public Button Start_Btn = null;

    int stageNum = 0;
    const int MinStageNum = 0;
    const int MaxStageNum = 2;

    void Start()
    {
        Time.timeScale = 1.0f; //인게임에서 죽으면 0.0f 되기 때문에 다시 맞춰줌

        if (Left_Btn)
            Left_Btn.onClick.AddListener(LeftArrowBtn);

        if (Right_Btn)
            Right_Btn.onClick.AddListener(RightArrowBtn);

        if (StageNum_Txt) //StageNum 초기화
            StageNum_Txt.text = (stageNum + 1).ToString();

        if (Start_Btn)
            Start_Btn.onClick.AddListener(StartGame);
    }

    //void Update() { }

    void StartGame()
    {
        string sceneName = "InGame_" + (stageNum + 1).ToString();
        if (!IsExistScene(sceneName)) return;

        AllSceneMgr.Instance.CurStageNum = stageNum;
        SceneManager.LoadScene(sceneName);
    }

    bool IsExistScene(string name) //BuildSetting에 존재하는 Scene인지 확인하는 함수
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        for (int i = 0; i < scenes.Length; i++)
        {
            if (scenes[i].path.Contains(name)) return true;
        }

        return false;
    }

    void LeftArrowBtn()
    {
        if (stageNum <= MinStageNum) return;

        stageNum--;
        StageNum_Txt.text = (stageNum + 1).ToString();
    }

    void RightArrowBtn()
    {
        if (MaxStageNum <= stageNum) return;

        stageNum++;
        StageNum_Txt.text = (stageNum + 1).ToString();
    }
}