using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSetter : MonoBehaviour
{
    public string key;
    Text text = null;

    void Start()
    {
        text = GetComponent<Text>();

        SetText();
    }

    public void SetText()
    {
        if (AllSceneMgr.Instance.langMgr != null)
            text.text = AllSceneMgr.Instance.langMgr.GetLangValue(key);
    }
}
