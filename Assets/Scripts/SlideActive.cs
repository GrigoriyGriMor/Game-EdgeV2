using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideActive : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerController player;

    public void OnPointerDown(PointerEventData data)
    {
        player.SlideStart();
        drag = true;
    }

   // public void OnDrag(PointerEventData data)
   // {
   //     player.SlideDrag();
   // }

    private bool drag = false;

    private void Update()
    {
        if (drag) player.SlideDrag();
    }

    public void OnPointerUp(PointerEventData data)
    {
        player.SlideStop();
        drag = false;
    }

    public void OnPointerExit(PointerEventData data)
    {
        player.SlideStop();
        drag = false;
    }
}
