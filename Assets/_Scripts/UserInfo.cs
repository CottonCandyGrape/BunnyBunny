using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UserInfo
{
    public string NickName = "";
    public float exp = 0.0f;
    public float attack = 100.0f;
    public float defense = 100.0f;
    public int level = 1;
    public int diaNum = 0;
    public int gold = 0;
    public Image profile_Img = null;
}
