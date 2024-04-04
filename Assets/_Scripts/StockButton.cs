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
                Debug.Log("준비중입니다."); //TODO : 팝업창으로 바꾸기
                break;
            case StockType.Armor:
                Debug.Log("준비중입니다.");
                break;
            case StockType.Dia:
                break;
            case StockType.Gold:
                Debug.Log("준비중입니다.");
                break;
        }
    }
}