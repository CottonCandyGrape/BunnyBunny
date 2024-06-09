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
    public Transform Cakes = null;
    public Transform Bombs = null;

    //섬광 관련 
    public SpriteRenderer FlashRender = null;
    Color flashColor = new Color(1, 1, 1, 0);
    float alphaSpeed = 5.0f;
    Coroutine flashCo = null;
    //섬광 관련 

    public GameObject[] ItemPrefabs = null;

    float cakeTimer = 0.0f;
    float cakeTime = 10.0f;

    public static ItemMgr Inst = null;

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        cakeTimer = cakeTime;
    }

    void Update()
    {
        UpdateCakeTimer();
    }

    void UpdateCakeTimer()
    {
        cakeTimer -= Time.deltaTime;
        if (cakeTimer <= 0.0f)
        {
            Spawncake(0.3f);
            cakeTimer = cakeTime;
        }
    }

    void Spawncake(float healRate)
    {
        GameObject crt = Instantiate(ItemPrefabs[(int)ItemType.Heal], Cakes);
        crt.transform.position = ScreenMgr.Inst.GetRandomPosCurScreen();

        ItemCtrl item = crt.GetComponent<ItemCtrl>();
        item.HealRate = healRate;
    }

    public void SpawnBomb(Vector3 pos)
    {
        GameObject bomb = Instantiate(ItemPrefabs[(int)ItemType.Bomb], Bombs);
        bomb.transform.position = pos;
    }

    public void SpawnGold(Vector3 pos, int goldVal)
    {
        GameObject gold = Instantiate(ItemPrefabs[(int)ItemType.Gold], Golds);
        gold.transform.position = pos;

        ItemCtrl item = gold.GetComponent<ItemCtrl>();
        item.GoldVal = goldVal;
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