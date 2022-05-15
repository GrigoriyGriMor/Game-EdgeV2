using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioClip background;
    [SerializeField] private GameObject soundManager;

    public void Awake()
    {
        if (soundManager != null)
        {
            SoundManager sm = FindObjectOfType<SoundManager>();
            if (sm == null)
                Instantiate(soundManager, new Vector2(0, 0), Quaternion.identity);
        }
    }

    private void Start()
    {
        if (SoundManager.Instance) SoundManager.Instance.BackgroundClipPlay(background);
    }

    public void PlayGame()
    {
        if (SoundManager.Instance) SoundManager.Instance.ClearSound();
        SceneManager.LoadScene(1); //SceneManager.LoadScene("level_1");
    }

    public void DeveloperTeam()
    {
        if (SoundManager.Instance) SoundManager.Instance.ClearSound();
        SceneManager.LoadScene(2); //SceneManager.LoadScene("level_1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        if (SoundManager.Instance) SoundManager.Instance.ClearSound();
        SceneManager.LoadScene(0); //SceneManager.LoadScene("level_1");
    }

    public void UseSound()
    {
        if (SoundManager.Instance) SoundManager.Instance.OnSound();
    }

    public void UseBackground()
    {
        if (SoundManager.Instance) SoundManager.Instance.OnBackgroundSound();
    }
}
