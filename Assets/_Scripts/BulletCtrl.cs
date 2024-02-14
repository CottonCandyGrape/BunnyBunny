using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    Vector3 bulletDir = Vector3.one;

    float bulletSpeed = 10.0f;
    float lifeTime = 0.0f;
    float outLine = 3.0f;

    void OnEnable()
    {
        bulletDir = GameMgr.inst.player.bulletDir.normalized;
        lifeTime = 5.0f;
    }

    void Start()
    {

    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0.0f)
        {
            gameObject.SetActive(false);
            return;
        }

        transform.position += bulletDir * bulletSpeed * Time.deltaTime;

        if (ScreenMgr.CurScMax.x + outLine < transform.position.x ||
            transform.position.x < ScreenMgr.CurScMin.x - outLine ||
            ScreenMgr.CurScMax.y + outLine < transform.position.y ||
            transform.position.y < ScreenMgr.CurScMin.y - outLine)
        {
            gameObject.SetActive(false);
        }
    }
}