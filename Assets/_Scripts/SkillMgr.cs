using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMgr : MonoBehaviour
{
    //자석 스킬
    [Header("------ Magnet ------")]
    public GameObject Magnet = null;
    MagnetCtrl magnetCtrlSc = null;
    public MagnetCtrl MagentCtrlSc { get { return magnetCtrlSc; } }

    public static SkillMgr Inst = null;

    void Awake()
    {
        Inst = this;

        SetSkillScripts();
    }

    void SetSkillScripts()
    {
        magnetCtrlSc = Magnet.GetComponent<MagnetCtrl>(); 
    }
}
