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
        SceneManager.LoadScene("UpLowUI");
        SceneManager.LoadScene("Store", LoadSceneMode.Additive);
    }

    void InventoryBtnClick()
    {
        SceneManager.LoadScene("UpLowUI");
        SceneManager.LoadScene("Inventory", LoadSceneMode.Additive);
    }

    void BattleBtnClick()
    {
        SceneManager.LoadScene("UpLowUI");
        SceneManager.LoadScene("Battle", LoadSceneMode.Additive);
    }

    void ReinforceBtnClick()
    {
        SceneManager.LoadScene("UpLowUI");
        SceneManager.LoadScene("Reinforce", LoadSceneMode.Additive);
    }

    void PressCurSceneBtn()
    {
        string SceneName = "";

        if (SceneManager.loadedSceneCount > 1)
            SceneName = SceneManager.GetSceneAt(1).name;

        if (sceneBtnDic.ContainsKey(SceneName))
        {
            Image btn_Img = sceneBtnDic[SceneName].transform.GetChild(0).GetComponent<Image>();
            if (btn_Img != null)
                btn_Img.color = pressedColor;
        }
    }

    public void RefreshTopUI()
    {
        Exp_Img.fillAmount = 0.7f; //TODO : 바꿔야함.
        Nickname_Txt.text = AllSceneMgr.Instance.user.NickName;
        Dia_Txt.text = AllSceneMgr.Instance.user.diaNum.ToString() + " / 30";
        Gold_Txt.text = AllSceneMgr.Instance.user.gold.ToString();
        Level_Txt.text = AllSceneMgr.Instance.user.level.ToString();
    }
}