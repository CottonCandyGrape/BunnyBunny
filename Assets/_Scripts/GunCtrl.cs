using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCtrl : MonoBehaviour
{
    [HideInInspector] public static int level = 0;
    [HideInInspector] public static bool evolve = false;

    const float cstBulletDist = 0.3f;

    void Start() { }

    //void Update() { }

    public void FireBullet(Vector3 bltDir)
    {
        bltDir.Normalize();

        BulletCtrl bltCtrl = MemoryPoolMgr.Inst.AddBulletPool();
        bltCtrl.MoveDir = bltDir;
        bltCtrl.gameObject.SetActive(true);
        bltCtrl.transform.position = transform.position + bltDir * cstBulletDist;
        float angle = Mathf.Atan2(bltDir.y, bltDir.x) * Mathf.Rad2Deg;
        bltCtrl.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void FanFire(Vector3 midDir)
    {
        int deg = 20; //TODO : 하드코딩 바꾸기 
        for (int cnt = -2; cnt < 3; cnt++)
        {
            Vector3 dir = Quaternion.AngleAxis(cnt * deg, Vector3.forward) * midDir;
            FireBullet(dir);
        }
    }
}