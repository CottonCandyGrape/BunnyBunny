using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Gold,
    Heal,
}

public class ItemCtrl : MonoBehaviour
{
    public ItemType itemType = ItemType.Gold;

    float goldVal = 0.0f;
    public float GoldVal
    {
        get
        {
            return goldVal;
        }
        set
        {
            if (itemType == ItemType.Gold)
                goldVal = value;
            else
                return;
        }
    }

    float healVal = 0.0f;
    public float HealVal
    {
        get
        {
            return HealVal;
        }
        set
        {
            if (itemType == ItemType.Heal)
                healVal = value;
            else
                return;
        }
    }

    void Start()
    {
        Destroy(gameObject, 10.0f); //TODO : 나중에는 안먹으면 안사라지게 할거임
    }

    //void Update() { }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            if (itemType == ItemType.Gold)
            {
                GameMgr.Inst.AddGold(goldVal);
                Destroy(gameObject);
            }
            else if (itemType == ItemType.Heal)
            {

            }
        }
    }
}