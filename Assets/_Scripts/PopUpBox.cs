using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PopUpType { Msg, Inventory, Reinforce, Setting, Pause }

public class PopUpBox : MonoBehaviour
{
    const int Kilo = 1000;
    const int Million = 1000000;

    public PopUpType PopUpBoxType = PopUpType.Msg;
    ReinType RfType = ReinType.Attack;

    [Header("------ Message ------")]
    public Text Title_Txt = null;
    public Text Msg_Txt = null;
    public Button Ok_Btn = null;

    [Header("------ Inventory ------")]
    public Image Inven_Img = null;
    public Button Equip_Btn = null;
    public Text Equip_Txt = null;
    InvenButton invBtn = null;
    Transform upper = null;
    Transform lower = null;

    InventoryMgr invMgr = null;

    [Header("------ Reinforece ------")]
    public Button Exit_Btn = null;
    public Button Rein_Btn = null;
    public Text Gold_Txt = null;
    public RawImage Alpha_RImg = null;
    ReinCellButton reinCell = null;

    [Header("------ Setting ------")]
    public Slider Bgm_Sld = null;
    public Slider Sfx_Sld = null;
    public Toggle JoyStick_Tgl = null;

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

        if (Equip_Btn)
            Equip_Btn.onClick.AddListener(EquipBtnClick);

        if (Bgm_Sld)
            Bgm_Sld.onValueChanged.AddListener(BgmSliderMove);

        if (Sfx_Sld)
            Sfx_Sld.onValueChanged.AddListener(SfxSliderMove);

        if (JoyStick_Tgl)
            JoyStick_Tgl.onValueChanged.AddListener(JoyStickToggleClick);

        if (PopUpBoxType == PopUpType.Reinforce)
            SetAlpha();
        else if (PopUpBoxType == PopUpType.Inventory)
        {
            if (lower == null)
                lower = GameObject.Find("Content").transform;

            if (upper == null)
                upper = GameObject.Find("Upper_Panel").transform;

            if (invMgr == null)
                invMgr = FindObjectOfType<InventoryMgr>();
        }
    }

    public void SetReinInfo(ReinCellButton rCell)
    {
        reinCell = rCell;
        RfType = rCell.RfType;
        cellNum = rCell.cellNum;
        reinVal = 3;
        GoldVal = 1000;
        Title_Txt.text = reinTitles[(int)RfType];
        Msg_Txt.text = reinMsgs[(int)RfType] + reinVal.ToString(); //TODO : cellLV에 따른 증가량

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
                AllSceneMgr.Instance.ReinSuccess(RfType, rGold, reinVal, reinCell);
            else
                AllSceneMgr.Instance.InitMsgPopUp("보유 골드가 부족합니다.");
        }
        else
            AllSceneMgr.Instance.InitMsgPopUp("K 또는 M이 있거나 다른 문자열이 껴있슴...");
    }

    void SetAlpha()
    {
        if (AllSceneMgr.Instance.user.reinCursor < cellNum
            && Rein_Btn.gameObject.activeSelf)
            Alpha_RImg.gameObject.SetActive(true);
        else
            Alpha_RImg.gameObject.SetActive(false);
    }

    public void SetMsgText(string msg)
    {
        Msg_Txt.text = msg;
    }

    public void SetInvenComp(InvenButton iBtn)
    {
        invBtn = iBtn;

        Inven_Img.sprite = invBtn.Inven_Img.sprite;
        Inven_Img.rectTransform.sizeDelta = invBtn.Inven_Img.rectTransform.sizeDelta;
        if (invBtn.isUpper)
            Equip_Txt.text = "장착 해제";
        else
            Equip_Txt.text = "장착하기";
    }

    //Click Functions
    void EquipBtnClick()
    {
        if (invBtn.isUpper) //아래로 내려야함.
        {
            if (lower != null)
                invBtn.transform.SetParent(lower);
        }
        else
        {
            if (invMgr != null)
            {
                if (upper != null)
                    invBtn.transform.SetParent(upper);

                invBtn.transform.position = invMgr.UpInvenPos[(int)invBtn.InvType].transform.position;
            }
        }

        Destroy(gameObject);
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

    void BgmSliderMove(float value)
    {

    }

    void SfxSliderMove(float value)
    {

    }

    void JoyStickToggleClick(bool isOn)
    {
        AllSceneMgr.Instance.user.joystick = isOn;
    }

    void OKBtnClick()
    {
        AllSceneMgr.Instance.WriteUserInfo();

        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }
}