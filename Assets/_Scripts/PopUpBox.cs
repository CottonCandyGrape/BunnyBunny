using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpBox : MonoBehaviour
{
    public Text Msg_Txt = null;
    public Button Ok_Btn = null;

    void Start()
    {
        if (Ok_Btn)
            Ok_Btn.onClick.AddListener(OKBtnClick);
    }

    void OKBtnClick()
    {
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }

    public void SetMsgText(string msg)
    {
        Msg_Txt.text = msg;
    }
}