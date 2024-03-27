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
    public Image LockStage_Img = null;
    public Image Lock = null;
    public Text StageNum_Txt = null;
    public Button Start_Btn = null;

    Color lockColor = new Color32(120, 114, 114, 255);

    const int MinStageNum = 0;
    const int MaxStageNum = 2;
    int stageNum = 0;
    int unLockStageNum = 0;

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

        SetLockImage();
    }

    void RightArrowBtn()
    {
        if (MaxStageNum <= stageNum) return;

        stageNum++;
        StageNum_Txt.text = (stageNum + 1).ToString();

        SetLockImage();
    }

    void SetLockImage()
    {
        if (stageNum <= unLockStageNum)
        { 
            Lock.gameObject.SetActive(false);
            LockStage_Img.color = Color.white;
            StageNum_Txt.color = Color.white;
        }
        else
        {
            Lock.gameObject.SetActive(true);
            LockStage_Img.color = lockColor;
            StageNum_Txt.color = lockColor;
        }
    }
}