using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : G_Singleton<SoundMgr>
{
    [HideInInspector] public AudioSource AudioSrc = null;
    Dictionary<string, AudioClip> adClipDict = new Dictionary<string, AudioClip>();

    protected override void Init()
    {
        base.Init();
        AudioSrc = GetComponent<AudioSource>();
    }

    void Start()
    {
        AudioClipLoad();
    }

    void AudioClipLoad()
    {
        AudioClip audioClip = null;
        object[] temp = Resources.LoadAll("Sounds");
        for(int i=0;i <temp.Length; i++ )
        {
            audioClip = temp[i] as AudioClip;
            if (adClipDict.ContainsKey(audioClip.name))
                continue;
            adClipDict.Add(audioClip.name, audioClip);
        }
    }

    public void PlayBGM(string fileName, float volume)
    {
        AudioClip clip = null;
        if(adClipDict.ContainsKey(fileName))
        {
            clip = adClipDict[fileName] as AudioClip;
        }
        else
        {
            clip = Resources.Load("Sounds/" + fileName) as AudioClip;
            adClipDict.Add(fileName, clip);
        }

        if (AudioSrc == null) return;

        if (AudioSrc.clip != null && AudioSrc.clip.name == fileName)
            return;

        AudioSrc.clip = clip;
        AudioSrc.volume = volume;  //TODO : 비율로 적용?
        AudioSrc.loop = true;
        AudioSrc.Play();
    }
}