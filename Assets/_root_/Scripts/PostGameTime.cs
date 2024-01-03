using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PostGameTime : MonoBehaviour
{
    bool CanBeAMinute;
    public TextMeshProUGUI textMeshProUGUI;
    // Start is called before the first frame update
    void Start()
    {
        string showcaseTime;
        CanBeAMinute = true;
        float fullTime = Mathf.FloorToInt(FoundThreats.Instance.time);
        int displayedTime = (int) fullTime;
        int minutes = 0;
        int seconds = 0;
        while (CanBeAMinute)
        {
            if (displayedTime - 60 >= 0)
            {
                minutes++;
                displayedTime -= 60;
            }
            else
            {
                seconds = displayedTime;
                break;
            }
        }
        textMeshProUGUI.text += "We expected you to complete this mission in 03:00.<br><br>";
        if (seconds < 10 && minutes < 10)
        {
            textMeshProUGUI.text += $"You completed your mission in 0{minutes}:0{seconds}";
           showcaseTime  = $"0{minutes}:0{seconds}<br>";
        }
        else if(seconds < 10)
        {
            textMeshProUGUI.text += $"You completed your mission in {minutes}:0{seconds}";
            showcaseTime = $"{minutes}:0{seconds}<br>";
        }
        else if(minutes < 10)
        {
            textMeshProUGUI.text += $"You completed your mission in 0{minutes}:{seconds}";
            showcaseTime = $"0{minutes}:{seconds}<br>";
        }
        else { textMeshProUGUI.text += $"You completed your mission in {minutes}:{seconds}";
            showcaseTime = $"{minutes}:{seconds}<br>";
        }
        textMeshProUGUI.text += $"<br><br>You found {FoundThreats.Instance.threats} out of {FoundThreats.Instance.totalThreats} threats.";
        if (FoundThreats.Instance.threats == FoundThreats.Instance.totalThreats) {
            textMeshProUGUI.text += " Well done!<br>";
        }
        textMeshProUGUI.text += $"<br>You were incorrect {FoundThreats.Instance.misclicks} times.";
        CreateTimeScore(fullTime, showcaseTime);

    }
    public void CreateTimeScore(float time, string showcaseTime)
    {
        TimeScore timeScore = new TimeScore(time, showcaseTime);
        ScoreBoardManager.Instance.AddTimeScore(timeScore);
        ScoreBoardManager.Instance.DisplayScores();
    }

}
