using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScore : MonoBehaviour
{
    private float time;
    private string showcaseTime;
    
    public TimeScore(float time, string showcaseTime)
    {
        this.time = time;
        this.showcaseTime = showcaseTime;
    }

    public float GetTime()
    {
        return time;
    }
    public string GetShowcaseTime()
    {
        return showcaseTime;
    }
}
