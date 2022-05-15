using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoBackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private PlayerController player;

    public void OnPointerDown(PointerEventData data)
    {
        player.goBack = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        player.goBack = false;
    }

    public void OnPointerExit(PointerEventData data)
    {
        player.goBack = false;
    }

}
