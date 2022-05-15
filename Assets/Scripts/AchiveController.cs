using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AchiveController : MonoBehaviour
{
    private static AchiveController instance;
    public static AchiveController Instance => instance;

    [SerializeField] private Text messageText;

    [Header("Hend control")]
    [SerializeField] private Transform handPos;
    [SerializeField] private float distanceForMessage = 0.5f;

    [Header("Distance control")]
    [SerializeField] private Text evolutionPointText;
    [SerializeField] private MessageIf[] messageIf = new MessageIf[5];

    [Header("Spider Control")]
    private int spiderCount = 0;
    [SerializeField] private MessageIf[] minSpiderCount = new MessageIf[5];

    [Header("Fail controll")]
    private int branchCount = 0;
    [SerializeField] private MessageIf[] minBranchCount = new MessageIf[5];

    private void Awake()
    {
        instance = this;
        messageText.gameObject.SetActive(false);
    }

    public void UpdateSpider()
    {
        spiderCount += 1;

        if (spiderCount > 5)
        {

        }
    }


}

[Serializable]
public class MessageIf
{
    public int distance;
    public string text;
}
