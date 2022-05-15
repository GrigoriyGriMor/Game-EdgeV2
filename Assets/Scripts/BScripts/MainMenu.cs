using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace lastGame
{
    public class MainMenu : MonoBehaviour
    {
        public AudioSource[] audioController = new AudioSource[2];
        public AudioClip[] audioSound = new AudioClip[2];

        private float _activeLoading = 0.0f;
        private bool _activeButton = false;
        private bool exitGame = false;

        [SerializeField]
        private GUIStyle[] buttonММ = new GUIStyle[2];

        //при старте определим размер экрана и кнопки соответственно
        private float _buttonStart_Width = 0.0f;
        private float _buttonStart_Height = 0.0f;
        private float _buttonStartposition_Width = 0.0f;
        private float _buttonStartposition_Height = 0.0f;

        private float _buttonExitposition_Width = 0.0f;
        private float _buttonExitposition_Height = 0.0f;


        private void Start()
        {

            // подсчет позиции и размеров кнопки старт
            _buttonStart_Width = (Screen.width / 5) * 1;
            _buttonStart_Height = (Screen.height / 6.5f) * 1;
            _buttonStartposition_Width = Screen.width / 2 - _buttonStart_Width / 2;
            _buttonStartposition_Height = Screen.height / 2.5f - _buttonStart_Height / 2;

            // подсчет позиции и размеров кнопки выход
            _buttonExitposition_Width = Screen.width / 2 - _buttonStart_Width / 2;
            _buttonExitposition_Height = _buttonStartposition_Height + (_buttonStart_Height + _buttonStart_Height / 6);

            audioController[0].clip = audioSound[0];//присваеваем контроллеру объект, от которого будет исходить звук
        }

        private void FixedUpdate()
        {
            if (_activeLoading < 3.0f)//показываем название игры в течении трех секунд после старта игры
            {
                _activeLoading += 1 * Time.deltaTime;
            }
            else
            {
                _activeButton = true;

                if (!audioController[0].loop)
                {
                    audioController[0].Play();
                    audioController[0].loop = true;
                }
            }
        }

        private void OnGUI()
        {
            if (_activeButton)//если меню активно
            {
                if (!exitGame)
                {
                    //рисуем кнопку старт
                    if (GUI.Button(new Rect(_buttonStartposition_Width, _buttonStartposition_Height, _buttonStart_Width, _buttonStart_Height), "ИГРАТЬ"/*, buttonММ[0]*/))
                    {
                        audioController[1].Stop();//останавливаем клип, если на момент вызова функции он еще проигрывается от предыдущего вызова
                        audioController[1].PlayOneShot(audioSound[1]);
                        audioController[0].Stop();
                        SceneManager.LoadScene(1);
                    }

                    //рисуем кнопку выход
                    if (GUI.Button(new Rect(_buttonExitposition_Width, _buttonExitposition_Height, _buttonStart_Width, _buttonStart_Height), "ВЫХОД"/*, buttonММ[1]*/))
                    {
                        audioController[1].Stop();//останавливаем клип, если на момент вызова функции он еще проигрывается от предыдущего вызова
                        audioController[1].PlayOneShot(audioSound[1]);

                        exitGame = true;
                    }
                }
                else
                {
                    GUI.Box(new Rect(Screen.width / 2 - 170, Screen.height / 2 - 100, 340, 200), "", buttonММ[2]);

                    if (GUI.Button(new Rect(Screen.width / 2 - 125, Screen.height / 2, 100, 50), "", buttonММ[3]))
                    {
                        audioController[1].Stop();//останавливаем клип, если на момент вызова функции он еще проигрывается от предыдущего вызова
                        audioController[1].PlayOneShot(audioSound[1]);

                        Time.timeScale = 1;
                        Application.Quit();
                    }

                    if (GUI.Button(new Rect(Screen.width / 2 + 25, Screen.height / 2, 100, 50), "", buttonММ[4]))
                    {
                        audioController[1].Stop();//останавливаем клип, если на момент вызова функции он еще проигрывается от предыдущего вызова
                        audioController[1].PlayOneShot(audioSound[1]);

                        exitGame = false;
                    }
                }
            }
            else
            if (!_activeButton)
            {
                GUI.Box(new Rect(Screen.width / 2 - 125, Screen.height / 2 - 70, 250, 25), "   Приключенческая Игра");

                GUI.Box(new Rect(Screen.width / 2 - 125, Screen.height / 2 - 25, 250, 25), " The Monster From The Well");
                GUI.Box(new Rect(Screen.width / 2 - 125, Screen.height / 2 + 1, 250, 25), "Или - НА ДНО И ОБРАТНО! :D");
            }
        }
    }
}
