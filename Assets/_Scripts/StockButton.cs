using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Button Stk_Btn = null;

    void Start()
    {
        if (!Stk_Btn)
            Stk_Btn = GetComponent<Button>();

        Stk_Btn.onClick.AddListener(ButtonClick);
    }

    void ButtonClick()
    {
        switch (StkType)
        {
            case StockType.Weapon:
                AllSceneMgr.Instance.InitPopUpMsg("준비 중입니다.");
                break;
            case StockType.Armor:
                AllSceneMgr.Instance.InitPopUpMsg("준비 중입니다.");
                break;
            case StockType.Dia:
                break;
            case StockType.Gold:
                AllSceneMgr.Instance.InitPopUpMsg("준비 중입니다.");
                break;
        }
    }
}