using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBg : MonoBehaviour
{
    float scrollSpeed = 0.05f;
    float offset = 0.0f;

    Image img = null;

    void Start()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        offset += Time.deltaTime * scrollSpeed;
        if (10000.0f <= offset)
            offset -= 10000.0f;

        img.material.mainTextureOffset = new Vector2(offset, offset);
    }
}