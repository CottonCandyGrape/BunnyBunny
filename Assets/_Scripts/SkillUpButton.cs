using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillBtnType
{
    Skill,
    Item
}

public class SkillUpButton : MonoBehaviour
{
    public SkillBtnType BtnType = SkillBtnType.Skill;

    public Button SkillUp_Btn = null;
    public Image[] FStars_Img = null;
    Weapon weapon = null;
    public Weapon Skill { set { weapon = value; } }

    void OnEnable()
    {
        //if (weapon.CurLv == 3)
        //{

        //}
    }

    void Start()
    {
        if (SkillUp_Btn != null)
            SkillUp_Btn.onClick.AddListener(SkillUpBtnClick);
    }

    void SkillUpBtnClick()
    {
        GameMgr.Inst.LevelUp();
        weapon.LevelUpWeapon();
        if (BtnType == SkillBtnType.Skill)
            SetStar();

        if (!weapon.IsEvolve)
            gameObject.SetActive(false);
        //else
        //    Destroy(gameObject); //SkillUpPopUp에 List에도 알려주고 지워야함.
    }

    void SetStar()
    {
        int level = weapon.CurLv;
        Debug.Log(level);
        FStars_Img[level - 1].gameObject.SetActive(true);
    }
}