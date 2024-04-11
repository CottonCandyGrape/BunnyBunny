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

        //if (Scroll_Image != null) //TODO : 배경색을 바꿀까?...
        //    Scroll_Image.enabled = false;

    }

    void RefreshInvenTxts()
    {
        if (Hp_Txt != null)
            Hp_Txt.text = AllSceneMgr.Instance.user.hp.ToString();

        if (Atk_Txt != null)
            Atk_Txt.text = AllSceneMgr.Instance.user.attack.ToString();
    }

    void InitContents()
    {
        for (int i = 0; i < InvenPrefabs.Length; i++)
        {
            Instantiate(InvenPrefabs[i], Content);
        }
    }
}