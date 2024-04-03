using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StockType
{
    Weapon,
    Armor,
    Dia,
    Gold
}

public class StockButton : MonoBehaviour
{
    public StockType StkType = StockType.Weapon;
    public int StockNum = 0;
    public int StockPrice = 0;
}