using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "VideoClipSO", menuName = "Scriptable Objects/VideoClip", order = 1)]
public class VideoClipSO : ScriptableObject
{
    [SerializeField] private VideoClip videoClip;
    [SerializeField] private AudioClip speech;
    [SerializeField] private AudioClip roomSound;
    [SerializeField] private AudioClip musicSound;

    public VideoClip VideoClip
    {
        get => videoClip;
        set => videoClip = value;
    }

    public AudioClip Speech
    {
        get => speech;
        set => speech = value;
    }

    public AudioClip RoomSound
    {
        get => roomSound;
        set => roomSound = value;
    }

    public AudioClip MusicSound
    {
        get => musicSound;
        set => musicSound = value;
    }
}
