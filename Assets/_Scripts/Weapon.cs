using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : MonoBehaviour
{
    protected const int MaxLevel = 3;

    protected int curLevel = 0;
    protected bool isEvolve = false;

    abstract public void LevelUpWeapon();
    abstract public void EvolveWeapon();
}