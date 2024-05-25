using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : G_Singleton<SoundMgr>
{
    [HideInInspector] public AudioSource AudioSrc = null;
    Dictionary<string, AudioClip> adClipDict = new Dictionary<string, AudioClip>();

    AudioSource[] sfxSrcList = new AudioSource[10];
    const int MaxSfxCnt = 5;
    int sfxCnt = 0;

    protected override void Init()
    {
        base.Init();
        SetSfxSrc();
        SetClipDict();
    }

    void Start()
    {
        SetSoundOnOff();
    }

    void SetClipDict()
    {
        AudioClip audioClip = null;
        object[] temp = Resources.LoadAll("Sounds");
        for (int i = 0; i < temp.Length; i++)
        {
            audioClip = temp[i] as AudioClip;
            if (adClipDict.ContainsKey(audioClip.name))
                continue;
            adClipDict.Add(audioClip.name, audioClip);
        }
    }

    void SetSfxSrc()
    {
        AudioSrc = GetComponent<AudioSource>();

        // 게임 효과음 플레이를 위한 5개의 레이어 생성 코드
        for (int i = 0; i < MaxSfxCnt; i++)
        {
            GameObject newSoundObj = new GameObject(); // empty gameobject 만들기
            newSoundObj.transform.SetParent(this.transform);
            newSoundObj.transform.localPosition = Vector3.zero; //transform reset
            AudioSource a_AudioSrc = newSoundObj.AddComponent<AudioSource>();
            a_AudioSrc.playOnAwake = false;
            a_AudioSrc.loop = false;
            newSoundObj.name = "SoundEffObj";

            sfxSrcList[i] = a_AudioSrc;
        }
    }

    public void SetSoundOnOff()
    {
        bool mute = !AllSceneMgr.Instance.user.Bgm; //헷갈리니깐 거꾸로 

        if (AudioSrc != null)
        {
            AudioSrc.mute = mute;
            if (!mute && !AudioSrc.isPlaying)
                AudioSrc.time = 0;
        }

        mute = !AllSceneMgr.Instance.user.Sfx;

        for (int i = 0; i < sfxCnt; i++)
        {
            if (sfxSrcList[i] != null)
            {
                sfxSrcList[i].mute = mute;
                if (!mute)
                    sfxSrcList[i].time = 0;
            }
        }
    }

    public void PlayBGM(string fileName)
    {
        AudioClip clip = null;
        if (adClipDict.ContainsKey(fileName))
            clip = adClipDict[fileName] as AudioClip;

        if (clip == null)
        {
            Debug.Log("CLIP IS NULL");
            return;
        }

        if (AudioSrc.clip != null && AudioSrc.clip.name == fileName)
            return;

        AudioSrc.clip = clip;
        AudioSrc.volume = 1.0f;
        AudioSrc.loop = true;
        AudioSrc.Play();
    }

    public void PlaySfxSound(string fileName)
    {
        if (!AllSceneMgr.Instance.user.Sfx) return;

        AudioClip clip = null;
        if (adClipDict.ContainsKey(fileName))
            clip = adClipDict[fileName] as AudioClip;

        if (clip == null)
        {
            Debug.Log("CLIP IS NULL");
            return;
        }

        if (sfxSrcList[sfxCnt] != null)
        {
            sfxSrcList[sfxCnt].volume = 1.0f;
            sfxSrcList[sfxCnt].PlayOneShot(clip, 1.0f);
        }

        sfxCnt++;
        if (MaxSfxCnt <= sfxCnt)
            sfxCnt = 0;
    }
}