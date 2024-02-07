using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMgr : MonoBehaviour
{
    const int width = 9;
    const int height = 16;

    public static Vector3 InitScMin = Vector3.zero;
    public static Vector3 InitScMax = Vector3.zero;

    public static Vector3 CurScMin = Vector3.zero;
    public static Vector3 CurScMax = Vector3.zero;

    Camera cam = null;

    void Awake()
    {
        cam = Camera.main;

        Rect rect = cam.rect;

        //기기 화면비
        float deviceRatio = (float)Screen.width / Screen.height;
        //원하는 화면비
        float targetRatio = (float)width / height;

        //Viewport Coords에서의 Height, Width 크기 (0f~1f)
        float scaleHeight = deviceRatio / targetRatio;
        float scaleWidth = 1.0f / scaleHeight;

        if (scaleHeight < 1.0f) //세로가 더 큰경우(==위아래에 레터박스 생김)
        {
            rect.height = scaleHeight;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else //가로가 더 큰경우(==좌우에 레터박스 생김)
        {
            rect.width = scaleWidth;
            rect.x = (1.0f - scaleWidth) / 2.0f;
        }

        cam.rect = rect;

        //rect 설정 후 Screen Size 얻어오기
        InitScMin = cam.ViewportToWorldPoint(Vector3.zero);
        InitScMax = cam.ViewportToWorldPoint(Vector3.one);
    }

    void Start()
    {
        Debug.Log("Camera.main.rect.width : " + Camera.main.rect.width);
        Debug.Log("Camera.main.rect.height : " + Camera.main.rect.height);
        Debug.Log("Camera.main.rect.x : " + Camera.main.rect.x);
        Debug.Log("Camera.main.rect.y : " + Camera.main.rect.y);
    }

    void Update()
    {
        Debug.Log("Camera.main.aspect : " + Camera.main.aspect);
        Debug.Log("Screen.width : " + Screen.width);
        Debug.Log("Screen.height : " + Screen.height);
        CurScMin = cam.ViewportToWorldPoint(Vector3.zero);
        CurScMax = cam.ViewportToWorldPoint(Vector3.one);
    }
}
