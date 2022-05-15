using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpideFall : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private bool animUse = false;

    private void Start()
    {
        if (animUse == true)
        {
            animUse = false;
            anim.SetTrigger("Reset");
        }
    }

    public void BreakSpide()
    {
        anim.SetTrigger("breakSpide");
        animUse = true;
    }
}
