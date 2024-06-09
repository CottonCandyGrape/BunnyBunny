using System;
using System.IO;
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
    public GameObject Inven_Btn = null;

    [HideInInspector] public UserInventory userInven = new UserInventory();

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
        string filePath = Application.persistentDataPath + "/";
        string fileName = "PlayerInvens" + ".json";

        if (File.Exists(filePath + fileName))
        {
            string fromJson = File.ReadAllText(filePath + fileName);
            userInven = JsonUtility.FromJson<UserInventory>(fromJson);

            RefreshInvenBtns();
        }
        else
        {
            //맨 처음엔 총 하나 준다.
            Item bGun = GetBasicGun();
            userInven.ItemList.Add(bGun);

            string jsonStr = JsonUtility.ToJson(userInven);
            File.WriteAllText(filePath + fileName, jsonStr);

            //UI에서 버튼 추가
            GameObject inven = Instantiate(Inven_Btn, LowPnl);
            InvenButton invBtn = inven.GetComponent<InvenButton>();
            if (invBtn != null)
            {
                invBtn.InvType = InvenType.Weapon;
                invBtn.Inven_Img.sprite = Resources.Load<Sprite>("Images/gun");
                invBtn.InvName = bGun.Name;
                inven.transform.localScale = Vector3.one;
            }
        }
    }

    void RefreshInvenBtns()
    {
        for (int i = 0; i < userInven.ItemList.Count; i++)
        {
            GameObject inven = Instantiate(Inven_Btn);
            InvenButton invBtn = inven.GetComponent<InvenButton>();
            if (invBtn != null)
            {
                invBtn.InvType = (InvenType)Enum.Parse(typeof(InvenType), userInven.ItemList[i].Type);
                string itemName = userInven.ItemList[i].Name;
                invBtn.Inven_Img.sprite = Resources.Load<Sprite>("Images/" + itemName);
                invBtn.InvName = itemName;

                if (AllSceneMgr.Instance.user.IsEquiped[(int)invBtn.InvType] == itemName)
                {
                    inven.transform.SetParent(UpPnl);
                    inven.transform.position = UpInvenPos[(int)invBtn.InvType].transform.position;
                }
                else
                    inven.transform.SetParent(LowPnl);

                inven.transform.localScale = Vector3.one;
            }
        }
    }

    Item GetBasicGun()
    {
        Item item = new Item(); //맨 처음에는 총 준다.
        item.Name = "gun";
        item.Type = InvenType.Weapon.ToString();

        return item;
    }
}