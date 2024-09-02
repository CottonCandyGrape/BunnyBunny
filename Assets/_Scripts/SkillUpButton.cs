using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillBtnType
{
    Weapon,
    Skill,
    Item
}

public class SkillUpButton : MonoBehaviour
{
    public SkillBtnType BtnType = SkillBtnType.Weapon;

    public Weapon Weapon { set { weapon = value; } }
    public Skill Skill { set { skill = value; } }

    [Header("------ UI ------")]
    public Button SkillUp_Btn = null;
    public Image Weapon_Img = null;
    public Sprite Ev_Sprite = null;
    public Text Explain_Txt = null;
    public Text WpName_Txt = null;
    public Image[] FStars_Img = null;

    //star 관련
    float starSize = 40.0f;
    Vector2 curSize = Vector2.zero;
    WaitForSecondsRealtime starDelay = new WaitForSecondsRealtime(0.1f);
    //star 관련

    Weapon weapon = null;
    Skill skill = null;
    SkillUpPopUp skPopUp = null;

    void OnEnable()
    {
        SkillUp_Btn.interactable = true;

        if (weapon != null)
        {
            string key = weapon.GetExplainText();
            Explain_Txt.text = AllSceneMgr.Instance.langMgr.GetLangValue(key); //설명 텍스트 설정

            if (weapon.CurLv == 3) //진화 직전에 이미지 교체
            {
                Weapon_Img.sprite = Ev_Sprite;
                WpName_Txt.text = weapon.Ev_Name;
            }
        }

        if(skill != null)
        {
            string key = skill.GetExplainText();
            Explain_Txt.text = AllSceneMgr.Instance.langMgr.GetLangValue(key); //설명 텍스트 설정
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

        SkillUp_Btn.interactable = false;

        if (BtnType == SkillBtnType.Weapon)
        {
            if (weapon.CurLv < 3)
            {
                weapon.LevelUpWeapon();
                StartCoroutine(FillStar());
                return; //코루틴 후 사라져야하는 예외처리
            }
            else if (3 <= weapon.CurLv) //진화 직전
            {
                weapon.EvolveWeapon(); //진화 시키고

                if (skPopUp != null)
                    skPopUp.SuBtns.Remove(this); //SkillUpPopUp의 List에서 삭제.
                Destroy(gameObject); //프리팹 삭제
            }
        }
        else if (BtnType == SkillBtnType.Skill)
        { 
            if (skill.CurLv < 3)
            {
                skill.LevelUpSkill();
                StartCoroutine(FillStar());
                return; //코루틴 후 사라져야하는 예외처리
            }
            else if (3 <= skill.CurLv) // 레벨업 끝
            {
                if (skPopUp != null)
                    skPopUp.SuBtns.Remove(this); //SkillUpPopUp의 List에서 삭제.
                Destroy(gameObject); //프리팹 삭제
            }
        }
        else if (BtnType == SkillBtnType.Item)
        {
            if (gameObject.name.Contains("Cake"))
                GameMgr.Inst.player.GetHp();
            else if (gameObject.name.Contains("Gold"))
                GameMgr.Inst.AddGold(200);
        }

        Time.timeScale = 1.0f;
        skPopUp.gameObject.SetActive(false);
    }

    IEnumerator FillStar()
    {
        Image star = null;
        if (weapon != null)
            star = FStars_Img[weapon.CurLv - 1];
        else if (skill != null)
            star = FStars_Img[skill.CurLv - 1];

        star.gameObject.SetActive(true);
        star.rectTransform.sizeDelta = curSize;

        while (star.rectTransform.sizeDelta.x < starSize)
        {
            curSize += Vector2.Lerp(curSize, Vector2.one * starSize, Time.unscaledDeltaTime);
            star.rectTransform.sizeDelta = curSize;
            yield return null;
        }

        star.rectTransform.sizeDelta = Vector2.one * starSize;
        yield return starDelay;

        Time.timeScale = 1.0f;
        skPopUp.gameObject.SetActive(false);
    }
}