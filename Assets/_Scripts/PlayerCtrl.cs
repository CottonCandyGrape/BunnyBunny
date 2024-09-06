using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerAnim { Idle, Run, Hurt, Dead }

public enum AtkType { Fire, Water, Count }

public class PlayerCtrl : MonoBehaviour
{
    //이동 관련
    [HideInInspector] public float h = 0.0f;
    [HideInInspector] public float v = 0.0f;
    float moveSpeed = 2.5f;
    Vector3 moveDir = Vector3.zero;
    SpriteRenderer playerSpRenderer = null;
    Rigidbody2D rigid = null;
    [HideInInspector] public float OffsetX = 4.7f;
    [HideInInspector] public float OffsetY = 5.1f;
    JoyStickCtrl joyCtrl = null;
    //이동 관련

    //Flip 관련
    CapsuleCollider2D capColl = null;
    GameObject gun = null;
    Transform mainWeapon = null;
    const float capOffsetX = 0.04f;
    const float gunOffsetX = 0.15f;
    const float gunImgOffsetX = 0.05f;
    //Flip 관련

    //화살표 관련
    public Transform MonsterPool = null;
    public GameObject DirArrow = null;
    Vector3 gunDir = Vector3.up;
    Vector3 arrowDir = Vector3.up;
    float arrowAngle = 0.0f;
    float angleOffset = 90.0f;
    const float ArrowOffset = 0.7f;
    //화살표 관련

    //능력치 관련
    //float maxHp = float.MaxValue;
    float maxHp = 200.0f;
    public float MaxHp { get { return maxHp; } }
    float curHp = 100.0f;
    float defense;
    float attack;
    public float Atk { get { return attack / 100; } }
    bool isDead = false;
    public bool IsDead { get { return isDead; } }
    //능력치 관련

    //UI 관련
    public Canvas SubCanvas = null;
    public Image HpBar_Img = null;
    Vector3 dmgTxtOffset = new Vector3(0, 0.5f, 0);
    //UI 관련

    //Timer 관련
    float mAtkTimer = 0.25f; //메인 총알
    float mAtkTime = 0.25f;
    float rktTimer = 2.0f; //로켓
    float rktTime = 2.0f;
    float drlTimer = 4.0f; //드릴
    float drlTime = 4.0f;
    //Timer 관련

    //Animation 관련
    [HideInInspector] public PlayerAnim AnimState = PlayerAnim.Idle;
    Animator animator = null;
    //Animation 관련

    //Attack Type 관련
    [HideInInspector] public AtkType AttackType = AtkType.Fire;
    //Attack Type 관련

    WeaponMgr wpMgr = null; //이 Script에서는 너무 많이 써서 선언하고 쓰는 중. (static 있어도)

    void Awake()
    {
        maxHp = AllSceneMgr.Instance.user.Hp;
        curHp = maxHp;
        attack = AllSceneMgr.Instance.user.Attack;
        defense = AllSceneMgr.Instance.user.Defense;
        AttackType = (AtkType)AllSceneMgr.Instance.AtkTypeNum;
    }

    void Start()
    {
        playerSpRenderer = GameObject.Find("Player_Img").GetComponent<SpriteRenderer>();
        capColl = GetComponent<CapsuleCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        wpMgr = FindObjectOfType<WeaponMgr>();
        mainWeapon = GameObject.Find("MainWeapon").transform;
        joyCtrl = FindObjectOfType<JoyStickCtrl>();

        SetLimitOffset();
    }

    //MapRePosition하는 큰 Box Collider 때문에 웬만하면 여기서 이 함수 구현 안함
    //void OnTriggerEnter2D(Collider2D coll) { }

    void FixedUpdate()
    {
        Move(); //충돌(collision, not trigger) 때문에 여기서 호출
    }

