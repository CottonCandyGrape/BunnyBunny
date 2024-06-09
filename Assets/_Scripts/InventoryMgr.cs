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
    public Transform UpPnl = null;
    public Transform LowPnl = null;
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

    void InitContents() //TODO : 아이템 종류, 개수가 1개니깐 가능한 코드. 추후 수정
    {
        for (int i = 0; i < InvenPrefabs.Length; i++)
        {
            GameObject inven = Instantiate(InvenPrefabs[i]);
            InvenButton invBtn = inven.GetComponent<InvenButton>();
            if (invBtn != null)
            {
                if (AllSceneMgr.Instance.user.IsEquiped[(int)invBtn.InvType])
                {
                    inven.transform.SetParent(UpPnl);
                    inven.transform.position = UpInvenPos[(int)invBtn.InvType].transform.position;
                }
                else
                {
                    inven.transform.SetParent(LowPnl);
                }
            }

            inven.transform.localScale = Vector3.one;
        }
    }
}