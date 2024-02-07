using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBg : MonoBehaviour
{
    float scrollSpeed = 0.05f;
    float offset = 0.0f;

    SpriteRenderer spRender = null;

    void Start()
    {
        spRender = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        offset += Time.deltaTime * scrollSpeed;
        if (10000.0f <= offset)
            offset -= 10000.0f;

        spRender.material.mainTextureOffset = new Vector2(offset, offset);
    }
}