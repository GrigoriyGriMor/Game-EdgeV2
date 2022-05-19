using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Multiplier : MonoBehaviour
{
    public int XCounter { get; private set; }
    [SerializeField] private int MaxXCounter;
    [SerializeField]private float defaultTimer;
    [SerializeField] private Animator timerImageAnim;
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private CanvasGroup canvasGroup;
    private float timer;
    private GameObject timerImage;
    private static Multiplier instance;
    public static Multiplier Instance => instance;

    private void Start()
    {
        instance = this;
        XCounter = 0;
        timer = defaultTimer;
        timerImage = timerImageAnim.gameObject;
        timerImage.SetActive(false);
        SetCanvas(false);
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer<0)
        {
            timerImage.SetActive(false);
            XCounter = 0;
            multiplierText.text = XCounter.ToString();
            SetCanvas(false);
        }
    }

    public void GetBonus(int points)
    {
        if(XCounter<MaxXCounter)
            XCounter++;
        
        timer = defaultTimer;
        multiplierText.text = XCounter.ToString();
        GameLevel.Instance.UpgradePoint(points * XCounter);
        timerImage.SetActive(false);
        //timerImageAnim.SetTrigger("RestartTimer");
        timerImage.SetActive(true);
        SetCanvas(true);
    }
    public void SetCanvas(bool SetTo)
    {
        if(SetTo)
            canvasGroup.alpha = 1;
        else
            canvasGroup.alpha = 0;
    }
}
