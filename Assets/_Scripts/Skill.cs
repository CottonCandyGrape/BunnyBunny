using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Skill : MonoBehaviour
{
    protected const int MaxLevel = 3;

    protected int curLevel = 0;
    public int CurLv { get { return curLevel; } }

    abstract public void LevelUpSkill();
    abstract public string GetExplainText();
}
