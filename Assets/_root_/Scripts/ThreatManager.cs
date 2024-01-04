using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatManager : MonoBehaviour
{
    [SerializeField] private int totalThreats;
    [SerializeField] private int scene;
    [SerializeField] private FadeInOut fadeInOut;
    [SerializeField] private TimeManager timeManager;
    private bool noMultiLoad = true;
    private int threatsFound;
    private Sequence sequence;
    
    private void Update()
    {
       // if(threatsFound == totalThreats && noMultiLoad == true)
     //   {
     //       timeManager.MakeTimerStop();
      //      sequence = DOTween.Sequence();
     ////      sequence.AppendInterval(7);
      //      sequence.AppendCallback(() => fadeInOut.levelLoader(scene));
     //       noMultiLoad = false;
     //       sequence.Play();
     //   }
    }

    // Start is called before the first frame update
    public int getThreatsFound()
    { return threatsFound; }
    public void setThreatsFound()
    {
        threatsFound++;
    }
    public int getTotalThreats()
    {
        return totalThreats;
    }
}
