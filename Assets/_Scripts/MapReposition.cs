using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType
{
    Ground,
    Vertical,
    FixedGround
}

public class MapReposition : MonoBehaviour
{
    int moveDist = 40;

    float dirX = 0.0f;
    float dirY = 0.0f;
    float distX = 0.0f;
    float distY = 0.0f;
    float distXY = 0.0f;

    void OnTriggerExit2D(Collider2D coll)
    {
        if (!coll.CompareTag("Area")) return;

        if (GameMgr.Inst.MType == MapType.Ground)
            GroundMap();
        else if (GameMgr.Inst.MType == MapType.Vertical)
            VerticalMap();
    }

    void GroundMap()
    {
        moveDist = 40;

        Vector3 playerPos = GameMgr.Inst.player.transform.position;
        Vector3 tilePos = this.transform.position;

        dirX = playerPos.x - tilePos.x;
        dirY = playerPos.y - tilePos.y;

        distX = Mathf.Abs(dirX);
        distY = Mathf.Abs(dirY);
        distXY = Mathf.Abs(distX - distY);

        dirX = dirX > 0 ? 1 : -1;
        dirY = dirY > 0 ? 1 : -1;

        if (distXY <= 0.1f)
        {
            transform.position += Vector3.up * dirY * moveDist;
            transform.position += Vector3.right * dirX * moveDist;
        }
        else if (distX > distY)
            transform.position += Vector3.right * dirX * moveDist;
        else if (distX < distY)
            transform.position += Vector3.up * dirY * moveDist;
    }

    void VerticalMap()
    {
        moveDist = 30;

        Vector3 playerPos = GameMgr.Inst.player.transform.position;
        Vector3 tilePos = this.transform.position;

        if (playerPos.y > tilePos.y)
            transform.position += Vector3.up * moveDist;
        else if (playerPos.y < tilePos.y)
            transform.position += Vector3.down * moveDist;
    }
}
