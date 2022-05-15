using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{
    public void SceneStop()
    {
        Time.timeScale = 0;
    }

    public void SceneStart()
    {
        Time.timeScale = 1;
    }
}
