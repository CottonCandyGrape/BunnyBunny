using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StockType { Weapon, Armor, Dia, Gold }

public class StockButton : MonoBehaviour
{
    public StockType StkType = StockType.Weapon;
    public int StockNum = 0;
    public int StockPrice = 0;

    public Button Stk_Btn = null;

    const int MaxDia = 30;

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
                AllSceneMgr.Instance.InitMsgPopUp("준비 중입니다.");
                break;
            case StockType.Armor:
                AllSceneMgr.Instance.InitMsgPopUp("준비 중입니다.");
                break;
            case StockType.Dia:
                TryBuyDia();
                break;
            case StockType.Gold:
                AllSceneMgr.Instance.InitMsgPopUp("준비 중입니다.");
                break;
        }
    }

    void TryBuyDia()
    {
        int curDia = AllSceneMgr.Instance.user.diaNum;
        int curGold = AllSceneMgr.Instance.user.gold;

        if (curGold < StockPrice)
        {
            AllSceneMgr.Instance.InitMsgPopUp("보유 골드가 부족합니다.");
            return;
        }
        else if (MaxDia < curDia + StockNum)
        {
            AllSceneMgr.Instance.InitMsgPopUp("최대 보유 개수보다 더 많이 구매하실 수 없습니다.");
            return;
        }
        //구매 실패하면 바로 return;

        AllSceneMgr.Instance.GetDia(StockNum, StockPrice);
    }
}