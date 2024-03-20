using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneMgr : MonoBehaviour
{
    private static SceneMgr instance;
    public static SceneMgr Instance
    {
        get { return instance; }
    }

    public Image Fade_Img = null;
    public Image bunny_Img = null;

    void Start()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }
    //TODO : 수업시간에 제원쌤이 해준코드 보기
}