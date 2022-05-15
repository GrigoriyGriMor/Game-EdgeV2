using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioSource audioSource;

    public void SetVolume()
    {

        if (audioSource.volume == 1.0f)
            audioSource.volume = 0f;
        else audioSource.volume = 1f; ;
    }
}
