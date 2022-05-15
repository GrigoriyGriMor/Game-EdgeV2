using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //если хотим использовать Array.resize, то нужно подключать эту библиотеку
using UnityEngine.UI;


public class ControlResultPlayer : MonoBehaviour
{
    public int colMass = 1;
    public float[] LastResults = new float[1];

    public float bestResult = 1.0f;

    public Text TimerText;

    private void Awake()
    {
        TimerText = GameObject.Find("Canvas/Result").GetComponent<Text>();//определяем канвас, далее он будет передаваться из класса InstantiateGameObject
    }


    private void Update()
    {
        TimerText.text = "Лучшее время: " + bestResult.ToString();

        DontDestroyOnLoad(gameObject);
    }

    public void newElement(float _newElement)//получаем крайний результат и сравниваем с предыдущим лучшим
    {
        if (((colMass - 1) >= 0 && LastResults[colMass - 1] != 0.0f) || (LastResults[colMass - 1] == 0.0f && colMass == 1))
        {
            colMass += 1;
            Array.Resize(ref LastResults, colMass);
        }

        for (int i = 0; i <= LastResults.Length - 1; ++i)// записываем крайний результат
        {
                LastResults[i] = _newElement;
            if (LastResults[i] > bestResult)
            {
                bestResult = LastResults[i];
            }
        }
    }




}
