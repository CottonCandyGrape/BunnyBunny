using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PopUpType { Store, Reinforce, Setting, Pause }

public class PopUpBox : MonoBehaviour
{
    const int Kilo = 1000;
    const int Million = 1000000;

    public PopUpType PopUpBoxType = PopUpType.Store;
    ReinType RfType = ReinType.Attack;

    public Button Exit_Btn = null;
    public Text Title_Txt = null;
    public Text Msg_Txt = null;
    public Button Ok_Btn = null;
    public Button Rein_Btn = null;
    public Text Gold_Txt = null;

    string[] reinTitles = { "힘", "체력", "인내", "회복" };
    string[] reinMsgs = { "공격력 +", "HP +", "방어구 +", "당근 회복 +" };
    int reinVal = 0;
    int GoldVal = 0;

    void Start()
    {
        if (Exit_Btn)
            Exit_Btn.onClick.AddListener(ExitBtnClick);

        if (Rein_Btn)
            Rein_Btn.onClick.AddListener(ReinBtnClick);

        if (Ok_Btn)
            Ok_Btn.onClick.AddListener(OKBtnClick);
    }

    public void SetReinInfo(ReinType rType) //TODO : Title, Msg, ReinBtn
    {
        RfType = rType;
        reinVal = 3;
        GoldVal = 1000;
        Title_Txt.text = reinTitles[(int)rType];
        Msg_Txt.text = reinMsgs[(int)rType] + reinVal.ToString(); //TODO : cellLV에 따른 증가량
        Gold_Txt.text = "x " + GoldVal.ToString(); //TODO : cellLV에 따라 다른 가격 
    }

    void OKBtnClick()
    {
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }

    void ReinBtnClick() //TODO : 가격 고려하여 유저정보에 반영. UI에도 반영
    {
        int rGold;
        //TODO : 뒤에 K or M 있는거 변환해줘야한다.
        string subGoldTxt = Gold_Txt.text.Substring(2);
        if (int.TryParse(subGoldTxt, out rGold))
        {
            int uGold = AllSceneMgr.Instance.user.gold;
            if (rGold <= uGold)
            {
                AllSceneMgr.Instance.user.gold -= rGold;
                switch (RfType)
                {
                    case ReinType.Attack:
                        AllSceneMgr.Instance.user.attack += reinVal;
                        break;
                    case ReinType.Defense:
                        AllSceneMgr.Instance.user.defense += reinVal;
                        break;
                    case ReinType.Heal:
                        AllSceneMgr.Instance.user.heal += reinVal;
                        break;
                    case ReinType.Hp:
                        AllSceneMgr.Instance.user.hp += reinVal;
                        break;
                }

                AllSceneMgr.Instance.WriteUserInfo();
                AllSceneMgr.Instance.RefreshTopUI();
                AllSceneMgr.Instance.InitStorePopUp("강화 성공.");
                //TODO : 강화되면 다음에는 못누르게 해야한다.
            }
            else
            {
                AllSceneMgr.Instance.InitStorePopUp("보유 골드가 부족합니다.");
                return;
            }
        }
        else
        {
            AllSceneMgr.Instance.InitStorePopUp("K 또는 M이 있거나 다른 문자열이 껴있슴...");
            return;
        }

        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }

    void ExitBtnClick()
    {
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }

    public void SetMsgText(string msg)
    {
        Msg_Txt.text = msg;
    }
}