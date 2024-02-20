using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DmgTxtCtrl : MonoBehaviour
{
    float moveTime = 0.5f;
    float moveSpeed = 1.0f;

    public Text dmg_Txt = null;

    void Start() { }

    void Update()
    {
        moveTime -= Time.deltaTime;

        if (0.0f <= moveTime)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init(Vector3 pos, float damage, Color txt_color) //TODO : Alpha 값 추가될 수도 있음
    {
        transform.position = pos;
        if (dmg_Txt != null)
        {
            dmg_Txt.text = ((int)damage).ToString();
            dmg_Txt.color = txt_color;
        }
    }
}