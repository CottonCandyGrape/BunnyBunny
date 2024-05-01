using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEffect : MonoBehaviour
{
    Animator anim = null;

    GameObject target = null;
    public GameObject Target { set { target = value; } }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
            gameObject.SetActive(false);

        if (target != null)
            transform.position = target.transform.position;
    }
}