using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PopUpType { Msg, Reinforce, Setting, Pause }

public class PopUpBox : MonoBehaviour
{
    const int Kilo = 1000;
    const int Million = 1000000;

    public PopUpType PopUpBoxType = PopUpType.Msg;
    ReinType RfType = ReinType.Attack;

    public Button Exit_Btn = null;
    public Text Title_Txt = null;
    public Text Msg_Txt = null;
    public Button Ok_Btn = null;
    public Button Rein_Btn = null;
    public Text Gold_Txt = null;
    public RawImage Alpha_RImg = null;

    string[] reinTitles = { "힘", "체력", "인내", "회복" };
    string[] reinMsgs = { "공격력 +", "HP +", "방어력 +", "당근 회복 +" };
    int reinVal = 0;
    int GoldVal = 0;
    int cellNum = 0;

    void Start()
    {
        if (Exit_Btn)
            Exit_Btn.onClick.AddListener(ExitBtnClick);

        if (Rein_Btn)
            Rein_Btn.onClick.AddListener(ReinBtnClick);

        if (Ok_Btn)
            Ok_Btn.onClick.AddListener(OKBtnClick);

        if (PopUpBoxType == PopUpType.Reinforce)
            SetAlpha();
    }

    public void SetReinInfo(ReinType rType, int cNum) //TODO : ReinBtn or OkBtn
    {
        RfType = rType;
        cellNum = cNum;
        reinVal = 3;
        GoldVal = 1000;
        Title_Txt.text = reinTitles[(int)rType];
        Msg_Txt.text = reinMsgs[(int)rType] + reinVal.ToString(); //TODO : cellLV에 따른 증가량

        if (cellNum < AllSceneMgr.Instance.user.reinCursor)
        {
            Rein_Btn.gameObject.SetActive(false);
            Ok_Btn.gameObject.SetActive(true);
        }
        else if (AllSceneMgr.Instance.user.reinCursor <= cellNum)
        {
            Rein_Btn.gameObject.SetActive(true);
            Ok_Btn.gameObject.SetActive(false);

            Gold_Txt.text = "x " + GoldVal.ToString(); //TODO : cellLV에 따라 다른 가격 
        }
    }

    void OKBtnClick()
    {
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }

    void TryReinforce()
    {
        if (AllSceneMgr.Instance.user.reinCursor < cellNum)
        {
            AllSceneMgr.Instance.InitMsgPopUp("아직 강화하실 수 없습니다.");
            return;
        }

        int rGold;
        string subGoldTxt = Gold_Txt.text.Substring(2); //TODO : 뒤에 K or M 있는거 변환해줘야한다.
        if (int.TryParse(subGoldTxt, out rGold))
        {
            int uGold = AllSceneMgr.Instance.user.gold;
            if (rGold <= uGold)
                AllSceneMgr.Instance.ReinSuccess(RfType, rGold, reinVal, cellNum);
            else
                AllSceneMgr.Instance.InitMsgPopUp("보유 골드가 부족합니다.");
        }
        else
            AllSceneMgr.Instance.InitMsgPopUp("K 또는 M이 있거나 다른 문자열이 껴있슴...");
    }

    void ReinBtnClick()
    {
        TryReinforce();

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

    void SetAlpha()
    {
        if (AllSceneMgr.Instance.user.reinCursor < cellNum
            && Rein_Btn.gameObject.activeSelf)
            Alpha_RImg.gameObject.SetActive(true);
        else
            Alpha_RImg.gameObject.SetActive(false);
    }
}