using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpLowUIMgr : MonoBehaviour
{
    Color pressedColor = new Color32(118, 127, 136, 225);

    Dictionary<string, Button> sceneBtnDic;

    [Header("------ Top UI ------")]
    public Image Exp_Img = null;
    public Text Nickname_Txt = null;
    public Text Dia_Txt = null;
    public Text Gold_Txt = null;
    public Text Level_Txt = null;

    [Header("------ Bottom UI ------")]
    public Button Store_Btn = null;
    public Button Inventory_Btn = null;
    public Button Battle_Btn = null;
    public Button Reinforce_Btn = null;

    void Awake()
    {
        sceneBtnDic = new Dictionary<string, Button> {
            { "Store", Store_Btn },
            { "Inventory", Inventory_Btn },
            { "Battle", Battle_Btn },
            { "Reinforce", Reinforce_Btn },
        };
    }

    void Start()
    {
        if (Store_Btn)
            Store_Btn.onClick.AddListener(StoreBtnClick);

        if (Inventory_Btn)
            Inventory_Btn.onClick.AddListener(InventoryBtnClick);

        if (Battle_Btn)
            Battle_Btn.onClick.AddListener(BattleBtnClick);

        if (Reinforce_Btn)
            Reinforce_Btn.onClick.AddListener(ReinforceBtnClick);

        PressCurSceneBtn();

        RefreshTopUI();
    }

    void StoreBtnClick()
    {
        if (AllSceneMgr.Instance.user.Sfx)
            SoundMgr.Instance.PlayGUISound("btnClick");

        SceneManager.LoadScene("Store");
        SceneManager.LoadScene("UpLowUI", LoadSceneMode.Additive);
    }

    void InventoryBtnClick()
    {
        if (AllSceneMgr.Instance.user.Sfx)
            SoundMgr.Instance.PlayGUISound("btnClick");

        SceneManager.LoadScene("Inventory");
        SceneManager.LoadScene("UpLowUI", LoadSceneMode.Additive);
    }

    void BattleBtnClick()
    {
        if (AllSceneMgr.Instance.user.Sfx)
            SoundMgr.Instance.PlayGUISound("btnClick");

        SceneManager.LoadScene("Battle");
        SceneManager.LoadScene("UpLowUI", LoadSceneMode.Additive);
    }

    void ReinforceBtnClick()
    {
        if (AllSceneMgr.Instance.user.Sfx)
            SoundMgr.Instance.PlayGUISound("btnClick");

        SceneManager.LoadScene("Reinforce");
        SceneManager.LoadScene("UpLowUI", LoadSceneMode.Additive);
    }

    void PressCurSceneBtn()
    {
        string SceneName = "";

        if (SceneManager.loadedSceneCount > 1)
            SceneName = SceneManager.GetSceneAt(0).name;

        if (sceneBtnDic.ContainsKey(SceneName))
        {
            Image btn_Img = sceneBtnDic[SceneName].transform.GetChild(0).GetComponent<Image>();
            if (btn_Img != null)
                btn_Img.color = pressedColor;
        }
    }

    public void RefreshTopUI()
    {
        Exp_Img.fillAmount = (AllSceneMgr.Instance.user.CurExp - AllSceneMgr.Instance.user.PrevExp) /
            (AllSceneMgr.Instance.user.NextExp - AllSceneMgr.Instance.user.PrevExp);
        Nickname_Txt.text = AllSceneMgr.Instance.user.NickName;
        Dia_Txt.text = AllSceneMgr.Instance.user.DiaNum.ToString() + " / 30";
        Gold_Txt.text = AllSceneMgr.Instance.user.Gold.ToString();
        Level_Txt.text = AllSceneMgr.Instance.user.Level.ToString();
    }
}