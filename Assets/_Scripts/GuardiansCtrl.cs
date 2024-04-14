using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardiansCtrl : Weapon
{
    public GameObject GuardPrefab = null;
    public Transform Guardians = null;
    public Sprite Ev_Sprite = null;

    const float Ev_RotSpeed = 250.0f;
    const float Ev_CollRadius = 0.21f;
    const int GuardInitCount = 3;

    float lifeTimer = 0.0f;
    float lifeTime = 3.0f;
    float waitTimer = 0.0f;
    float waitTime = 1.0f;

    bool IsOn = true;

    void Start()
    {
        InitGuardians();
    }

    void Update()
    {
        if (!isEvolve)
            OnOffGuardians();
    }

    void InitGuardians() //3개로 시작
    {
        for (int i = 0; i < GuardInitCount; i++)
        {
            GameObject guardObj = Instantiate(GuardPrefab, Guardians);
            GuardCtrl guard = guardObj.GetComponent<GuardCtrl>();
            guard.Degree = (360 / GuardInitCount) * i;
        }
    }

    void OnOffGuardians()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0.0f)
        {
            if (IsOn)
            {
                for (int i = 0; i < Guardians.childCount; i++)
                    Guardians.GetChild(i).gameObject.SetActive(false);
                IsOn = false;
            }

            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0.0f)
            {
                for (int i = 0; i < Guardians.childCount; i++)
                    Guardians.GetChild(i).gameObject.SetActive(true);

                lifeTimer = lifeTime;
                waitTimer = waitTime;
                IsOn = true;
            }
        }
    }

    public override void LevelUpWeapon()
    {
        if (MaxLevel <= curLevel)
        {
            if (!isEvolve) EvolveWeapon();
            return;
        }

        GameObject guardObj = Instantiate(GuardPrefab, Guardians);
        if (!IsOn) guardObj.SetActive(false);

        GuardCtrl[] guards = Guardians.GetComponentsInChildren<GuardCtrl>(true);
        for (int i = 0; i < guards.Length; i++)
            guards[i].Degree = (360 / guards.Length) * i;

        curLevel++;
    }

    public override void EvolveWeapon()
    {
        isEvolve = true;
        for (int i = 0; i < Guardians.childCount; i++)
        {
            GameObject guard = Guardians.GetChild(i).gameObject;
            guard.SetActive(true);

            GuardCtrl gCtrl = guard.GetComponent<GuardCtrl>();
            if (gCtrl != null) gCtrl.RotSpeed = Ev_RotSpeed;

            CircleCollider2D cColl = guard.GetComponent<CircleCollider2D>();
            if (cColl != null) cColl.radius = Ev_CollRadius;

            SpriteRenderer spRend = guard.GetComponentInChildren<SpriteRenderer>();
            if (spRend != null) spRend.sprite = Ev_Sprite;
        }
    }
}