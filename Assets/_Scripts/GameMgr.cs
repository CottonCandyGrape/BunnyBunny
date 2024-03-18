using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    //TODO : UI 변수 따로 빼서 헤더 선언할까?
    public PlayerCtrl player = null;

    //게임 시간 관련 변수
    public Text Time_Txt = null;
    float curTime = 0.0f;
    float minTime = 60.0f;
    int min = 0;
    int sec = 0;
    //float endTime = 90.0f;
    float endTime = float.MaxValue; //Test 용. //TODO : 적정 시간 찾기
    //게임 시간 관련 변수

    //몬스터 킬수 표시
    public Text Kill_Txt = null;
    int killCount = 0;
    //몬스터 킬수 표시

    //Gold 관련
    public Text Gold_Txt = null;
    float inGameGold = 0.0f;
    //Gold 관련

    //Exp 관련
    //public Text CurExpLevel_Txt = null; //inGameExp test 용
    public Text ExpLevel_Txt = null;
    public Image ExpBar_Img = null;
    float inGameExp = 0.0f;
    float[] expLevelArr = { 0.0f, 30.0f, 60.0f, 100.0f, 150.0f, 210.0f, 280.0f, 360.0f, 450.0f, 550.0f }; //TODO : 만랩 늘리면 수식으로 바꾸기
    int inGameLevel = 1;
    int maxLevel = 10; // 현재 만랩 10 //TODO : 만랩 늘리기. 
    Coroutine expCo = null;
    //Exp 관련

    //데미지 표시
    public Canvas SubCanvas = null;
    public GameObject DmgTxtPrefab = null;
    //데미지 표시

    //현재 인게임 관련
    //static으로 할까? Scene 시작할때 바로 초기화 돼서 MemoryPoolMgr로 안전하게 넘겨야 하는데..
    //static 이면 awake에서 해도되나?
    [HideInInspector] public int StageNum = 0;
    //현재 인게임 관련

    //Boss전 관련
    public GameObject BossRing = null;
    //Boss전 관련

    public static GameMgr Inst = null;

    MonGenerator mongen = null; //Elite Mon Spawn Test 코드
    CameraCtrl camctrl = null; //zoom out test 코드

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        curTime = 0.0f;

        Time.timeScale = 1.0f;

        mongen = FindObjectOfType<MonGenerator>(); //Elite Mon Spawn Test 코드
        camctrl = FindObjectOfType<CameraCtrl>(); //zoom out test 코드
    }

    void Update()
    {
        UpdateGameTime();

        //if(Input.GetKeyDown(KeyCode.Space)) //Elite Mon Spawn Test 코드
        //{
        //    mongen.SpawnEliteMon();
        //}
        if(Input.GetKeyDown(KeyCode.Space)) //zoom out test 코드
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
            float end = (inGameExp - expLevelArr[inGameLevel - 1]) /
                (expLevelArr[inGameLevel] - expLevelArr[inGameLevel - 1]);

            expCo = StartCoroutine(ExpBarFill(end));
        }
    }

    IEnumerator ExpBarFill(float end)
    {
        float expTimer = 0.0f;
        float expTime = 1.0f;
        float speed = 5.0f;

        while (expTimer <= 1.0f)
        {
            expTimer += speed * Time.deltaTime;
            ExpBar_Img.fillAmount = Mathf.Lerp(ExpBar_Img.fillAmount, end, (expTimer / expTime));
            yield return null;
        }
    }

    void InitBossBattle()
    {
        Vector2 spawnPos = ScreenMgr.Inst.GetCenterCurScreen();

        camctrl.ZoomOut();

        if (BossRing != null) //링 스폰. TODO : 이때부터 범위 제한 해야한다. 데미지는 아직 no 
        {
            GameObject ring = Instantiate(BossRing);
            ring.transform.position = spawnPos;
        }

        mongen.SpawnBossMon(spawnPos);  //몬스터 스폰. TODO : 몇 초 있다가 스폰 시키기
    }

    void GameOver()
    {
        Time.timeScale = 0.0f;
    }
}