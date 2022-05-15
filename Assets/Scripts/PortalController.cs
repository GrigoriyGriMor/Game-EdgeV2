using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] private AudioClip portalSound;

    public void Start()
    {
        if (SoundManager.Instance) SoundManager.Instance.ClipPlay(portalSound);
    }

    private void FixedUpdate()
    {
        transform.position = new Vector2((transform.position.x + InstantiateLevel.Instance.moveSpeed * InstantiateLevel.Instance.moveLevelTHIRD * 2.5f), transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            other.transform.position = new Vector2(other.transform.position.x, other.transform.position.y * -1);
            transform.position = new Vector2(transform.position.x, transform.position.y * -1);

            if (!other.GetComponent<PlayerController>().inversGravity)
            other.GetComponent<PlayerController>().inversGravity = true;
            else
                other.GetComponent<PlayerController>().inversGravity = false;
        }
    }

}
