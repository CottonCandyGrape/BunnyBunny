using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Gold,
    Heal,
    Bomb,
}

public class ItemMgr : MonoBehaviour
{
    public Transform Golds = null;
    public Transform Carrots = null;
    public Transform Bombs = null;

    //섬광 관련 
    public SpriteRenderer FlashRender = null;
    Color flashColor = new Color(1, 1, 1, 0);
    float alphaSpeed = 5.0f;
    Coroutine flashCo = null;
    //섬광 관련 

    public GameObject[] ItemPrefabs = null;

    float carrotTimer = 0.0f;
    float carrotTime = 10.0f;

    public static ItemMgr Inst = null;

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        carrotTimer = carrotTime;
    }

    void Update()
    {
        UpdateCarrotTimer();
    }

    void UpdateCarrotTimer()
    {
        carrotTimer -= Time.deltaTime;
        if (carrotTimer <= 0.0f)
        {
            SpawnCarrot(0.3f);
            carrotTimer = carrotTime;
        }
    }

    void SpawnCarrot(float healRate) //TODO : healRate 기준 정하기.
    {
        GameObject crt = Instantiate(ItemPrefabs[(int)ItemType.Heal], Carrots);
        crt.transform.position = ScreenMgr.Inst.GetRandomPosInCurScreen(); 

        ItemCtrl item = crt.GetComponent<ItemCtrl>();
        item.HealRate = healRate;
    }

    void SpawnBomb(Vector3 pos) //TODO : 호출위치 정하기. 매개변수 pos 필요 없을 수도.
    {
        GameObject bomb = Instantiate(ItemPrefabs[(int)ItemType.Bomb], Bombs);
        bomb.transform.position = pos;
    }

    public void SpawnGold(Vector3 pos, MonsterType monType)
    {
        GameObject gold = Instantiate(ItemPrefabs[(int)ItemType.Gold], Golds);
        gold.transform.position = pos;

        ItemCtrl item = gold.GetComponent<ItemCtrl>();
        item.GoldVal = 10;
        if (monType == MonsterType.EliteMon)
            item.GoldVal = 50;
        else if (monType == MonsterType.BossMon)
            item.GoldVal = 100;
    }

    public void FlashEffect()
    {
        FlashRender.transform.position = GameMgr.Inst.player.transform.position; //섬광 좌표
        if (flashCo != null)
            StopCoroutine(flashCo);
        flashCo = StartCoroutine(FlashEffectCo());
    }

    IEnumerator FlashEffectCo()
    {
        while (flashColor.a < 1.0f) // 섬광 터지기
        {
            flashColor.a += alphaSpeed * Time.deltaTime;
            FlashRender.color = flashColor;
            yield return null;
        }

        while (0.0f < flashColor.a) // 잦아들기
        {
            flashColor.a -= alphaSpeed * Time.deltaTime;
            FlashRender.color = flashColor;
            yield return null;
        }
    }
}