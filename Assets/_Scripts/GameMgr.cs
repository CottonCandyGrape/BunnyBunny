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
    float[] expLevelArr = { 0.0f, 30.0f, 60.0f, 100.0f, 150.0f, 210.0f, 280.0f, 360.0f, 450.0f, 550.0f }; //TODO : 만랩 늘리면 수식으로 바꾸기
    int inGameLevel = 1;
    int maxLevel = 10; // 현재 만랩 10 //TODO : 만랩 늘리기. 
    Coroutine expCo = null;
    //Exp 관련

    //데미지 표시
    public GameObject DmgTxtPrefab = null;
    //데미지 표시

    //현재 인게임 관련
    //static으로 할까? Scene 시작할때 바로 초기화 돼서 MemoryPoolMgr로 안전하게 넘겨야 하는데..
    //static 이면 awake에서 해도되나?
    [HideInInspector] public int StageNum = 0;
    //현재 인게임 관련

    //Boss전 관련
    public GameObject BattleRing = null;
    [HideInInspector] public bool hasBoss = false;
    //Boss전 관련

    //Class 변수
    MonGenerator monGen = null; //Elite Mon Spawn Test 코드 //TODO : 이 두 변수 어케 처리 할지?
    CameraCtrl camCtrl = null; //zoom out test 코드
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
    //UI 변수

    public static GameMgr Inst = null;

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        curTime = 0.0f;

        Time.timeScale = 1.0f;

        monGen = FindObjectOfType<MonGenerator>(); //Elite Mon Spawn Test 코드
        camCtrl = FindObjectOfType<CameraCtrl>(); //zoom out test 코드
    }

    void Update()
    {
        UpdateGameTime();

        //if(Input.GetKeyDown(KeyCode.Space)) //Elite Mon Spawn Test 코드
        //{
        //    mongen.SpawnEliteMon();
        //}
        if (Input.GetKeyDown(KeyCode.Space)) //zoom out test 코드
        {
            InitBossBattle();
        }
    }

    void UpdateGameTime()
    {
        curTime += Time.deltaTime;

        min = (int)(curTime / minTime);
        sec = (int)(curTime - min * minTime);

        Time_Txt.text = string.Format("{0:D2}:{1:D2}", min, sec);

        if (endTime <= curTime)
        {
            GameOver();
            return;
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
        for (int i = 0; i < expLevelArr.Length; i++)
        {
            if (expLevelArr[i] <= inGameExp)
                inGameLevel = i + 1;
            else
                break;
        }

        //CurExpLevel_Txt.text = inGameExp.ToString(); //inGameExp Test용
        ExpLevel_Txt.text = inGameLevel.ToString();

        if (inGameLevel >= maxLevel)
            ExpBar_Img.fillAmount = 1;
        else
        {
            if (expCo != null)
                StopCoroutine(expCo);
            float target = (inGameExp - expLevelArr[inGameLevel - 1]) /
                (expLevelArr[inGameLevel] - expLevelArr[inGameLevel - 1]);

            expCo = StartCoroutine(FillBarImg(ExpBar_Img, target));
        }
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
        if (hasBoss) return; //Boss 스폰 되어있다면 return

        hasBoss = true;

        camCtrl.ZoomOut(); //카메라 올리기
        MemoryPoolMgr.Inst.OffAllNorMon(); //Normal Monster 다 끄기

        Vector2 spawnPos = ScreenMgr.Inst.GetCenterCurScreen();
        if (BattleRing != null) //링 스폰. TODO : 링에 닿으면 데미지 
        {
            GameObject ring = Instantiate(BattleRing);
            ring.transform.position = spawnPos;
        }
        player.TrapBossRing(); //player 링에 가두기

        monGen.SpawnBossMon(spawnPos);  //Boss Monster 스폰. TODO : 몇 초 있다가 스폰 시키기

        GameObject bossHpBar = BossHpBar_Img.transform.parent.gameObject;
        bossHpBar.SetActive(true); //bossHpBar 켜기. //Expbar는 뒤에 묻혀서 안껏음.
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
    }
}