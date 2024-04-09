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

    public static int reinBtnCnt = 0;
    const int CellPerLv = 3;
    //int cellLv = 0; //cellNum을 알면 cellLv을 알수있음.
    int cellNum = 0;

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

        if (reinBtnCnt == AllSceneMgr.Instance.user.level * CellPerLv)
            Bar_Img.SetActive(false);
    }

    void ReinBtnClick()
    {
        AllSceneMgr.Instance.InitReinPopUp(RfType, cellNum);
    }

    void SetAlpha()
    {
        if (cellNum < AllSceneMgr.Instance.user.reinCursor)
            Alpha_RImg.gameObject.SetActive(true);
        else
            Alpha_RImg.gameObject.SetActive(false);
    }
}