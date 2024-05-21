using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ReinType { Attack, Hp, Defense, Heal }

public class ReinCellButton : MonoBehaviour
{
    public GameObject LevelText = null;
    public GameObject Bar_Img = null;
    public Button Rein_Btn = null;
    public RawImage Alpha_RImg = null;

    public ReinType RfType; //Inspector에서 초기화
    public int cellNum = 0;

    public static int reinBtnCnt = 0;
    const int CellPerLv = 3;

    void Start()
    {
        if (!Rein_Btn)
            Rein_Btn = GetComponentInChildren<Button>();
        Rein_Btn.onClick.AddListener(ReinBtnClick);

        Init();
        SetAlpha();
    }

    //void Update() { }

    void Init() //level txt, bar_img on, off
    {
        //cellLv = (reinBtnCnt / CellPerLv) + 1;
        cellNum = reinBtnCnt;

        reinBtnCnt++;

        if (reinBtnCnt % CellPerLv == 0)
        {
            LevelText.SetActive(true);
            Text lvTxt = LevelText.GetComponentInChildren<Text>();
            lvTxt.text = "LEVEL " + (reinBtnCnt / CellPerLv).ToString();
        }

        if (reinBtnCnt == AllSceneMgr.Instance.user.Level * CellPerLv)
            Bar_Img.SetActive(false);
    }

    void ReinBtnClick()
    {
        AllSceneMgr.Instance.InitReinPopUp(this);
    }

    public void SetAlpha()
    {
        if (cellNum < AllSceneMgr.Instance.user.ReinCursor)
            Alpha_RImg.gameObject.SetActive(true);
        else
            Alpha_RImg.gameObject.SetActive(false);
    }
}