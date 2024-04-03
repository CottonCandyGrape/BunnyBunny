using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBg : MonoBehaviour
{
    float scrollSpeed = 0.05f;
    float offset = 0.0f;

    RawImage rawImg = null;
    Rect uvRect;

    void Start()
    {
        rawImg = GetComponent<RawImage>();
    }

    void Update()
    {
        offset += Time.deltaTime * scrollSpeed;
        if (10000.0f <= offset)
            offset -= 10000.0f;

        uvRect = rawImg.uvRect;
        uvRect.x = offset;
        uvRect.y = offset;
        rawImg.uvRect = uvRect;
    }
}