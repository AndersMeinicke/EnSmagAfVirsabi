using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeShowcase : MonoBehaviour
{
    public TextMeshProUGUI TimeText;
    public GameObject TimeTaker;
    public bool stopCounting = false;
    TimeManager TimeManager;
    private bool CanBeAMinute;
    private int displayedTime = 0;
    private void Start()
    {
       TimeManager = TimeTaker.GetComponent<TimeManager>();
    }
    private void Update()
    {
        int fullTime = 180;
        CanBeAMinute = true;
        if(stopCounting == false) {
         displayedTime = Mathf.FloorToInt(TimeManager.GetTimeElapsed());
        }
        int minutes = 0;
        int seconds = 0;
        fullTime -= displayedTime;
        while(CanBeAMinute)
        {
            if(fullTime-60 >= 0)
            {
                minutes++;
                fullTime -= 60;
            }
            else
            {
                seconds = fullTime;
                break;
            }
        }
        if (seconds < 10)
        {
            TimeText.text = $"{minutes}:0{seconds}";
        }
        else {
            TimeText.text = $"{minutes}:{seconds}";
        }
    }
}
