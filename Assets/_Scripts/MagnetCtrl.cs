using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetCtrl : Skill
{
    Vector3 playerPos = Vector3.zero;

    float radius = 0.0f;
    float incRange = 0.5f;

    void Start() { }

    public void Magneting()
    {
        if (curLevel < 1) return;

        playerPos = GameMgr.Inst.player.transform.position;

        Collider2D[] colls = Physics2D.OverlapCircleAll(playerPos, radius);
        for (int i = 0; i < colls.Length; i++)
        {
            int layer = colls[i].gameObject.layer;
            string layerName = LayerMask.LayerToName(layer);

            if (layerName == "Item")
            {
                ItemCtrl item = colls[i].GetComponent<ItemCtrl>();
                if (!item.Magnet) item.Magnet = true;
            }
        }
    }

    public override void LevelUpSkill()
    {
        curLevel++;
        radius = curLevel * incRange;
    }

    public override string GetExplainText()
    {
        if (curLevel == 0)
            return "magnetLv0";
        else if (curLevel == 1 || curLevel == 2)
            return "magnetLv1";
        else if (curLevel == 3)
            return "magnetLv3";
        return string.Empty;
    }
}
