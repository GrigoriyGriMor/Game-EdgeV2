using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AlphaButtonController : MonoBehaviour
{
    [Range(0f, 1f)] public float AlphaLevelInput = 1;

    void Awake()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaLevelInput;    
    }
}
