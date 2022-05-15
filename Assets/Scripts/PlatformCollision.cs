using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollision : MonoBehaviour
{//это дополнительный класс, что бы игрок мог запрыгнуть на платформу снизу вверх, игнорируя ее коллайдер
    public GameObject Player;
    private Collider2D playerCollider;
    private BoxCollider2D _controlerTriggerActive;
    private PlayerController playerController;

    private void Start()
    {
        Player = GameObject.Find("Player");//!!! ВНИМАНИЕ - в классе PlatformCollision идет инициализация игрока в Awake (то есть, если игрок на сцене появится после платформы
        playerCollider = Player.gameObject.GetComponent<Collider2D>();
        playerController = Player.gameObject.GetComponent<PlayerController>();
        //класс на этой платформе его не увидит)
        _controlerTriggerActive = gameObject.GetComponent<BoxCollider2D>();
    }

    public void FixedUpdate()
    {
        if (playerController.yToStandatre)
        {
            if (!_controlerTriggerActive.isTrigger && (gameObject.transform.position.y > playerCollider.bounds.min.y))//если платформа твердая и ее позиция стала выше позиции игрока
            {
                _controlerTriggerActive.isTrigger = true;//делаем платформу тригерром
            }
            else
                if (_controlerTriggerActive.isTrigger && (gameObject.transform.position.y < playerCollider.bounds.min.y))//если платформа мягкая и ее позиция стала ниже позиции игрока
                {
                _controlerTriggerActive.isTrigger = false;//делаем платформу коллайдером
                }
        }
        else
        {
            if (!_controlerTriggerActive.isTrigger && (gameObject.transform.position.y < playerCollider.bounds.max.y))//если платформа твердая и ее позиция стала выше позиции игрока
            {
                _controlerTriggerActive.isTrigger = true;//делаем платформу тригерром
            }
            else
                if (_controlerTriggerActive.isTrigger && (gameObject.transform.position.y > playerCollider.bounds.max.y))//если платформа мягкая и ее позиция стала ниже позиции игрока
                {
                _controlerTriggerActive.isTrigger = false;//делаем платформу коллайдером
                }
        }

    }
}
