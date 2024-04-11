using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenButton : MonoBehaviour
{
    public InvenType InvType = InvenType.Weapon;
    public Image Inven_Img = null;
    public bool isUpper = false;
    Button Inven_Btn = null;

    void Start()
    {
        if (Inven_Btn == null)
            Inven_Btn = GetComponent<Button>();
        Inven_Btn.onClick.AddListener(InvenBtnClick);
    }

    void InvenBtnClick()
    {
        isUpper = IsUpperInven();
        AllSceneMgr.Instance.InitInvenPopUp(this);
    }

    bool IsUpperInven()
    {
        if (transform.parent.name.Contains("Content"))
            return false;
        else
            return true;
    }
}