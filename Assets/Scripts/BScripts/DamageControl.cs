using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageControl : MonoBehaviour
{
    public float _damage = 5.0f;
    public bool _itsChost = false;

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll != null)
        {
            if (coll.collider.tag == "PlayerGame")
            {
                coll.collider.gameObject.GetComponent<PlayerController>()._healPoint = coll.collider.gameObject.GetComponent<PlayerController>()._healPoint - _damage;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll != null && _itsChost)//проверяем тригерр столкнувшийся триггер является призраком? если нет, то игнорируем, если да, то передаем урон 
        {
            if (coll.tag == "PlayerGame")
            {
                coll.gameObject.GetComponent<PlayerController>()._healPoint = coll.gameObject.GetComponent<PlayerController>()._healPoint - _damage;
            }
        }
    }
}
