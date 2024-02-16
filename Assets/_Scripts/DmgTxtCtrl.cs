using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DmgTxtCtrl : MonoBehaviour
{
    float moveTime = 0.5f;
    float moveSpeed = 1.0f;

    public Text dmg_Txt = null;

    void Start()
    {
    }

    void Update()
    {
        //위치로 Destroy 시점 판별함. 시간으로 바꿀수도 있음.
        //위치로 하니깐 안댐. 할거면 상대 위치로 하던가. 월드로 하니깐 내가 내려가면 안사라짐. 
        //시간으로 하자.

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

    public void Init(Vector3 pos, float damage) //TODO : Color, Alpha 값 추가될 수도 있음
    {
        transform.position = pos;
        if (dmg_Txt != null)
            dmg_Txt.text = "-" + ((int)damage).ToString();
    }
}