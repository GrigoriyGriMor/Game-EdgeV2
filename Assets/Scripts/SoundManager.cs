using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance => instance;

    [SerializeField] private int souceCurrent = 5;
    [SerializeField] private AudioSource[] sorce;

    [SerializeField] private AudioSource backgrounSorce;

    private void Awake()
    {
        SoundManager[] _go = FindObjectsOfType<SoundManager>();
        for (int i = 0; i < _go.Length; i++)
            if (_go[i] != this)
                Destroy(_go[i].gameObject);

        instance = this;
        sorce = new AudioSource[souceCurrent];
        Inicialize();
    }

    public void Update()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Inicialize()
    {
        backgrounSorce = gameObject.AddComponent<AudioSource>();//создаем отдельный сорс для бэкграунда
        for (int i = 0; i < sorce.Length; i++)
        {
            if (sorce[i] == null)
                sorce[i] = gameObject.AddComponent<AudioSource>(); 
        }
    }

    public void BackgroundClipPlay(AudioClip clip)
    {
        backgrounSorce.clip = clip;
        backgrounSorce.loop = true;
        backgrounSorce.Play();
    }

    public void ClipPlay(AudioClip clip)
    {
        AudioSource sr = GetFree();
        if (sr != null) sr.PlayOneShot(clip);
    }

    public void ClipLoopAndPlay(AudioClip clip)
    {
        AudioSource sr = GetFree();
        sr.clip = clip;
        sr.loop = true;
        sr.Play();
    }

    private AudioSource GetFree()
    {
        for (int i = 0; i < sorce.Length; i++)
        {
            if (sorce[i].clip == null)
                return sorce[0];
        }

        return null;
    }

    public void ClearSound()
    {
        for (int i = 0; i < sorce.Length; i++)
        {
            if (sorce[i] != null)
            {
                if (sorce[i].clip != null)
                    sorce[i].clip = null;
            }
        }
    }

    public void OnSound()
    {
        for (int i = 0; i < sorce.Length; i++)
        {
            if (sorce[i] != null)
            {
                if (sorce[i].volume == 0)
                    sorce[i].volume = 1;
                else
                    sorce[i].volume = 0;
            }
        }
    }

    public void OnBackgroundSound()
    {
        if (backgrounSorce.volume == 0)
            backgrounSorce.volume = 0.7f;
        else
            backgrounSorce.volume = 0.0f;
    }
}
