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

    public ReinType ReinforceType = ReinType.Attack;

    public static int reinBtnCnt = 0;
    const int CellPerLv = 3;

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
        switch (ReinforceType)
        {
            case ReinType.Attack:
                break;
            case ReinType.Hp:
                break;
            case ReinType.Defense:
                break;
            case ReinType.Heal:
                break;
        }
    }
}