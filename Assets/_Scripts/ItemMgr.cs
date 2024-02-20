using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Gold,
    Heal,
}

public class ItemMgr : MonoBehaviour
{
    public static ItemMgr Inst = null;

    public GameObject[] ItemPrefabs = null;

    float meatTimer = 0.0f;
    float meatTime = 10.0f;

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
        meatTimer -= Time.deltaTime;
        if (meatTimer <= 0.0f)
        {
            float x = Random.Range(ScreenMgr.CurScMin.x, ScreenMgr.CurScMax.x);
            float y = Random.Range(ScreenMgr.CurScMin.y, ScreenMgr.CurScMax.y);
            Vector2 pos = new Vector2(x, y);
            SpawnMeat(pos, 0.3f);

            meatTimer = meatTime;
        }
    }

    public void SpawnGold(Vector3 pos, MonsterType monType)
    {
        GameObject gold = Instantiate(ItemPrefabs[(int)ItemType.Gold]);
        gold.transform.position = pos;

        ItemCtrl item = gold.GetComponent<ItemCtrl>();
        item.GoldVal = 10;
        if (monType == MonsterType.EliteMon)
            item.GoldVal = 50;
        else if (monType == MonsterType.BossMon)
            item.GoldVal = 100;
    }


    void SpawnMeat(Vector3 pos, float healRate) //TODO : healRate 기준 정하기.
    {
        GameObject meat = Instantiate(ItemPrefabs[(int)ItemType.Heal]);
        meat.transform.position = pos;

        ItemCtrl item = meat.GetComponent<ItemCtrl>();
        item.HealRate = healRate; 
    }
}