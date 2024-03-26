using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSceneMgr : MonoBehaviour
{
    [Header("------ Top UI ------")]
    public Text Nickname_Txt = null;
    public Text Heart_Txt = null;
    public Text Gold_Txt = null;
    public Text Level_Txt = null;
    public Button Setting_Btn = null;

    [Header("------ Center UI ------")]
    public Button Left_Btn = null;
    public Button Right_Btn = null;
    public Text StageNum_Txt = null;
    public Button Start_Btn = null;

    [Header("------ Bottom UI ------")]
    public Button Store_Btn = null;
    public Button Inventory_Btn = null;
    public Button Battle_Btn = null;
    public Button Evolve_Btn = null;

    int stageNum = 1;
    const int MinStageNum = 1;
    const int MaxStageNum = 3;

    void Start()
    {
        // Top UI 

        // Center UI
        if (Left_Btn)
            Left_Btn.onClick.AddListener(LeftArrowBtn);

        if (Right_Btn)
            Right_Btn.onClick.AddListener(RightArrowBtn);

        if (StageNum_Txt) //StageNum 초기화
            StageNum_Txt.text = stageNum.ToString();

        if (Start_Btn)
            Start_Btn.onClick.AddListener(StartGame);

        // Bottom UI
    }

    //void Update() { }

    //TODO : stageNum를 GameMgr에 넘겨줘서 알맞은 스테이지가 실행되어야 한다.
    //Scene 이름에 Stage번호를 넣어야겠네...
    void StartGame()
    {
        SceneManager.LoadScene("InGame");
    }

    void LeftArrowBtn()
    {
        if (stageNum <= MinStageNum) return;

        stageNum--;
        StageNum_Txt.text = stageNum.ToString();
    }

    void RightArrowBtn()
    {
        if (MaxStageNum <= stageNum) return;

        stageNum++;
        StageNum_Txt.text = stageNum.ToString();
    }
}