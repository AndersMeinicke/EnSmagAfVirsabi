using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static OVRInput;

public class ScoreBoardManager : MonoBehaviour
{
    public static ScoreBoardManager Instance { get; private set; }
    [SerializeField] private static List<TimeScore> BestTimes;
    [SerializeField] private static TextMeshProUGUI textMeshProUGUI;
    public Controller RightHandController;
    private static TimeScore MostRecentTime;
    private void Awake()
    {
        if (Instance == null)
        {
            BestTimes = new List<TimeScore>();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.One, RightHandController))
        {
            ShowLatestTime();
        }
    }
    public void AddTimeScore(TimeScore timeScore)
    {
        MostRecentTime = timeScore;
        BestTimes.Add(timeScore);
    }
    public void DisplayScores()
    {
        textMeshProUGUI = GameObject.Find("ShowBoard").GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = null;
        List<TimeScore> sortedScores = BestTimes.OrderBy(obj => obj.GetTime()).ToList();
        List<TimeScore> lowestScores = sortedScores.Take(10).ToList();
        int placement = 0;
        foreach (var score in lowestScores)
        {  
            placement++;
            if( placement == 1 ) {
                textMeshProUGUI.text += $"{placement}. {score.GetShowcaseTime()}";
            }
            else { textMeshProUGUI.text += $"<br>{placement}. {score.GetShowcaseTime()}"; }
            
        }
    }
    public void ShowLatestTime()
    {
        textMeshProUGUI = GameObject.Find("DevTimeChecker").GetComponent<TextMeshProUGUI>();
        if(textMeshProUGUI.text == null) { 
        textMeshProUGUI.text = $"{MostRecentTime.GetShowcaseTime()}";
    
        }
        else
        {
            textMeshProUGUI.text = null;
        }
    }
}
