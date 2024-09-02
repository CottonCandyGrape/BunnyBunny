using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpPopUp : MonoBehaviour
{
    WeaponMgr wpMgr = null;

    public List<GameObject> WpList = new List<GameObject>(); //G, R, D, M 순서
    public List<GameObject> SkList = new List<GameObject>(); //Ma, ... 순서
    public List<GameObject> Items = new List<GameObject>();

    List<SkillUpButton> suBtns = new List<SkillUpButton>();
    public List<SkillUpButton> SuBtns { get { return suBtns; } }
    List<SkillUpButton> itBtns = new List<SkillUpButton>();

    Weapon[] wpArray;
    Skill[] skArray;

    List<int> order = new List<int>();
    Vector2[] BtnPos =
    {
        new Vector2(-330.0f, -55.0f),
        new Vector2(0.0f, -55.0f),
        new Vector2(330.0f, -55.0f),
        new Vector2(-200.0f, -55.0f),
        new Vector2(200.0f, -55.0f)
    };

    void OnEnable()
    {
        //최초 시작에는 wpBtns.count==0 이기 때문에 에러남.
        //첫 Start 이후에 실행되어야 함.
        if (wpMgr == null) return;

        if (suBtns.Count >= 3)
        {
            order = ShuffleNum(suBtns.Count, 3);
            for (int i = 0; i < 3; i++)
            {
                suBtns[order[i]].transform.localPosition = BtnPos[i];
                suBtns[order[i]].gameObject.SetActive(true);
            }
        }
        else if (suBtns.Count == 2)
        {
            order = ShuffleNum(3, 3);
            List<SkillUpButton> tmp = suBtns.ToList(); //깊은 복사
            tmp.Add(itBtns[Random.Range(0, itBtns.Count)]);

            for (int i = 0; i < 3; i++)
            {
                tmp[order[i]].transform.localPosition = BtnPos[i];
                tmp[order[i]].gameObject.SetActive(true);
            }
        }
        else if (suBtns.Count == 1)
        {
            order = ShuffleNum(3, 3);
            List<SkillUpButton> tmp = itBtns.ToList(); //깊은 복사
            tmp.Add(suBtns[0]);

            for (int i = 0; i < 3; i++)
            {
                tmp[order[i]].transform.localPosition = BtnPos[i];
                tmp[order[i]].gameObject.SetActive(true);
            }
        }
        else if (suBtns.Count == 0)
        {
            order = ShuffleNum(2, 2);

            for (int i = 0; i < 2; i++)
            {
                itBtns[order[i]].transform.localPosition = BtnPos[i + 3];
                itBtns[order[i]].gameObject.SetActive(true);
            }
        }
    }

    void Start()
    {
        wpMgr = FindObjectOfType<WeaponMgr>();

        //Weapon Script 초기화. 순서 중요
        wpArray = new Weapon[]
        {
            wpMgr.GuardiansCtrlSc,
            wpMgr.RocketCtrlSc,
            wpMgr.DrillCtrlSc,
            wpMgr.MainWp
        };

        skArray = new Skill[]
        {
            SkillMgr.Inst.MagentCtrlSc
        };

        WpList.Add(wpMgr.MWSkillUpBtns[(int)wpMgr.MainType]); //메인 무기까지 추가

        //무기 Btn Pref List 초기화
        for (int i = 0; i < WpList.Count; i++)
        {
            GameObject weapon = Instantiate(WpList[i], transform);
            SkillUpButton skBtn = weapon.GetComponent<SkillUpButton>();
            skBtn.Weapon = wpArray[i];
            if (skBtn != null) suBtns.Add(skBtn);
            skBtn.gameObject.SetActive(false);
        }

        //패시브 스킬 Btn Pref List 초기화
        for (int i = 0; i < SkList.Count; i++)
        {
            GameObject skill = Instantiate(SkList[i], transform);
            SkillUpButton skBtn = skill.GetComponent<SkillUpButton>();
            skBtn.Skill = skArray[i];
            if (skBtn != null) suBtns.Add(skBtn);
            skBtn.gameObject.SetActive(false);
        }

        //아이템 Btn Pref List 초기화
        for (int i = 0; i < Items.Count; i++)
        {
            GameObject item = Instantiate(Items[i], transform);
            SkillUpButton skBtn = item.GetComponent<SkillUpButton>();
            if (skBtn != null) itBtns.Add(skBtn);
            skBtn.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    List<int> ShuffleNum(int n, int r)
    {
        if (n < r) return new List<int>();

        List<int> result = new List<int>();

        List<int> tmp = new List<int>();
        for (int i = 0; i < n; i++) tmp.Add(i);

        for (int i = 0; i < r; i++)
        {
            int idx = Random.Range(0, tmp.Count);
            result.Add(tmp[idx]);
            tmp.RemoveAt(idx);
        }

        return result;
    }
}