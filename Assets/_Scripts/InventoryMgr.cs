using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InvenType { Weapon, Glove, Armor, Shoes }

public class InventoryMgr : MonoBehaviour
{
    public Text Hp_Txt = null;
    public Text Atk_Txt = null;
    public Image Scroll_Image = null;
    public Transform Content = null;
    public Transform UpPnl = null;
    public GameObject[] UpInvenPos = null;
    public GameObject[] InvenPrefabs = null;

    void Start()
    {
        RefreshInvenTxts();

        InitContents();
    }

    void RefreshInvenTxts()
    {
        if (Hp_Txt != null)
            Hp_Txt.text = AllSceneMgr.Instance.user.Hp.ToString();

        if (Atk_Txt != null)
            Atk_Txt.text = AllSceneMgr.Instance.user.Attack.ToString();
    }

    void InitContents()
    {
        for (int i = 0; i < InvenPrefabs.Length; i++)
        {
            Instantiate(InvenPrefabs[i], Content);
        }
    }
}