using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCtrl : Weapon
{
    //const float BulletOffset = 0.3f;
    const int BaseShotCnt = 4;
    int curCount = 0;

    const float ShotOffset = 0.25f;
    const float FirePosOffsetX = 0.6f;
    Transform fPos = null;

    public GameObject Ev_Bullet = null;
    public Sprite Ev_Sprite = null;
    Vector2 ev_Scale = new Vector2(0.1f, 0.1f);

    void Start() { }

    //void Update() { }

    public void OneShot(Vector3 dir, bool evShot)
    {
        if (fPos == null)
            fPos = GameObject.FindGameObjectWithTag("FirePos").transform;
        SetFirePos();

        BulletCtrl bltCtrl;
        if (!evShot)
            bltCtrl = MemoryPoolMgr.Inst.AddBulletPool();
        else
            bltCtrl = MemoryPoolMgr.Inst.AddEvBulletPool();
        bltCtrl.gameObject.SetActive(true);

        dir.Normalize();
        bltCtrl.MoveDir = dir;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bltCtrl.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        bltCtrl.transform.position = fPos.position;
    }

    public void DoubleShot(Vector3 dir)
    {
        for (int i = 0; i < 2; i++)
        {
            BulletCtrl bltCtrl = MemoryPoolMgr.Inst.AddBulletPool();
            bltCtrl.gameObject.SetActive(true);

            dir.Normalize();
            bltCtrl.MoveDir = dir;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bltCtrl.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            bltCtrl.transform.position = GetFirePosDS(i, dir);
        }
    }

    public void FanFire(int num, Vector3 midDir) //부채꼴 모양 발사 스킬
    {
        int deg = 20;
        int start, end;

        if (num == 3)
        {
            start = -1; end = 2;
        }
        else if (num == 5)
        {
            start = -2; end = 3;
        }
        else
        {
            Debug.Log("개수가 맞지 않습니다."); return;
        }

        for (int cnt = start; cnt < end; cnt++)
        {
            Vector3 dir = Quaternion.AngleAxis(cnt * deg, Vector3.forward) * midDir;
            OneShot(dir, false);
        }
    }

    public void EvolvedShot(Vector3 dir)
    {
        if (!isEvolve) return;

        //OneShot(dir, true); return; //진화 총알 test 용

        if (curCount < BaseShotCnt)
        {
            FanFire(5, dir);
            curCount++;
        }
        else
        {
            OneShot(dir, true);
            curCount = 0;
        }
    }

    void SetFirePos()
    {
        if (fPos == null) return;

        Vector3 tmp = fPos.localPosition;
        tmp.x = GameMgr.Inst.player.h < 0.0f ? -FirePosOffsetX : FirePosOffsetX;
        fPos.localPosition = tmp;
    }

    Vector3 GetFirePosDS(int idx, Vector3 dir)
    {
        if (fPos == null)
            fPos = GameObject.FindGameObjectWithTag("FirePos").transform;
        SetFirePos();

        Vector3 firePos = fPos.position;
        Vector3 rDir = Quaternion.AngleAxis(90, Vector3.forward) * dir;

        if (idx == 0)
            firePos += rDir * (ShotOffset / 2.0f);
        else if (idx == 1)
            firePos -= rDir * (ShotOffset / 2.0f);

        return firePos;
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

        SpriteRenderer spRend = GetComponentInChildren<SpriteRenderer>();
        if (spRend != null)
        {
            spRend.sprite = Ev_Sprite;
            spRend.transform.localScale = ev_Scale;
        }

        MemoryPoolMgr.Inst.InitEvBulletPool();
        MemoryPoolMgr.Inst.InitEvSupBulletPool();
    }

    public override string GetExplainText()
    {
        if (curLevel == 0)
            return "더블샷.";
        else if (curLevel == 1)
            return "트리플샷.";
        else if (curLevel == 2)
            return "반달샷.";
        else if (curLevel == 3)
            return "거대 에너지 추가 발사.";
        return string.Empty;
    }

    /*
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
    */
}