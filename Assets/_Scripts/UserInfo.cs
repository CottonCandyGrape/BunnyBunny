using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UserInfo
{
    public string NickName = "";
    public int level = 1;
    public float exp = 0.0f;
    public int diaNum = 0;
    public int gold = 0;
    public Image profile_Img = null;
}
