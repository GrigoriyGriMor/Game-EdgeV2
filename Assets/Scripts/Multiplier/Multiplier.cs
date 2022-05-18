using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Multiplier : MonoBehaviour
{
    public int XCounter { get; private set; }

    [SerializeField]private float defaultTimer;
    [SerializeField] private Animator timerImageAnim;
    [SerializeField] private TMP_Text multiplierText;
    private float timer;
    private static Multiplier instance;
    public static Multiplier Instance => instance;

    private void Start()
    {
        instance = this;
        XCounter = 1;
        timer = defaultTimer;

    }
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer<0)
        {
            XCounter = 1;
            multiplierText.text = XCounter.ToString();
        }
    }

    public void CatchBonus()
    {
        if(timer>0)
        {
            XCounter++;
        }
        timer = defaultTimer;
        multiplierText.text = XCounter.ToString();
        GameLevel.Instance.UpgradePoint(10 * XCounter);
        timerImageAnim.SetTrigger("RestartTimer");
        
    }
}
