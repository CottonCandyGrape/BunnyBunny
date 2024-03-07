using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    const int MaxLevel = 3;
    [HideInInspector] public static int level = 0; // 굳이 public, static 이어야 하는지?
    [HideInInspector] public static bool evolve = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}