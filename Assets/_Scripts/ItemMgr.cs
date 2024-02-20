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

    void Awake()
    {
        Inst = this;
    }

    void Start() { }

    //void Update() { }

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


    public void SpawnMeat(Vector4 pos, float healRate) //TODO : healRate 기준 정하기.
    {
        GameObject meat = Instantiate(ItemPrefabs[(int)ItemType.Heal]);
        meat.transform.position = pos;

        ItemCtrl item = meat.GetComponent<ItemCtrl>();
        item.HealRate = healRate; 
    }
}