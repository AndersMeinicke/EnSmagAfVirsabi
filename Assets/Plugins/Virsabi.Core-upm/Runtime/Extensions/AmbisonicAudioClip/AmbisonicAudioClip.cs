using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// TODO: make property drawer
/// </summary>
/// 

[Serializable]
public class AmbisonicAudioClip : ISerializationCallbackReceiver
{
    [SerializeField]
    private AudioClip clip;

    public AudioClip Clip
    {
        get => clip; 
        set
        {
            clip = value;
            ValidateClip();
        }
    }

    public void OnAfterDeserialize()
    {
        
    }

    public void OnBeforeSerialize()
    {
        ValidateClip();
    }

    private void ValidateClip()
    {
        if(clip)
            if (!Clip.ambisonic)
                Debug.LogError("Clip " + clip.name + " is not ambisonic!");
    }
}
