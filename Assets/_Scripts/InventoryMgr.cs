using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMgr : MonoBehaviour
{
    public Text Hp_Txt = null;
    public Text Atk_Txt = null;

    void Start()
    {
        RefreshInvenTxts();
    }

    void RefreshInvenTxts()
    {
        if (Hp_Txt != null)
            Hp_Txt.text = AllSceneMgr.Instance.user.hp.ToString();

        if (Atk_Txt != null)
            Atk_Txt.text = AllSceneMgr.Instance.user.attack.ToString();
    }
}