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
    public Transform Meats = null;
    public Transform Bombs = null;

    //섬광 관련 
    public SpriteRenderer FlashRender = null;
    Color flashColor = new Color(1, 1, 1, 0);
    float alphaSpeed = 5.0f;
    Coroutine flashCo = null;
    //섬광 관련 

    public GameObject[] ItemPrefabs = null;

    float meatTimer = 0.0f;
    float meatTime = 10.0f;

    public static ItemMgr Inst = null;

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        meatTimer = meatTime;
    }

    void Update()
    {
        UpdateMeatTimer();
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

    void UpdateMeatTimer()
    {
        meatTimer -= Time.deltaTime;
        if (meatTimer <= 0.0f)
        {
            SpawnMeat(0.3f);
            meatTimer = meatTime;
        }
    }

    void SpawnMeat(float healRate) //TODO : healRate 기준 정하기.
    {
        GameObject meat = Instantiate(ItemPrefabs[(int)ItemType.Heal], Meats);
        meat.transform.position = ScreenMgr.Inst.GetRandomPosInCurScreen(); 

        ItemCtrl item = meat.GetComponent<ItemCtrl>();
        item.HealRate = healRate;
    }

    void SpawnBomb(Vector3 pos) //TODO : 호출위치 정하기. 매개변수 pos 필요 없을 수도.
    {
        GameObject bomb = Instantiate(ItemPrefabs[(int)ItemType.Bomb], Bombs);
        bomb.transform.position = pos;
    }

    public void ExplosionBomb(float radius) 
    {
        Vector2 playerPos = GameMgr.Inst.player.transform.position;
        Collider2D[] colls = Physics2D.OverlapCircleAll(playerPos, radius);

        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].CompareTag("Monster"))
            {
                MonsterCtrl monCtrl = colls[i].gameObject.GetComponent<MonsterCtrl>();
                monCtrl.TakeDamage(1000); //TODO : Bomb 데미지 정하기
            }
        }

        //섬광 효과
        FlashRender.transform.position = playerPos; //섬광 좌표
        if (flashCo != null)
            StopCoroutine(flashCo);
        flashCo = StartCoroutine(FlashEffect());
    }

    IEnumerator FlashEffect()
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