using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceMgr : MonoBehaviour
{
    public GameObject[] ReinCellPrefab = null;
    public Transform ScrollContent = null;

    const int CellPerLv = 3;

    void Start()
    {
        ReinCellButton.reinBtnCnt = 0; //static 변수 0으로 초기화 해줌. Scene이 반복되는 경우 Level이 높게 표시되는 버그 있음.

        InitReinCells();
    }

    void InitReinCells()
    {
        int genCnt = AllSceneMgr.Instance.user.level * CellPerLv;

        for (int i = 0; i < genCnt; i++)
        {
            int idx = i % ReinCellPrefab.Length;
            Instantiate(ReinCellPrefab[idx], ScrollContent);
        }
    }

    //void Update() { }
}