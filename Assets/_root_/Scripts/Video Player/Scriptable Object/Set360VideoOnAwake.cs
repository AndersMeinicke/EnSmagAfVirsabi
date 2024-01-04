using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Set360VideoOnAwake : MonoBehaviour
{
    [SerializeField] private VideoClipSO videoClipSO;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource speechSource;
    [SerializeField] private AudioSource roomSource;
    [SerializeField] private AudioSource musicSource;


    private void Awake()
    {
        videoPlayer.clip = videoClipSO.VideoClip;
        videoPlayer.Play();
        speechSource.clip = videoClipSO.Speech;
        speechSource.PlayDelayed(2);
        roomSource.clip = videoClipSO.RoomSound;
        roomSource.Play();
        musicSource.clip = videoClipSO.MusicSound;
        musicSource.Play();
    }
}
