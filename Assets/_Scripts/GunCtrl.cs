using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCtrl : Weapon
{
    const float BulletOffset = 0.3f;
    const float ShotOffset = 0.25f;
    const int BaseShotCnt = 3;
    int curCount = 0;

    void Start() { }

    //void Update() { }

    public void FireBullet(Vector3 bltDir) //방향, 위치, 각도 정하여 발사
    {
        if (BaseShotCnt < curCount)
        {
            curCount = 0;
            return;
        }

        int count = GetBulletCount();

        for (int i = 0; i < count; i++)
        {
            BulletCtrl bltCtrl = MemoryPoolMgr.Inst.AddBulletPool();
            bltCtrl.gameObject.SetActive(true);

            bltDir.Normalize();
            bltCtrl.MoveDir = bltDir;

            float angle = Mathf.Atan2(bltDir.y, bltDir.x) * Mathf.Rad2Deg;
            bltCtrl.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            bltCtrl.transform.position = GetFirePos(i, bltDir) + bltDir * BulletOffset;
        }

        curCount++;
    }

    int GetBulletCount()
    {
        int count = 0;
        if (curCount < BaseShotCnt)
            count = curLevel + 1;
        else if (curCount == BaseShotCnt)
            count = curLevel + 3;

        return count;
    }

    Vector3 GetFirePos(int idx, Vector3 dir)
    {
        Vector3 firePos = GameMgr.Inst.player.transform.position;
        Vector3 rDir = Quaternion.AngleAxis(90, Vector3.forward) * dir; //dir을 왼쪽으로 90도 돌려야함

        if (curCount < BaseShotCnt)
            firePos += rDir * (ShotOffset / 2.0f) * curLevel;
        else if (BaseShotCnt == curCount)
            firePos += rDir * (ShotOffset / 2.0f) * (curLevel + 2);

        firePos -= rDir * ShotOffset * idx;

        return firePos;
    }

    public void FanFire(Vector3 midDir) //부채꼴 모양 발사 스킬
    {
        int deg = 20;
        int start = -2;
        int end = 3;
        for (int cnt = start; cnt < end; cnt++)
        {
            Vector3 dir = Quaternion.AngleAxis(cnt * deg, Vector3.forward) * midDir;
            FireBullet(dir);
        }
    }

    public override void LevelUpWeapon()
    {
        if (MaxLevel <= curLevel)
        {
            if (!isEvolve) EvolveWeapon();
            return;
        }

        curLevel++;
    }

    public override void EvolveWeapon()
    {
        isEvolve = true;
    }
}