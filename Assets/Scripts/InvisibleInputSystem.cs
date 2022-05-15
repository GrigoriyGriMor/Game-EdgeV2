using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class InvisibleInputSystem : MonoBehaviour
{
    private static InvisibleInputSystem instance;
    public static InvisibleInputSystem Instance => instance;

    public bool activeSystem = false;

    public UnityEvent jumpActive = new UnityEvent();
    public UnityEvent slideActive = new UnityEvent();
    public UnityEvent teleportActive = new UnityEvent();

    public UnityEvent slideStart = new UnityEvent();
    public UnityEvent slideCancel = new UnityEvent();

    public GameObject buttonInputSystem;

    private float screenMiddle = 0.0f;

    private Vector2[] startCastTeleportSlidePos = new Vector2[2];
    private bool coolDown = false;
    private bool touchStart = false;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1 && !touchStart && Input.GetTouch(0).position.x < screenMiddle)
                slideStart.Invoke();

            if (Input.touchCount > 1 && !touchStart)
            {
                startCastTeleportSlidePos = new Vector2[Input.touchCount];

                for (int i = 0; i < Input.touchCount; i++)
                    startCastTeleportSlidePos[i] = Input.GetTouch(i).position;

                touchStart = true;
            }

            InputController();

        }
        else
        {
            slideCancel.Invoke();
            touchStart = false;
        }
    }



    private void InputController()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).position.x < screenMiddle)
                slideActive.Invoke();
            else
                jumpActive.Invoke();
        }
        else
        {
            for (int i = 0; i < Input.touchCount; i++)
                if (Input.GetTouch(i).position != startCastTeleportSlidePos[i])
                    UseTeleport();
        }
            

    }

    private void UseTeleport()
    {
        if (!coolDown)
        {
            teleportActive.Invoke();
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        coolDown = true;

        yield return new WaitForSeconds(3);
        coolDown = false;
    }

    private void Start()
    {
        instance = this;
        //if (activeSystem) buttonInputSystem.SetActive(false);
        //else
        //    buttonInputSystem.SetActive(true);
        screenMiddle = Screen.width / 2;
    }
}

