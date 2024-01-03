using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickCounter : MonoBehaviour
{

    [SerializeField]
    public int clicks;

    public int neededClicks;

    public UnityEvent OnNeededClicksReached;

    public bool removeClicksOverTime;

    public float removeAClickEveryXSecond = 1f;

    public void clicked()
    {
        CancelInvoke();
        Invoke("ResetClicks", 2);
        clicks++;
        if (clicks >= neededClicks)
            OnNeededClicksReached.Invoke();
    }

    public void ResetClicks()
    {
        clicks = 0;
    }

    float timer;

    private void Update()
    {
        if (!removeClicksOverTime)
            return;

        timer += Time.deltaTime;

        if(timer > removeAClickEveryXSecond)
        {
            timer = 0;
            clicks--;
            if (clicks < 0)
                clicks = 0;
        }
    }

}
