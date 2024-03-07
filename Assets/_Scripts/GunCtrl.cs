using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCtrl : Weapon
{
    const float BulletOffset = 0.3f;

    void Start() { }

    //void Update() { }

    public void FireBullet(Vector3 bltDir) //방향, 위치, 각도 정하여 발사
    {
        bltDir.Normalize();

        BulletCtrl bltCtrl = MemoryPoolMgr.Inst.AddBulletPool();
        bltCtrl.MoveDir = bltDir;
        bltCtrl.gameObject.SetActive(true);
        bltCtrl.transform.position = transform.position + bltDir * BulletOffset;
        float angle = Mathf.Atan2(bltDir.y, bltDir.x) * Mathf.Rad2Deg;
        bltCtrl.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void FanFire(Vector3 midDir) //부채꼴 모양 발사 스킬
    {
        int deg = 20; //TODO : 하드코딩 바꾸기 
        for (int cnt = -2; cnt < 3; cnt++)
        {
            Vector3 dir = Quaternion.AngleAxis(cnt * deg, Vector3.forward) * midDir;
            FireBullet(dir);
        }
    }

    public override void LevelUpWeapon()
    {

    }

    public override void EvolveWeapon()
    {

    }
}