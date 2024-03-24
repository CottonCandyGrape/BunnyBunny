using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapReposition : MonoBehaviour
{
    int moveDist = 40;

    void OnTriggerExit2D(Collider2D coll)
    {
        if (!coll.CompareTag("Area"))
            return;

        Vector3 playerPos = GameMgr.Inst.player.transform.position;
        Vector3 tilePos = this.transform.position;

        float distX = Mathf.Abs(playerPos.x - tilePos.x);
        float distY = Mathf.Abs(playerPos.y - tilePos.y);

        float dirX = GameMgr.Inst.player.h < 0 ? -1 : 1;
        float dirY = GameMgr.Inst.player.v < 0 ? -1 : 1;

        if (distX > distY)
            transform.position += Vector3.right * dirX * moveDist;
        else if (distX < distY)
            transform.position += Vector3.up * dirY * moveDist;
    }
}
