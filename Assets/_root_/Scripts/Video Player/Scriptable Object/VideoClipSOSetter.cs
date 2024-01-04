using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

public class VideoClipSOSetter : MonoBehaviour
{
    [SerializeField] private VideoClip videoClip;
    [SerializeField] private VideoClipSO videoClipSO;
    [SerializeField] private AudioClip voiceClip;
    [SerializeField] private AudioClip roomClip;
    [SerializeField] private AudioClip musicClip;

    public void SetVideo()
    {
        videoClipSO.VideoClip = videoClip;
        videoClipSO.Speech = voiceClip;
        videoClipSO.RoomSound = roomClip;
        videoClipSO.MusicSound = musicClip;
    }

#if UNITY_EDITOR
    [ContextMenu("Get Files Automatically")]
    public void GetFiles()
    {
        //Debug.Log("Assets/_root_/Graphics/2D/Video/DFDS 4K FFMPEG Export/" + transform.parent.parent.name);
        var videoGuid = AssetDatabase.FindAssets(transform.parent.parent.name + " t:VideoClip", new[] {"Assets/_root_/Graphics/2D/Video/DFDS 4K FFMPEG Export"});
        var roomGuid = AssetDatabase.FindAssets(transform.parent.parent.name + "_ambix" + " t:AudioClip", new[] {"Assets/_root_/Audio/Background"});

        var voiceGuid = AssetDatabase.FindAssets(transform.parent.parent.name + "_voice" + " t:AudioClip", new[] {"Assets/_root_/Audio/Voice"});
        
        Debug.Log(transform.parent.parent.name);
        //Debug.Log(AssetDatabase.GUIDToAssetPath(roomGuid[0]));
        
        videoClip = AssetDatabase.LoadAssetAtPath<VideoClip>(AssetDatabase.GUIDToAssetPath(videoGuid[0]));
        voiceClip = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(voiceGuid[0]));
        roomClip = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(roomGuid[0]));

        //speech = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(roomGuid[0]));
    }
#endif

}
