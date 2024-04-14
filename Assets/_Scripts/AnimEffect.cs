using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEffect : MonoBehaviour
{
    Animator anim = null;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            gameObject.SetActive(false);
    }
}