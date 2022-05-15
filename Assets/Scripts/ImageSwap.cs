using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwap : MonoBehaviour
{
    [SerializeField] private Sprite imageActive;
    [SerializeField] private Sprite imageDeactive;

    public void Swap()
    {
        if (gameObject.GetComponent<Image>().sprite.name == imageActive.name)
        {
            if (imageDeactive != null)
                gameObject.GetComponent<Image>().sprite = imageDeactive;
        }
        else
            if (imageActive != null)
                gameObject.GetComponent<Image>().sprite = imageActive;

    }
}
