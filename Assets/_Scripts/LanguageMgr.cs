using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageMgr : MonoBehaviour
{
    Dictionary<string, string> langDict;

    void Start() 
    {
        InitLangDict();
    } 

    public void InitLangDict()
    {
        string filePath = "Languages/" + GetLangFileName();
        string jsonStr = Resources.Load<TextAsset>(filePath).text;
        LanguagePack langPack = JsonUtility.FromJson<LanguagePack>(jsonStr);

        langDict = new Dictionary<string, string>();
        for (int i = 0; i < langPack.lang_pack.Length; i++)
            langDict.Add(langPack.lang_pack[i].key, langPack.lang_pack[i].value);
    }

    public string GetLangValue(string key)
    {
        string value = "missing";
        if (langDict.ContainsKey(key))
            value = langDict[key];
        return value;
    }

    public void UpdateLangValue()
    {
        TextSetter[] texts = FindObjectsOfType<TextSetter>();
        for (int i = 0; i < texts.Length; i++)
            texts[i].SetText();
    }

    string GetLangFileName()
    {
        int idx = PlayerPrefs.GetInt("LangNum");
        if (idx == 0)
            return "Korean";
        else if (idx == 1)
            return "English";
        return "English";
    }

    public void CheckLangDict()
    {
        foreach(KeyValuePair<string, string> kvp in langDict)
            Debug.Log(kvp.Key + " / " + kvp.Value);
    }
}

[Serializable]
public class LanguagePack
{
    public LangPair[] lang_pack;
}

[Serializable]
public class LangPair
{
    public string key;
    public string value;
}
