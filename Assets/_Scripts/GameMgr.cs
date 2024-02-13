using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public static GameMgr inst = null; 
    public PlayerCtrl player = null;

    //게임 시간 관련 변수
    public Text Time_Txt = null;
    float curTime = 0.0f;
    int min = 0;
    int sec = 0;
    float endTime = 90.0f; //Test 용. //TODO : 적정 시간 찾기
    //게임 시간 관련 변수

    void Awake()
    {
        inst = this;
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

        min = (int)(curTime / 60.0f);
        sec = (int)curTime;

        Time_Txt.text = string.Format("{0:D2}:{1:D2}", min, sec);

        if(endTime <= curTime)
        {
            GameOver();
            return;
        }
    }

    void GameOver()
    {
        Time.timeScale = 0.0f;
    }
}