using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public PlayerCtrl player = null;

    //게임 시간 관련 변수
    public Text Time_Txt = null;
    float curTime = 0.0f;
    float minTime = 60.0f;
    int min = 0;
    int sec = 0;
    float endTime = 90.0f; //Test 용. //TODO : 적정 시간 찾기
    //게임 시간 관련 변수

    //몬스터 킬수 표시
    public Text Kill_Txt = null;
    int killCount = 0;
    //몬스터 킬수 표시

    public static GameMgr Inst = null; 

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        curTime = 0.0f;

        Time.timeScale = 1.0f;
    }

    void Update()
    {
        UpdateGameTime();
    }

    void UpdateGameTime()
    {
        curTime += Time.deltaTime;

        min = (int)(curTime / minTime);
        sec = (int)curTime;

        Time_Txt.text = string.Format("{0:D2}:{1:D2}", min, sec);

        if(endTime <= curTime)
        {
            GameOver();
            return;
        }
    }

    public void KillTxtUpdate()
    {
        killCount++;
        if (Kill_Txt != null)
            Kill_Txt.text = killCount.ToString(); 
    }

    void GameOver()
    {
        Time.timeScale = 0.0f;
    }
}