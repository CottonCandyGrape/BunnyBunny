using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCtrl : MonoBehaviour
{
    public ItemType itemType = ItemType.Gold;

    float goldVal = 0.0f;
    public float GoldVal
    {
        get { return goldVal; }
        set
        {
            if (itemType == ItemType.Gold)
                goldVal = value;
        }
    }
    bool magnet = false;
    public bool Magnet
    {
        get { return magnet; }
        set { magnet = value; }
    }
    float magnetSpeed = 9.0f;

    float bombRadius = 0.0f;

    void Start()
    {
        bombRadius = (ScreenMgr.InitScMax.x - ScreenMgr.InitScMin.x) / 2.0f;

        Destroy(gameObject, 10.0f);
    }

    void Update()
    {
        MagnetToPlayer();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            if (itemType == ItemType.Gold)
            {
                SoundMgr.Instance.PlaySfxSound("gold");
                GameMgr.Inst.AddGold(goldVal);
            }
            else if (itemType == ItemType.Heal)
            {
                SoundMgr.Instance.PlaySfxSound("cake");
                PlayerCtrl player = GameMgr.Inst.player.GetComponent<PlayerCtrl>();
                player.GetHp();
            }
            else if (itemType == ItemType.Bomb)
            {
                SoundMgr.Instance.PlaySfxSound("nuclear");
                Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, bombRadius);

                for (int i = 0; i < colls.Length; i++)
                {
                    if (colls[i].tag.Contains("Monster"))
                    {
                        MonsterCtrl monCtrl = colls[i].gameObject.GetComponent<MonsterCtrl>();
                        monCtrl.TakeDamage(GameMgr.Inst.player.MaxHp);
                    }
                }

                //섬광 효과. 코루틴 있어서 ItemMgr에 구현
                ItemMgr.Inst.FlashEffect(); 
            }

            Destroy(gameObject);
        }
    }

    void MagnetToPlayer()
    {
        if (!magnet) return;
        Vector3 player = GameMgr.Inst.player.transform.position;

        transform.position = Vector3.Lerp(transform.position, player, magnetSpeed * Time.deltaTime);
    }
}