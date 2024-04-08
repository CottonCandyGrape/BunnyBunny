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

    public ReinType RfType; //Inspector에서 초기화

    public static int reinBtnCnt = 0;
    const int CellPerLv = 3;
    int cellLv = 0;

    void Start()
    {
        if (!Rein_Btn)
            Rein_Btn = GetComponentInChildren<Button>();
        Rein_Btn.onClick.AddListener(ReinBtnClick);

        Init();
    }

    //void Update() { }

    void Init() //level txt, bar_img on, off
    {
        cellLv = (reinBtnCnt / CellPerLv) + 1;

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

    void ReinBtnClick() //TODO : 팝업 먼저 띄우기 -> 메세지 뭐? 나올지
    {
        AllSceneMgr.Instance.InitReinPopUp(RfType);
    }
}