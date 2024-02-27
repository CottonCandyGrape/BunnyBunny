using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{ 
    //이동 관련
    [HideInInspector] public float h = 0.0f;
    [HideInInspector] public float v = 0.0f;
    float moveSpeed = 3.0f;

    Vector3 moveDir = Vector3.zero;
    SpriteRenderer playerSpRenderer = null;
    //Vector3 limitPos = Vector3.zero;

    public GameObject DirArrow = null;
    [HideInInspector] public Vector3 arrowDir = Vector3.up;
    float arrowAngle = 0.0f;
    float angleOffset = 90.0f;
    const float cstArrowDist = 0.7f;
    //이동 관련

    //능력치 관련
    float curHp = 100.0f;
    float maxHp = 100.0f;
    //float maxHp = float.MaxValue;
    float attack = 10.0f;
    float defense = 10.0f;
    //능력치 관련

    //공격 관련
    float atkTimer = 0.0f;
    float atkTime = 0.2f;
    int fireCnt = 5;
    int curFire = 0;
    //공격 관련

    //UI 관련
    public Canvas SubCanvas = null;
    public Image HpBar_Img = null;
    Vector3 dmgTxtOffset = new Vector3(0, 0.5f, 0);
    //UI 관련

    //TODO : Skill

    void Start()
    {
        playerSpRenderer = GameObject.Find("Player_Img").GetComponent<SpriteRenderer>();
        curHp = maxHp;

        //WeaponMgr.Inst.SetGuardians(); //가디언 test 용
    }

    void Update()
    {
        Move();
        DirectionArrow();
        ReadyAttack();
        //MoveSubCanvas();

        //if (Input.GetKeyDown(KeyCode.Space))
        //    WeaponMgr.Inst.LevelUpGuardiands(); //가디언 test 용
    }

    //MapRePosition하는 큰 Box Collider 때문에 웬만하면 여기서 이 함수 구현 안함
    //void OnTriggerEnter2D(Collider2D coll) { }

    void Move()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        //좌우 방향 바뀔때마다 flip
        if (0.0f < h)
            playerSpRenderer.flipX = false;
        else if (h < 0.0f)
            playerSpRenderer.flipX = true;

        moveDir = (Vector2.up * v) + (Vector2.right * h);
        if (1.0f < moveDir.magnitude)
            moveDir.Normalize();

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        SubCanvas.transform.position = transform.position; //subcanvas 까지 움직임. 한줄이라 여기에 추가
    }

    void DirectionArrow()
    {
        if (moveDir.normalized != Vector3.zero)
            arrowDir = moveDir.normalized;

        arrowAngle = Mathf.Atan2(arrowDir.normalized.y, arrowDir.normalized.x) * Mathf.Rad2Deg;
        DirArrow.transform.rotation = Quaternion.AngleAxis(arrowAngle - angleOffset, Vector3.forward);
        DirArrow.transform.position = transform.position + arrowDir.normalized * cstArrowDist;
    }

    void ReadyAttack() // 공격 텀 재는 함수
    {
        atkTimer -= Time.deltaTime;

        if (atkTimer <= 0.0f)
        {
            atkTimer = atkTime;

            if (fireCnt < curFire)
            {
                WeaponMgr.Inst.GunCtrlSc.FanFire(arrowDir);
                curFire = 0;
            }
            else
            {
                WeaponMgr.Inst.GunCtrlSc.FireBullet(arrowDir);
                curFire++;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        //1. Hp변수 깎기
        float dmgTxt = curHp < damage ? curHp : damage;
        curHp -= damage;
        //2. Hp UI 수정
        HpBar_Img.fillAmount = curHp / maxHp;
        //3. Dmg Txt 띄우기 
        GameMgr.Inst.SpawnDmgTxt(transform.position + dmgTxtOffset, dmgTxt, Color.red);

        if (curHp <= 0.0f)
        {
            PlayerDie();
        }

        //TODO : UI 데미지 표시
    }

    public void GetHp(float healRate)
    {
        float heal = maxHp * healRate;

        curHp += heal;
        if (maxHp <= curHp)
            curHp = maxHp;

        HpBar_Img.fillAmount = curHp / maxHp;

        GameMgr.Inst.SpawnDmgTxt(transform.position + dmgTxtOffset, heal, Color.blue);
    }

    void PlayerDie()
    {
        Time.timeScale = 0.0f;
        return;
    }

    /* //너무 짧아서 그냥 호출
    void MoveSubCanvas()
    {
        SubCanvas.transform.position = transform.position;
    }
    */

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
