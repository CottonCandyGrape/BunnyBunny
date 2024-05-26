using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : MonoBehaviour
{
    protected const int MaxLevel = 3;

    protected bool isEvolve = false;
    public bool IsEvolve { get { return isEvolve; } }
    protected int curLevel = 0;
    public int CurLv { get { return curLevel; } }
    protected string ev_Name;
    public string Ev_Name { get { return ev_Name; } }

    abstract public void LevelUpWeapon();
    abstract public void EvolveWeapon();
    abstract public string GetExplainText();
}