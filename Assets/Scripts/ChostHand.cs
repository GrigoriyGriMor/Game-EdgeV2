using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChostHand : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (other.gameObject.GetComponent<PlayerController>().root) return;

            anim.SetTrigger("Root");
            other.gameObject.GetComponent<PlayerController>().RootHand();
            GameLevel.Instance.OnLose();
            player = other.gameObject;
        }
    }


    private void Update()
    {
        if (player != null)
        {
            player.transform.position = Vector2.Lerp(player.transform.position, gameObject.transform.position, 1);
        }
    }
}
