using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UserInfo
{
    public string NickName = "";
    public float IncRatio = 1.7f;
    public float PrevExp = 0.0f;
    public float CurExp = 0.0f;
    public float NextExp = 50.0f;
    public float Hp = 100.0f;
    public float Attack = 100.0f;
    public float Defense = 100.0f;
    public float Heal = 30.0f;
    public int Level = 1;
    public int DiaNum = 0;
    public int Gold = 0;
    public int ReinCursor = 0;
    public int unLockStageNum = 0;
    public bool Joystick = true;
    public Image Profile_Img = null;
}
