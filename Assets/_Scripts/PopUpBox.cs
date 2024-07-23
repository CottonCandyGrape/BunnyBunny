using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Text GoldMsg_Txt = null;
    public Text KillMsg_Txt = null;
    public Text ExpMsg_Txt = null;
    public Button Ok_Btn = null;
    //public Button Reward_Btn = null;
    //const float posX = 120.0f;
    //Vector2 btnPos = Vector2.zero;

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
    public Toggle Bgm_Tgl = null;
    public Toggle Sfx_Tgl = null;
    public Toggle JoyStick_Tgl = null;
    public Dropdown Language_Drop = null;

    string[] reinTitles = { "힘", "체력", "인내", "회복" };
    string[] reinMsgs = { "공격력 +", "HP +", "방어력 +", "케이크 회복 +" };
    int reinVal = 0;
    int GoldVal = 0;
    int cellNum = 0;

    void Start()
    {
        SoundMgr.Instance.PlaySfxSound("pop");

        if (Exit_Btn)
            Exit_Btn.onClick.AddListener(ExitBtnClick);

        if (Rein_Btn)
            Rein_Btn.onClick.AddListener(ReinBtnClick);

        if (Ok_Btn)
            Ok_Btn.onClick.AddListener(OKBtnClick);

        //if (Reward_Btn)
        //    Reward_Btn.onClick.AddListener(RewardBtnClick);

        if (Equip_Btn)
            Equip_Btn.onClick.AddListener(EquipBtnClick);

        if (Bgm_Tgl)
            Bgm_Tgl.onValueChanged.AddListener(BgmToggleClick);

        if (Sfx_Tgl)
            Sfx_Tgl.onValueChanged.AddListener(SfxToggleClick);

        if (JoyStick_Tgl)
            JoyStick_Tgl.onValueChanged.AddListener(JoyStickToggleClick);

        if (Language_Drop)
        {
            InitLangDropDown();

            Language_Drop.onValueChanged.AddListener(LangChange);
        }

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

    void InitLangDropDown()
    {
        Language_Drop.ClearOptions();
        List<string> options = new List<string> { "한국어", "English" };
        Language_Drop.AddOptions(options);

        int idx = -1;
        if (PlayerPrefs.HasKey("LangNum"))
            idx = PlayerPrefs.GetInt("LangNum");
        else
        {
            idx = GetLangIndex();
            PlayerPrefs.SetInt("LangNum", idx);
        }

        Language_Drop.value = idx;
        Language_Drop.RefreshShownValue();
    }

    int GetLangIndex()
    {
        SystemLanguage systemLan = Application.systemLanguage;
        if (systemLan == SystemLanguage.Korean)
            return 0;
        else if (systemLan == SystemLanguage.English)
            return 1;
        return 1;
    }

    void LangChange(int value)
    {
        PlayerPrefs.SetInt("LangNum", value);
        Language_Drop.value = value;
        Language_Drop.RefreshShownValue();
    }

    public void SetReinInfo(ReinCellButton rCell)
    {
        reinCell = rCell;
        RfType = rCell.RfType;
        cellNum = rCell.cellNum;
        int lv = (rCell.cellNum / 3) + 1;
        reinVal = lv * 3;
        GoldVal = 1000 + (lv - 1) * 1500;
        Title_Txt.text = reinTitles[(int)RfType];
        Msg_Txt.text = reinMsgs[(int)RfType] + reinVal.ToString();

        if (cellNum < AllSceneMgr.Instance.user.ReinCursor)
        {
            Rein_Btn.gameObject.SetActive(false);
            Ok_Btn.gameObject.SetActive(true);
        }
        else if (AllSceneMgr.Instance.user.ReinCursor <= cellNum)
        {
            Rein_Btn.gameObject.SetActive(true);
            Ok_Btn.gameObject.SetActive(false);

            Gold_Txt.text = "x " + GoldVal.ToString();
        }
    }

    void TryReinforce()
    {
        if (AllSceneMgr.Instance.user.ReinCursor < cellNum)
        {
            AllSceneMgr.Instance.InitMsgPopUp("아직 강화하실 수 없습니다.");
            return;
        }

        int rGold;
        string subGoldTxt = Gold_Txt.text.Substring(2); //TODO : 뒤에 K or M 있는거 변환해줘야한다.
        if (int.TryParse(subGoldTxt, out rGold))
        {
            int uGold = AllSceneMgr.Instance.user.Gold;
            if (rGold <= uGold)
                AllSceneMgr.Instance.ReinSuccess(RfType, rGold, reinVal, reinCell);
            else
                AllSceneMgr.Instance.InitMsgPopUp("보유 골드가 부족합니다.");
        }
        else
            AllSceneMgr.Instance.InitMsgPopUp("K 또는 M이 있거나 다른 문자열이 껴있슴...");
    }

    public void SetSettingInfo()
    {
        Bgm_Tgl.isOn = AllSceneMgr.Instance.user.Bgm;
        Sfx_Tgl.isOn = AllSceneMgr.Instance.user.Sfx;
        JoyStick_Tgl.isOn = AllSceneMgr.Instance.user.Joystick;
    }

    void SetAlpha()
    {
        if (AllSceneMgr.Instance.user.ReinCursor < cellNum
            && Rein_Btn.gameObject.activeSelf)
            Alpha_RImg.gameObject.SetActive(true);
        else
            Alpha_RImg.gameObject.SetActive(false);
    }

    public void SetMsgText(string msg)
    {
        Msg_Txt.text = msg;
    }

    public void SetGameOverText(bool isClear, float gold, int kill, float exp)
    {
        if (isClear)
        {
            Title_Txt.text = "Game Clear!";
            SoundMgr.Instance.PlaySfxSound("gameClear");
        }
        else
            Title_Txt.text = "Game Over";

        GoldMsg_Txt.text = gold.ToString();
        KillMsg_Txt.text = kill.ToString();
        ExpMsg_Txt.text = exp.ToString();
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

    //public void SetRewardBtnPos()
    //{
    //    Reward_Btn.gameObject.SetActive(true);
    //    btnPos = Reward_Btn.transform.localPosition;
    //    btnPos.x = posX;
    //    Reward_Btn.transform.localPosition = btnPos;
    //    btnPos.x = -posX;
    //    Ok_Btn.transform.localPosition = btnPos;
    //}

    //Click Functions
    void EquipBtnClick()
    {
        SoundMgr.Instance.PlaySfxSound("pop");

        if (invBtn.isUpper) //아래로 내려야함.
        {
            if (lower != null)
            {
                invBtn.transform.SetParent(lower);
                AllSceneMgr.Instance.user.IsEquiped[(int)invBtn.InvType] = string.Empty;
            }
        }
        else
        {
            if (invMgr != null)
            {
                if (upper != null)
                {
                    invBtn.transform.SetParent(upper);
                    AllSceneMgr.Instance.user.IsEquiped[(int)invBtn.InvType] = invBtn.InvName;
                }

                invBtn.transform.position = invMgr.UpInvenPos[(int)invBtn.InvType].transform.position;
            }
        }

        AllSceneMgr.Instance.WriteUserInfo();

        Destroy(gameObject);
    }

    void ReinBtnClick()
    {
        TryReinforce();

        Destroy(gameObject);
    }

    void ExitBtnClick()
    {
        SoundMgr.Instance.PlaySfxSound("pop");

        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }

    void OKBtnClick()
    {
        SoundMgr.Instance.PlaySfxSound("pop");

        if (PopUpBoxType == PopUpType.Setting)
            AllSceneMgr.Instance.WriteUserInfo();
        else if (PopUpBoxType == PopUpType.Pause) //Pasue는 Ingame에서만 나옴.
        {
            SoundMgr.Instance.TurnOffSound();
            GameMgr.Inst.GoToBattleScene(false);
            return;
        }
        else if (PopUpBoxType == PopUpType.Msg && //Msg && InGame일 경우는 GameOver일 경우뿐.
            SceneManager.GetActiveScene().name == "InGame")
        {
            Ok_Btn.interactable = false;
            StartCoroutine(AllSceneMgr.Instance.adsMgr.ShowInterstitialAd());
            return;
        }

        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }

    //void RewardBtnClick()
    //{
    //    AllSceneMgr.Instance.adsMgr.ShowRewardedAd();

    //    Destroy(gameObject);
    //}

    void BgmToggleClick(bool isOn)
    {
        SoundMgr.Instance.PlaySfxSound("btnClick");

        if (isOn)
            SoundMgr.Instance.PlayBGM("UIScene");
        else
        {
            SoundMgr.Instance.AudioSrc.Stop();
            SoundMgr.Instance.AudioSrc.clip = null;
        }

        AllSceneMgr.Instance.user.Bgm = isOn;

        SoundMgr.Instance.SetMuteOnOff();
    }

    void SfxToggleClick(bool isOn)
    {
        SoundMgr.Instance.PlaySfxSound("btnClick");

        AllSceneMgr.Instance.user.Sfx = isOn;

        SoundMgr.Instance.SetMuteOnOff();
    }

    void JoyStickToggleClick(bool isOn)
    {
        SoundMgr.Instance.PlaySfxSound("btnClick");

        AllSceneMgr.Instance.user.Joystick = isOn;
    }
    //Click Functions
}