using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public PlayerCtrl player = null;

    //게임 시간 관련 변수
    float curTime = 0.0f;
    float minTime = 60.0f;
    int min = 0;
    int sec = 0;
    float nextAddTime = 30.0f;
    float eliteTime = 90.0f;
    float endTime = 180.0f;
    //float endTime = float.MaxValue; //Test 용. 
    //게임 시간 관련 변수

    //인게임 정보
    //몬스터 킬수 표시
    int killCount = 0;
    //Gold 관련
    float inGameGold = 0.0f;
    //Exp 관련
    float inGameExp = 0.0f;
    float prevExp = 0.0f;
    float nextExp = 50.0f;
    float incRatio = 1.5f;
    int inGameLevel = 1;
    Coroutine expCo = null;
    //인게임 정보

    //Elite, Boss전 관련
    bool hasSpawnElite = false;
    public GameObject[] Rings = null;
    public GameObject[] ExpEffects = null;
    int expEffectCnt = 10;
    [HideInInspector] public GameObject BattleRing = null;
    [HideInInspector] public bool hasBoss = false;
    [HideInInspector] public bool hasRing = false;
    [HideInInspector] public bool stageClear = false;
    Coroutine bHpCo = null;
    //Elite, Boss전 관련

    //Class 변수
    MonGenerator monGen = null;
    public MonGenerator MonGen
    {
        get
        {
            if (monGen != null) return monGen;
            else return null;
        }
    }
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
        if (Input.GetKeyDown(KeyCode.Alpha1)) //zoom out Test 코드
        {
            InitBossBattle();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            monGen.SpawnEliteMon();
        }
    }

    void SetMap()
    {
        if (AllSceneMgr.Instance.CurStageNum == 0)
            MType = MapType.Ground;
        else if (AllSceneMgr.Instance.CurStageNum == 1)
            MType = MapType.Vertical;
        else if (AllSceneMgr.Instance.CurStageNum == 2)
            MType = MapType.FixedGround;

        Instantiate(Maps[(int)MType], Vector3.zero, Quaternion.identity);
    }

    void UpdateGameTime()
    {
        if (hasRing) return;

        curTime += Time.deltaTime;

        min = (int)(curTime / minTime);
        sec = (int)(curTime - min * minTime);

        Time_Txt.text = string.Format("{0:D2}:{1:D2}", min, sec);

        if (nextAddTime <= curTime && monGen.MonLimit < MonGenerator.MaxMonCnt)
        {
            monGen.MonLimit += 10;
            nextAddTime += 45.0f;
        }

        if (eliteTime <= curTime && !hasSpawnElite)
        {
            monGen.SpawnEliteMon();
            hasSpawnElite = true;
        }

        if (endTime <= curTime)
        {
            InitBossBattle();
        }
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
        inGameExp += eVal;
        if (nextExp <= inGameExp) //levelup 할 때.
        {
            inGameLevel++;
            prevExp = nextExp;
            nextExp *= incRatio;
            nextExp = (int)nextExp;

            ShowSkillPopUp();
        }

        ExpLevel_Txt.text = "Lv. " + inGameLevel.ToString();

        if (expCo != null) StopCoroutine(expCo);
        float target = (inGameExp - prevExp) / (nextExp - prevExp);
        expCo = StartCoroutine(FillBarImg(ExpBar_Img, target));
    }

    void ShowSkillPopUp()
    {
        if (player.IsDead || stageClear) return;

        SoundMgr.Instance.PlaySfxSound("skillUp");
        Time.timeScale = 0.0f;
        SkillUpPopUp.SetActive(true);
    }

    public void UpdateBossHpBar(float target)
    {
        if (bHpCo != null)
            StopCoroutine(bHpCo);
        bHpCo = StartCoroutine(FillBarImg(BossHpBar_Img, target));
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

    public IEnumerator ExploseEffect(Vector3 bossPos)
    {
        List<GameObject> expList = new List<GameObject>();
        for (int i = 0; i < expEffectCnt; i++)
        {
            SoundMgr.Instance.PlaySfxSound("rocket");

            GameObject expEff = Instantiate(ExpEffects[Random.Range(0, ExpEffects.Length)]);
            expEff.transform.position
                = bossPos + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0.0f);
            expList.Add(expEff);

            yield return new WaitForSeconds(0.1f);
        }

        while (expList.Count > 0)
        {
            for (int i = 0; i < expList.Count; i++)
            {
                if (!expList[i].activeSelf)
                    expList.RemoveAt(i);
            }

            yield return null;
        }
    }

    public void GameOver(bool isClear)
    {
        AllSceneMgr.Instance.adsMgr.CreateBannerView();

        SoundMgr.Instance.TurnOffSound();

        Time.timeScale = 0.0f;

        GameObject goObj = Instantiate(GameOverPopUp, MainCanvas.transform);
        PopUpBox popUp = goObj.GetComponent<PopUpBox>();
        popUp.SetGameOverText(isClear, inGameGold, killCount, inGameExp);
    }

    public void GoToBattleScene(bool save)
    {
        if (save)
            AllSceneMgr.Instance.GetInGameResult((int)inGameGold, inGameExp, stageClear);

        StartCoroutine(AllSceneMgr.Instance.LoadScene("Battle"));
    }
}