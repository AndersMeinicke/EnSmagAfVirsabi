using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



public class AudioMixerController : MonoBehaviour
{
    public AudioMixerSnapshot audioOn;
    public AudioMixerSnapshot audioOff;



    // Start is called before the first frame update
    void Start()
    {
        audioOn.TransitionTo(2); // 2 is the transition time.
    }



   public void turnOff(float f)
    {
        audioOff.TransitionTo(f);
    }
}