    void Update()
    {
        if (isDead) return;

        DirectionArrow();
        CalcWeaponsTimer();
        CalcSkillTimer();
        PlayerStateUpdate();

        if (wpMgr.MainType == MWType.Gun)
            RotateGun();

        SubCanvas.transform.position = transform.position; //Move()에 있었는데 느려서 여기서 호출 

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            wpMgr.GunCtrlSc.LevelUpWeapon();
            //wpMgr.RocketCtrlSc.LevelUpWeapon();
            //wpMgr.GuardiansCtrlSc.LevelUpWeapon();
            //wpMgr.DrillCtrlSc.LevelUpWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            wpMgr.GunCtrlSc.EvolveWeapon();
            //wpMgr.RocketCtrlSc.EvolveWeapon();
            //wpMgr.GuardiansCtrlSc.EvolveWeapon();
            //wpMgr.DrillCtrlSc.EvolveWeapon();
        }
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    wpMgr.DrillCtrlSc.LevelUpWeapon(); 
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    wpMgr.GunCtrlSc.LevelUpWeapon(); 
        //}
    }

    void Move()
    {
        if (isDead) return;

        h = joyCtrl.JoyDir.x;
        v = joyCtrl.JoyDir.y;

        //좌우 방향 바뀔때마다 flip
        if (0.0f < h) Flip(true);
        else if (h < 0.0f) Flip(false);

        moveDir = (Vector2.up * v) + (Vector2.right * h);
        if (1.0f < moveDir.magnitude)
            moveDir.Normalize();

        Vector3 targetPos = transform.position + moveDir * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(targetPos);

        if (GameMgr.Inst.hasRing) LimitRingPos(targetPos);

        if (GameMgr.Inst.MType != MapType.Ground)
            LimitPos(targetPos);
    }

    void SetLimitOffset()
    {
        if (GameMgr.Inst.MType == MapType.Ground)
            OffsetX = 4.7f;
        else if (GameMgr.Inst.MType == MapType.Vertical)
            OffsetX = 2.85f;
        else if (GameMgr.Inst.MType == MapType.FixedGround)
            OffsetX = 6.3f;

        if (GameMgr.Inst.MType == MapType.FixedGround)
            OffsetY = 7.4f;
        else
            OffsetY = 5.1f;
    }

    void LimitRingPos(Vector2 pos)
    {
        pos.x = Mathf.Clamp(pos.x, GameMgr.Inst.BattleRing.transform.position.x - OffsetX,
            GameMgr.Inst.BattleRing.transform.position.x + OffsetX);
        pos.y = Mathf.Clamp(pos.y, GameMgr.Inst.BattleRing.transform.position.y - OffsetY,
            GameMgr.Inst.BattleRing.transform.position.y + OffsetY);

        rigid.MovePosition(pos);
    }

    //void LimitXPos(Vector2 pos)
    //{
    //    if (pos.x >= OffsetX)
    //        pos.x = OffsetX;
    //    else if (pos.x <= -OffsetX)
    //        pos.x = -OffsetX;

    //    rigid.MovePosition(pos);
    //}

    //void LimitYPos(Vector2 pos)
    //{
    //    if (pos.y >= OffsetY)
    //        pos.y = OffsetY;
    //    else if (pos.y <= -OffsetY)
    //        pos.y = -OffsetY;

    //    rigid.MovePosition(pos);
    //}

    void LimitPos(Vector2 pos)
    {
        if (pos.x >= OffsetX)
            pos.x = OffsetX;
        else if (pos.x <= -OffsetX)
            pos.x = -OffsetX;

        if (GameMgr.Inst.MType == MapType.FixedGround)
        {
            if (pos.y >= OffsetY)
                pos.y = OffsetY;
            else if (pos.y <= -OffsetY)
                pos.y = -OffsetY;
        }

        rigid.MovePosition(pos);
    }

    void Flip(bool flip)
    {
        Vector2 tmpVec = Vector2.zero;

        //player flip
        playerSpRenderer.flipX = flip;

        //collider flip
        tmpVec = capColl.offset;
        tmpVec.x = flip ? capOffsetX : -capOffsetX;
        capColl.offset = tmpVec;

        if (wpMgr.MainType == MWType.Gun)
        {
            //gun flip
            if (gun == null)
                gun = mainWeapon.GetChild(0).gameObject;

            tmpVec = gun.transform.localPosition;
            tmpVec.x = flip ? gunOffsetX : -gunOffsetX;
            gun.transform.localPosition = tmpVec;

            SpriteRenderer spRend = gun.GetComponentInChildren<SpriteRenderer>();
            spRend.flipX = flip;

            tmpVec = spRend.transform.localPosition;
            tmpVec.x = flip ? gunImgOffsetX : -gunImgOffsetX;
            spRend.transform.localPosition = tmpVec;
        }
    }

    void DirectionArrow()
    {
        if (moveDir.normalized != Vector3.zero)
            arrowDir = moveDir.normalized;

        arrowAngle = Mathf.Atan2(arrowDir.normalized.y, arrowDir.normalized.x) * Mathf.Rad2Deg;
        DirArrow.transform.rotation = Quaternion.AngleAxis(arrowAngle - angleOffset, Vector3.forward);
        DirArrow.transform.position = transform.position + arrowDir.normalized * ArrowOffset;
    }

    void RotateGun()
    {
        if (gun == null)
            gun = mainWeapon.GetChild(0).gameObject;

        if (moveDir.normalized != Vector3.zero)
            arrowDir = moveDir.normalized;

        if (h < 0.0f)
            gun.transform.rotation = Quaternion.AngleAxis(arrowAngle - 180f, Vector3.forward);
        else if (0.0f < h)
            gun.transform.rotation = Quaternion.AngleAxis(arrowAngle, Vector3.forward);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        SoundMgr.Instance.PlaySfxSound("attacked");

        damage *= (100.0f - defense) / 100.0f;
        curHp -= damage;
        //Hp UI 수정
        HpBar_Img.fillAmount = curHp / maxHp;

        if (curHp <= 0.0f)
        {
            isDead = true;
            capColl.isTrigger = true;
            StartCoroutine(PlayerDie());
        }
    }

    public void GetHp()
    {
        float hRate = AllSceneMgr.Instance.user.Heal / 100;
        float heal = maxHp * hRate;

        curHp += heal;
        if (maxHp <= curHp)
            curHp = maxHp;

        HpBar_Img.fillAmount = curHp / maxHp;
    }

    Vector3 GetCloseTarget()
    {
        Vector3 target = Vector3.up;

        MonsterCtrl[] monsters = MonsterPool.GetComponentsInChildren<MonsterCtrl>();
        if (monsters.Length > 0) 
        {
            target = monsters[0].transform.position;

            float dist = Vector3.Distance(transform.position, target);
            for (int i = 1; i < monsters.Length; i++)
            {
                float tmp = Vector3.Distance(transform.position, monsters[i].transform.position);
                if (tmp < dist)
                {
                    dist = tmp;
                    target = monsters[i].transform.position;
                }
            }
        }

        return target;
    }

    void SetGunDir()
    {
        if (!GameMgr.Inst.hasBoss)
        {
            Vector3 target = GetCloseTarget();
            gunDir = target - transform.position;
            gunDir.Normalize();
        }
        else
            gunDir = arrowDir;
    }

    void CalcWeaponsTimer()
    {
        //메인 무기 타이머
        mAtkTimer -= Time.deltaTime;
        if (mAtkTimer <= 0.0f)
        {
            mAtkTimer = mAtkTime;
            if (wpMgr.GunCtrlSc != null)
            {
                SetGunDir();

                if (!wpMgr.GunCtrlSc.IsEvolve)
                {
                    wpMgr.GunCtrlSc.shotCnt++;

                    if (wpMgr.GunCtrlSc.shotCnt > wpMgr.GunCtrlSc.CurLv + 1)
                    {
                        if (wpMgr.GunCtrlSc.shotCnt > GunCtrl.LimitShotCnt)
                            wpMgr.GunCtrlSc.shotCnt = 0;
                        return;
                    }

                    SoundMgr.Instance.PlaySfxSound("laser");

                    if (wpMgr.GunCtrlSc.CurLv == 0)
                        wpMgr.GunCtrlSc.OneShot(gunDir, false);
                    else if (wpMgr.GunCtrlSc.CurLv == 1)
                        wpMgr.GunCtrlSc.DoubleShot(gunDir);
                    else if (wpMgr.GunCtrlSc.CurLv == 2)
                        wpMgr.GunCtrlSc.FanFire(3, gunDir);
                    else if (wpMgr.GunCtrlSc.CurLv == 3)
                        wpMgr.GunCtrlSc.FanFire(5, gunDir);
                }
                else
                {
                    SoundMgr.Instance.PlaySfxSound("laser");
                    wpMgr.GunCtrlSc.EvolvedShot(gunDir);
                }
            }
        }
        //메인 무기 타이머

        //로켓 타이머
        if (wpMgr.RocketCtrlSc.CurLv > 0)
        {
            rktTimer -= Time.deltaTime;
            if (rktTimer <= 0.0f)
            {
                rktTimer = rktTime;
                if (!wpMgr.RocketCtrlSc.IsEvolve)
                    wpMgr.RocketCtrlSc.FireRocket();
                else
                    wpMgr.RocketCtrlSc.FireNuclear();
            }
        }
        //로켓 타이머

        //드릴 타이머
        if (wpMgr.DrillCtrlSc.CurLv > 0)
        {
            drlTimer -= Time.deltaTime;
            if (drlTimer <= 0.0f)
            {
                drlTimer = drlTime;
                if (!wpMgr.DrillCtrlSc.IsEvolve)
                    wpMgr.DrillCtrlSc.FireDrills();
                else
                    wpMgr.DrillCtrlSc.FireArrowHead();
            }
        }
        //드릴 타이머
    }

    void CalcSkillTimer()
    {
        //자석 스킬
        if (SkillMgr.Inst.MagentCtrlSc.CurLv > 0)
        {
            SkillMgr.Inst.MagentCtrlSc.Magneting();
        }
        //자석 스킬
    }

    //state, action update 하는 함수 따로 만들기?
    void PlayerStateUpdate()
    {
        if (moveDir.normalized == Vector3.zero)
            animator.SetBool("Moving", false);
        else
            animator.SetBool("Moving", true);
    }

    public void TrapBossRing(bool trap)
    {
        if (trap)
        {
            capColl.isTrigger = false;
            rigid.bodyType = RigidbodyType2D.Dynamic;
            rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rigid.gravityScale = 0.0f;
            rigid.mass = 0.0f; // 플레이어가 보스 밀지 못하게 0
        }
        else
        {
            capColl.isTrigger = true;
        }
    }

    IEnumerator PlayerDie()
    {
        animator.SetBool("Dead", true);
        AnimatorStateInfo animInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!animInfo.IsName("Bunny_Dead"))
        {
            animInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        yield return new WaitForSeconds(animInfo.length + 0.3f);

        GameMgr.Inst.GameOver(GameMgr.Inst.stageClear);
    }

    /*
    void MoveLimit() 
    {
        limitPos = transform.position;

        if (ScreenMgr.ScMin.x > transform.position.x)
            limitPos.x = ScreenMgr.ScMin.x;
        if (ScreenMgr.ScMax.x < transform.position.x)
            limitPos.x = ScreenMgr.ScMax.x;
        if (ScreenMgr.ScMin.y > transform.position.y)
            limitPos.y = ScreenMgr.ScMin.y;
        if (ScreenMgr.ScMax.y < transform.position.y)
            limitPos.y = ScreenMgr.ScMax.y;

        transform.position = limitPos;
    }
    */
}
