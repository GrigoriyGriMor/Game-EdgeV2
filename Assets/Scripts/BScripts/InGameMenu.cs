using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public bool _victory = false;
    public bool _playerLive = true;

    private KeyCode _esc = KeyCode.Escape;
    private bool ActiveMIG = false; //определяем должно быть активно Menu In Game - MIG
    private bool exit = false;

    private float _timer = 0.0f;

    public AudioSource[] audioButtonController = new AudioSource[2];
    public AudioClip[] audioButtonSound = new AudioClip[2];

    public GUIStyle planeMenu;//Фон внутриигрового меню
    public GUIStyle buttonGame;// продолжить игру
    public GUIStyle buttonGTMM; //кнопка расшифровывается как Go To Main Menu, переводит нас в главное меню игры
    public GUIStyle buttonExit;// Выход из игры
    public GUIStyle gameOver;
    public GUIStyle victoty;

    //ниже стили для меню выбора действий "да или нет"
    public GUIStyle planeExit;
    public GUIStyle buttonYes;
    public GUIStyle buttonNo;
    public Text TimerText;

    private void Start()
    {
        _timer = 0.0f;
        if (audioButtonController[1].loop)
        {
            audioButtonController[1].PlayOneShot(audioButtonSound[1]);
            audioButtonController[1].loop = true;
        }

        TimerText = GameObject.Find("Canvas/Text").GetComponent<Text>();
    }

    void Update()
    {
        if (Input.GetKeyDown(_esc) && !ActiveMIG)
        {
            ActiveMIG = true;
        }
        else
        {
            if (Input.GetKeyDown(_esc) && ActiveMIG)
            {
                ActiveMIG = false;
                exit = false;
            }
        }

        if (ActiveMIG)//замедляем время пока активно внутреигровое меню
        {
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;

        if (!ActiveMIG && !_victory && _playerLive)//игровой таймер, показывает за сколько игрок проходит уровень
        {
            _timer += Time.deltaTime;
            TimerText.text = "Твое время: " + Mathf.Floor(_timer).ToString();
        }
    }

    private void OnGUI()
    {
        if (ActiveMIG && !_victory && _playerLive)
        {
            if (!exit)
            {
                GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 200, 300, 400), "", planeMenu);
                if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 125, 200, 50), "Продолжить"))
                {
                    audioButtonController[0].Stop();//останавливаем клип, если на момент вызова функции он еще проигрывается от предыдущего вызова
                    audioButtonController[0].PlayOneShot(audioButtonSound[0]);

                    ActiveMIG = false;
                }
                if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "В главное меню"))
                {
                    audioButtonController[0].Stop();//останавливаем клип, если на момент вызова функции он еще проигрывается от предыдущего вызова
                    audioButtonController[0].PlayOneShot(audioButtonSound[0]);

                    Time.timeScale = 1;
                    ActiveMIG = false;
                    SceneManager.LoadScene(0);
                }
                if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 75, 200, 50), "Выход"))
                {
                    audioButtonController[0].Stop();//останавливаем клип, если на момент вызова функции он еще проигрывается от предыдущего вызова
                    audioButtonController[0].PlayOneShot(audioButtonSound[0]);

                    exit = true;
                }
            }
            else
            {
                GUI.Box(new Rect(Screen.width / 2 - 170, Screen.height / 2 - 100, 340, 200), "", planeExit);

                if (GUI.Button(new Rect(Screen.width / 2 - 125, Screen.height / 2, 100, 50), "", buttonYes))
                {
                    audioButtonController[0].Stop();//останавливаем клип, если на момент вызова функции он еще проигрывается от предыдущего вызова
                    audioButtonController[0].PlayOneShot(audioButtonSound[0]);

                    Time.timeScale = 1;
                    Application.Quit();
                }

                if (GUI.Button(new Rect(Screen.width / 2 + 25, Screen.height / 2, 100, 50), "", buttonNo))
                {
                    audioButtonController[0].Stop();//останавливаем клип, если на момент вызова функции он еще проигрывается от предыдущего вызова
                    audioButtonController[0].PlayOneShot(audioButtonSound[0]);

                    exit = false;
                }
            }
        }
        else
        {
            /*if (GUI.RepeatButton(new Rect(0, 0, Screen.width / 6, Screen.height), "<<<"))
            {
                GameObject.Find("Player").GetComponent<PlayerController>().LeftActive = true;
            }
            if (GUI.RepeatButton(new Rect((Screen.width - (Screen.width / 6)), 0, Screen.width / 6, Screen.height), ">>>"))
            {
                GameObject.Find("Player").GetComponent<PlayerController>().RightActive = true;
            }
            if (GUI.Button(new Rect(0, (Screen.height - (Screen.height / 6)), Screen.width, Screen.height / 6), "Jump"))
            {
                GameObject.Find("Player").GetComponent<PlayerController>().JumpActive = true;
            }*/
        }

        if (_victory)
        {
            Time.timeScale = 0.5f;//замедляем время в игре

            GUI.Box(new Rect(Screen.width / 2 - 85, Screen.height / 2 - 300, 170, 100), "", victoty);

            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 150, 300, 300), "", planeMenu);
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 125, 200, 50), "Еще раз"))
            {
                audioButtonController[0].Stop();//останавливаем клип, если на момент вызова функции он еще проигрывается от предыдущего вызова
                audioButtonController[0].PlayOneShot(audioButtonSound[0]);

                GameObject.Find("ResultController").GetComponent<ControlResultPlayer>().newElement(_timer);

                SceneManager.LoadScene(1);
            }

            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Выйти"))
            {
                audioButtonController[0].Stop();//останавливаем клип, если на момент вызова функции он еще проигрывается от предыдущего вызова
                audioButtonController[0].PlayOneShot(audioButtonSound[0]);

                Time.timeScale = 1;
                Application.Quit();
            }
        }

        if (!_playerLive)
        {
            Time.timeScale = 0.5f;

            GUI.Box(new Rect(Screen.width / 2 - 120, Screen.height / 2 - 300, 240, 120), "", gameOver);

            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 150, 300, 300), "", planeMenu);
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 125, 200, 50), "Заново"))
            {
                audioButtonController[0].Stop();//останавливаем клип, если на момент вызова функции он еще проигрывается от предыдущего вызова
                audioButtonController[0].PlayOneShot(audioButtonSound[0]);

                SceneManager.LoadScene(1);
            }

            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Выйти"))
            {
                audioButtonController[0].Stop();//останавливаем клип, если на момент вызова функции он еще проигрывается от предыдущего вызова
                audioButtonController[0].PlayOneShot(audioButtonSound[0]);

                Time.timeScale = 1;
                Application.Quit();
            }
        }
    }


}