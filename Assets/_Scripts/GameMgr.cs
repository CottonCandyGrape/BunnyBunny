using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public PlayerCtrl player = null;

    //게임 시간 관련 변수
    float curTime = 0.0f;
    float minTime = 60.0f;
    int min = 0;
    int sec = 0;
    //float endTime = 180.0f;
    float endTime = float.MaxValue; //Test 용. 
    //게임 시간 관련 변수

    //몬스터 킬수 표시
    int killCount = 0;
    //몬스터 킬수 표시

    //Gold 관련
    float inGameGold = 0.0f;
    //Gold 관련

    //Exp 관련
    //public Text CurExpLevel_Txt = null; //inGameExp test 용
    float inGameExp = 0.0f;
    float prevExp = 0.0f;
    float nextExp = 50.0f;
    float incRatio = 1.5f;
    int inGameLevel = 1;
    Coroutine expCo = null;
    //Exp 관련

    //Boss전 관련
    public GameObject[] Rings = null;
    [HideInInspector] public GameObject BattleRing = null;
    [HideInInspector] public bool hasBoss = false;
    [HideInInspector] public bool hasRing = false;
    [HideInInspector] public bool stageClear = false;
    Coroutine bHpCo = null;
    //Boss전 관련

    //Class 변수
    MonGenerator monGen = null;
    CameraCtrl camCtrl = null;
    //Class 변수

    //UI 변수
    [Header("------ UI ------")]
    public Text Time_Txt = null;
    public Text Kill_Txt = null;
    public Text Gold_Txt = null;
    public Text ExpLevel_Txt = null;
    public Image ExpBar_Img = null;
    public Image BossHpBar_Img = null;
    public Canvas SubCanvas = null;
    public Canvas MainCanvas = null;
    public Button Pause_Btn = null;
    public GameObject PausePopUp = null;
    public GameObject SkillUpPopUp = null;
    public GameObject GameOverPopUp = null;

    public GameObject DmgTxtPrefab = null; //데미지 표시 UI
    //UI 변수

    //Map 관련
    [HideInInspector] public MapType MType = MapType.Ground;
    public GameObject[] Maps = null;
    //Map 관련

    public static GameMgr Inst = null;

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        curTime = 0.0f;
        Time.timeScale = 1.0f;
        stageClear = false;

        monGen = FindObjectOfType<MonGenerator>();
        camCtrl = FindObjectOfType<CameraCtrl>();

        if (Pause_Btn)
            Pause_Btn.onClick.AddListener(PauseBtnClick);

        SetMap();

        AllSceneMgr.Instance.LoadingAnim_Canvas.SetActive(false); //로딩 화면 끄기
        SoundMgr.Instance.PlayBGM("stage_0" + (AllSceneMgr.Instance.CurStageNum + 1).ToString()); //Stage Bgm Play
    }

    void Update()
    {
        UpdateGameTime();

        //if(Input.GetKeyDown(KeyCode.Space)) //Elite Mon Spawn Test 코드
        //{
        //    mongen.SpawnEliteMon();
        //}
        if (Input.GetKeyDown(KeyCode.Space)) //zoom out Test 코드
        {
            InitBossBattle();
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ShowSkillPopUp();
        //}
    }

    void SetMap()
    {
        if (AllSceneMgr.Instance.CurStageNum == 0)
            MType = MapType.Ground;
        else if (AllSceneMgr.Instance.CurStageNum == 1)
            MType = MapType.Vertical;

        Instantiate(Maps[(int)MType], Vector3.zero, Quaternion.identity);
    }

    void UpdateGameTime()
    {
        curTime += Time.deltaTime;

        min = (int)(curTime / minTime);
        sec = (int)(curTime - min * minTime);

        Time_Txt.text = string.Format("{0:D2}:{1:D2}", min, sec);

        //if (endTime <= curTime) //TODO : 보스 나타나게 하기
        //{
        //    GameOver();
        //    return;
        //}
    }

    public void AddGold(float val)
    {
        inGameGold += val;
        Gold_Txt.text = inGameGold.ToString();
    }

    public void KillTxtUpdate()
    {
        killCount++;
        if (Kill_Txt != null)
            Kill_Txt.text = killCount.ToString();
    }

    public void SpawnDmgTxt(Vector3 pos, float damage, Color txt_color)
    {
        GameObject dmgObj = Instantiate(DmgTxtPrefab, SubCanvas.transform);
        DmgTxtCtrl dmgTxtCtrl = dmgObj.GetComponent<DmgTxtCtrl>();
        dmgTxtCtrl.Init(pos, damage, txt_color);
    }

    public void AddExpVal(float eVal)
    {
        //Debug.Log(string.Format("Current Exp:{0}, Prev {1}, Next {2}", inGameExp, prevExp, nextExp));
        inGameExp += eVal;
        if (nextExp <= inGameExp) //levelup 할 때.
        {
            inGameLevel++;
            prevExp = nextExp;
            nextExp *= incRatio;
            nextExp = (int)nextExp;

            ShowSkillPopUp();
        }

        //CurExpLevel_Txt.text = inGameExp.ToString(); //inGameExp Test용
        ExpLevel_Txt.text = "Lv. " + inGameLevel.ToString();

        if (expCo != null) StopCoroutine(expCo);
        float target = (inGameExp - prevExp) / (nextExp - prevExp);
        expCo = StartCoroutine(FillBarImg(ExpBar_Img, target));
    }

    void ShowSkillPopUp()
    {
        SoundMgr.Instance.PlaySfxSound("skillUp");
        Time.timeScale = 0.0f;
        SkillUpPopUp.SetActive(true);
    }

    public void UpdateBossHpBar(float target)
    {
        if (bHpCo != null)
            StopCoroutine(bHpCo);
        bHpCo = StartCoroutine(FillBarImg(BossHpBar_Img, target)); //TODO : BossPtr 아직 null
    }

    //게이지 방향이 음, 양일 수 있어서 Lerp로 구현함.
    IEnumerator FillBarImg(Image fImg, float target)
    {
        float fTimer = 0.0f;
        float fTime = 1.0f;
        float speed = 5.0f;

        while (fTimer <= 1.0f)
        {
            fTimer += speed * Time.deltaTime;
            fImg.fillAmount = Mathf.Lerp(fImg.fillAmount, target, (fTimer / fTime));
            yield return null;
        }
    }

    void InitBossBattle()
    {
        if (hasRing) return; //BattleRing 스폰 되어있다면 return
        hasRing = true;

        camCtrl.ZoomOut(); //카메라 올리기
        MemoryPoolMgr.Inst.DestroyAllNorMon(); //Normal Monster 다 끄기

        Vector2 spawnPos = ScreenMgr.Inst.GetCenterCurScreen();
        if (BattleRing != null) //링 스폰. 
        {
            BattleRing = Instantiate(Rings[(int)MType]);
            BattleRing.transform.position = spawnPos;
        }
        player.TrapBossRing(true); //player 링에 가두기

        monGen.SpawnBossMon(spawnPos); //Boss Monster 스폰.

        GameObject bossHpBar = BossHpBar_Img.transform.parent.gameObject;
        bossHpBar.SetActive(true); //bossHpBar 켜기. 
        ExpLevel_Txt.gameObject.SetActive(false); //Lv 끄기
        ExpBar_Img.gameObject.SetActive(false); //Exp Bar 끄기
    }

    void PauseBtnClick()
    {
        Time.timeScale = 0.0f;
        Instantiate(PausePopUp, MainCanvas.transform);
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f; //얘 때문에 BattleSceneMgr에서 토끼 배경 움직여 주려고 다시 1.0으로 맞춰준다.

        GameObject goObj = Instantiate(GameOverPopUp, MainCanvas.transform);
        PopUpBox popUp = goObj.GetComponent<PopUpBox>();
        popUp.SetGameOverText(inGameGold, killCount, inGameExp);
    }

    public void GoToBattleScene(bool save)
    {
        if (save)
            AllSceneMgr.Instance.GetInGameResult((int)inGameGold, inGameExp, stageClear);

        StartCoroutine(AllSceneMgr.Instance.LoadScene("Battle"));
    }
}