using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public static GameMgr inst = null; 
    public PlayerCtrl player = null;

    void Awake()
    {
        inst = this;
    }
}