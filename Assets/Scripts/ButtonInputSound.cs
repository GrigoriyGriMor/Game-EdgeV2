using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInputSound : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private void Start()
    {
        if (GetComponent<Button>()) GetComponent<Button>().onClick.AddListener(() => OnClick());
    }

    private void OnClick()
    {
        if (SoundManager.Instance) SoundManager.Instance.ClipPlay(clip);
    }
}
