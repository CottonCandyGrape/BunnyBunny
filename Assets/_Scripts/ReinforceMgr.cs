using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReinforceMgr : MonoBehaviour
{
    [HideInInspector] public List<ReinCellButton> ReinCellList = new List<ReinCellButton>();

    public GameObject[] ReinCellPrefab = null;
    public Transform ScrollContent = null;
    public Image ScrollView_Img = null;

    const int CellPerLv = 3;

    void Start()
    {
        ReinCellButton.reinBtnCnt = 0; //static 변수 0으로 초기화 해줌. Scene이 반복되는 경우 Level이 높게 표시되는 버그 있음.
        ScrollView_Img.enabled = false;

        InitReinCells();
    }

    void InitReinCells()
    {
        int genCnt = AllSceneMgr.Instance.user.level * CellPerLv;

        for (int i = 0; i < genCnt; i++)
        {
            int idx = i % ReinCellPrefab.Length;
            GameObject cell = Instantiate(ReinCellPrefab[idx], ScrollContent);
            ReinCellButton reinBtn = cell.GetComponent<ReinCellButton>();
            if (reinBtn != null) ReinCellList.Add(reinBtn);
        }
    }
}