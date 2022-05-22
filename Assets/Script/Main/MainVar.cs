using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainVar : MonoBehaviour
{

    public bool isArmed = false;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
