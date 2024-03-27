using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpObj : MonoBehaviour
{
    public Button Check_Btn = null;

    void Start()
    {
        if (Check_Btn)
            Check_Btn.onClick.AddListener(CheckBtnClick);
    }

    //void Update() { }

    void CheckBtnClick()
    {
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }
}