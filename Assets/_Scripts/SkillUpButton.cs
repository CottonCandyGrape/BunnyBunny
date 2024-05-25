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

    public Weapon Skill { set { weapon = value; } }

    public Button SkillUp_Btn = null;
    public Image Weapon_Img = null;
    public Sprite Ev_Sprite = null;
    public Text Explain_Txt = null;
    public Image[] FStars_Img = null;

    Weapon weapon = null;
    SkillUpPopUp skPopUp = null;

    void OnEnable()
    {
        if (weapon != null)
        {
            Explain_Txt.text = weapon.GetExplainText(); //설명 텍스트 설정

            if (weapon.CurLv == 3) //진화 직전에 이미지 교체
                Weapon_Img.sprite = Ev_Sprite;
        }
    }

    void Start()
    {
        if (SkillUp_Btn != null)
            SkillUp_Btn.onClick.AddListener(SkillUpBtnClick);

        skPopUp = GetComponentInParent<SkillUpPopUp>();
    }

    void OnDisable()
    {
        gameObject.SetActive(false); //SkillUpPopUp 닫힐때 다 꺼져야한다.
    }

    void SkillUpBtnClick()
    {
        SoundMgr.Instance.PlaySfxSound("skillSelect");

        if (BtnType == SkillBtnType.Skill)
        {
            if (weapon.CurLv < 3)
            {
                weapon.LevelUpWeapon();
                SetStar();
            }
            else if (3 <= weapon.CurLv) //진화 직전
                weapon.EvolveWeapon();

            if (weapon.IsEvolve) //진화 직후
            {
                if (skPopUp != null)
                    skPopUp.WpBtns.Remove(this); //SkillUpPopUp의 List에서 삭제.
                Destroy(gameObject); //프리팹 삭제
            }
        }
        else if (BtnType == SkillBtnType.Item)
        {
            if (gameObject.name.Contains("Carrot"))
                GameMgr.Inst.player.GetHp(0.3f);
            else if (gameObject.name.Contains("Gold"))
                GameMgr.Inst.AddGold(200);
        }

        Time.timeScale = 1.0f;
        skPopUp.gameObject.SetActive(false);
    }

    void SetStar()
    {
        int level = weapon.CurLv;
        FStars_Img[level - 1].gameObject.SetActive(true);
    }
}