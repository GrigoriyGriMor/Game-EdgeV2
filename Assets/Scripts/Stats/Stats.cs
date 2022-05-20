using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum StatsTypes
{
    Distance,
    Danger,
    Mistake,
    Time,
    Best
}

[System.Serializable]
public class ActionStat
{   
    public string[] lines;
    public int currentLine;
    public int[] lineDisplayAt;
    public int currentStatCount;
    public bool avalible = true;
}

public class Stats : MonoBehaviour
{
    
    public int TotalMistake { get; private set; }
    public int TotalDangerousMoments { get; private set; }
    [SerializeField] private ActionStat distanceLines;
    [SerializeField] private ActionStat dangerLines;
    [SerializeField] private ActionStat mistakeLines;
    [SerializeField] private GameObject textObj;
    [SerializeField] private Text summaryText;
    private Text text;
    

    private void Start()
    {
        text = textObj.GetComponent<Text>();     
    }

    private void Update()
    {
        if(distanceLines.currentLine<distanceLines.lineDisplayAt.Length && GameLevel.Instance.GetEvolutionPoint()> distanceLines.lineDisplayAt[distanceLines.currentLine]  )
        {
            AchivedSomething(StatsTypes.Distance);
        }
        
    }

    public void AchivedSomething(StatsTypes type)
    {
        if(!textObj.activeInHierarchy)
        {
            if (type == StatsTypes.Distance)
            {
                text.text = GetNextLine(StatsTypes.Distance);
            }
            else if (type == StatsTypes.Danger)
            {
                text.text = GetRandomLineShow(StatsTypes.Danger);
            }
            else if (type == StatsTypes.Mistake)
            {
                text.text = GetNextLine(StatsTypes.Mistake);

            }
            textObj.SetActive(true);
        }
        
    }

    public void SetFinalStats()
    {
        Dictionary<StatsTypes, string> sum = DoSummary();
        summaryText.text = ("Evolution: " + sum[StatsTypes.Distance] + "\n" + "\n" +
            "DangerMoment: " + sum[StatsTypes.Danger] + "\n" + "\n" +
            "MistakeMoment: " + sum[StatsTypes.Mistake] + "\n" + "\n" +
            "TimeInGame: " + sum[StatsTypes.Time] + "\n" + "\n" +
            "BestResult: " + sum[StatsTypes.Best]);
    }
    public void DangerousMoment()
    {
        TotalDangerousMoments++;
    }

    public void MistakeMoment()
    {
        TotalMistake++;
    }

    private Dictionary<StatsTypes, string> DoSummary()
    {
        Dictionary<StatsTypes, string> summary = new Dictionary<StatsTypes, string>();
        summary.Add(StatsTypes.Distance, GameLevel.Instance.GetEvolutionPoint().ToString());
        summary.Add(StatsTypes.Danger, TotalDangerousMoments.ToString());
        summary.Add(StatsTypes.Mistake, TotalMistake.ToString());
        summary.Add(StatsTypes.Time, GameLevel.Instance.totalTimePlaying.ToString());
        summary.Add(StatsTypes.Best, PlayerPrefs.GetInt("BestResult").ToString());
        return summary;
    }

 
    private string GetNextLine(StatsTypes type)
    {
        if (type == StatsTypes.Distance)
        {
            if (distanceLines.currentLine < distanceLines.lines.Length)
                distanceLines.currentLine++;
            else
                distanceLines.avalible = false;
            return distanceLines.lines[distanceLines.currentLine-1];
            
        }
        else if (type == StatsTypes.Danger)
        {
            if (dangerLines.currentLine < dangerLines.lines.Length)
                dangerLines.currentLine++;
            else
                dangerLines.avalible = false;
            return dangerLines.lines[dangerLines.currentLine-1];
        }
        else if (type == StatsTypes.Mistake)
        {
            if (mistakeLines.currentLine < mistakeLines.lines.Length)
                mistakeLines.currentLine++;
            else
                mistakeLines.avalible = false;
            return mistakeLines.lines[mistakeLines.currentLine-1];
        }
        return "NOT MATCHING STATSTYPES";
    }
    private string GetRandomLineShow(StatsTypes type)
    {
        if (type == StatsTypes.Distance)
        {
            return distanceLines.lines[Random.Range(0, distanceLines.lines.Length)];
        }
        else if (type == StatsTypes.Danger)
        {
            return dangerLines.lines[Random.Range(0, dangerLines.lines.Length)];
        }
        else if (type == StatsTypes.Mistake)
        {
            return mistakeLines.lines[Random.Range(0, mistakeLines.lines.Length)];
        }
        return "NOT MATCHING STATSTYPES";
    }
}
