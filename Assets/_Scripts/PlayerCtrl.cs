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

    public Vector3 moveDir = Vector3.zero;
    SpriteRenderer playerSpRenderer = null;
    //Vector3 limitPos = Vector3.zero;

    public GameObject DirArrow = null;
    public Vector3 arrowDir = Vector3.up;
    float arrowAngle = 0.0f;
    float angleOffset = 90.0f;
    const float cstArrowDist = 0.7f;
    //이동 관련

    //능력치 관련
    float curHp = 100.0f;
    float maxHp = 100.0f;
    float attack = 10.0f;
    float defense = 10.0f;
    float curExp = 0.0f;
    float nextExp = 100.0f;
    //능력치 관련

    //공격 관련
    public Vector3 bulletDir = Vector3.up;
    const float cstBulletDist = 0.3f;
    float bulletTimer = 0.0f;
    float bltTime = 0.2f;
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
    }

    void Update()
    {
        Move();
        DirectionArrow();
        LoadBullet();
        //MoveSubCanvas();
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

    void LoadBullet()
    {
        bulletTimer -= Time.deltaTime;

        if (bulletTimer <= 0.0f)
        {
            bulletTimer = bltTime;

            if (fireCnt < curFire)
            {
                FanFire(arrowDir);
                curFire = 0;
            }
            else
            {
                FireBullet(arrowDir);
                curFire++;
            }
        }
    }

    void FireBullet(Vector3 bltDir)
    {
        bltDir.Normalize();
        bulletDir = bltDir;

        BulletCtrl bltCtrl = MemoryPoolMgr.Inst.AddBulletPool();
        bltCtrl.gameObject.SetActive(true);
        bltCtrl.transform.position = transform.position + bltDir * cstBulletDist;
        float angle = Mathf.Atan2(bltDir.y, bltDir.x) * Mathf.Rad2Deg;
        bltCtrl.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void FanFire(Vector3 midDir)
    {
        int deg = 20; //TODO : 하드코딩 바꾸기 
        for (int cnt = -2; cnt < 3; cnt++)
        {
            Vector3 dir = Quaternion.AngleAxis(cnt * deg, Vector3.forward) * midDir;
            FireBullet(dir);
        }
    }

    public void TakeDamage(float damage)
    {
        //1. Hp변수 깎기
        curHp -= damage;
        //2. Hp UI 수정
        HpBar_Img.fillAmount = curHp / maxHp;
        //3. Dmg Txt 띄우기 
        GameMgr.Inst.SpawnDmgTxt(transform.position + dmgTxtOffset, damage);

        if (curHp <= 0.0f)
        {
            PlayerDie();
        }

        //TODO : UI 데미지 표시
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
