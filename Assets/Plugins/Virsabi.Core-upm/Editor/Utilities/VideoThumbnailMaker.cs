using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoThumbnailMaker : MonoBehaviour //todo: move to custom editor window and not inspector
{
    [HideInInspector]
    public VideoPlayer video;

    [SerializeField] private RawImage rawImageForThumbnail;

    [SerializeField] private int thumbnailWidth;
    [SerializeField] private int thumbnailHeight;
    
    [SerializeField] private int videoDisplayWidth;
    [SerializeField] private int videoDisplayHeight;

    [SerializeField] private string thumbnailDirectory;
}

#if UNITY_EDITOR
[CustomEditor(typeof(VideoThumbnailMaker))]
public class PlayVideoInEditModeInspector : Editor
{
    private SerializedProperty videoProperty;
    private SerializedProperty rawImageProperty;
    
    private SerializedProperty thumbnailWidthProperty;
    private SerializedProperty thumbnailHeightProperty;
    private SerializedProperty thumbnailDirectoryString;
    
    private SerializedProperty videoDisplayWidthProperty;
    private SerializedProperty videoDisplayHeightProperty;
    
    private VideoPlayer myVideoPlayer;
    
    
    private Texture previewTexture;
    private bool play, pause, stop, playing;
    private bool playOneFrameForward, playOneFrameBackwards;
    
    private void OnEnable()
    {
        myVideoPlayer = (target as VideoThumbnailMaker).video;
        
        videoProperty = serializedObject.FindProperty("video");
        rawImageProperty = serializedObject.FindProperty("rawImageForThumbnail");
        
        videoDisplayWidthProperty = serializedObject.FindProperty("videoDisplayWidth");
        videoDisplayHeightProperty = serializedObject.FindProperty("videoDisplayHeight");
        
        thumbnailWidthProperty = serializedObject.FindProperty("thumbnailWidth");
        thumbnailHeightProperty = serializedObject.FindProperty("thumbnailHeight");
        thumbnailDirectoryString = serializedObject.FindProperty("thumbnailDirectory");
        
        previewTexture = new Texture2D(thumbnailWidthProperty.intValue,thumbnailHeightProperty.intValue);
    }

    public override void OnInspectorGUI()
    {
        
        EditorGUILayout.LabelField("Video Display", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(videoProperty);
        EditorGUILayout.PropertyField(rawImageProperty);
        
        EditorGUILayout.PropertyField(videoDisplayWidthProperty);
        EditorGUILayout.PropertyField(videoDisplayHeightProperty);
        
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Thumbnail maker", EditorStyles.boldLabel);
        
        EditorGUILayout.PropertyField(thumbnailHeightProperty);
        EditorGUILayout.PropertyField(thumbnailWidthProperty);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(thumbnailDirectoryString);
        GUILayout.Button("Save");
        EditorGUILayout.EndHorizontal();
        

        if (play && !playing)
        {
            playing = true;
            myVideoPlayer.Play();
        }
        else if (!play && playing)
        {
            playing = false;
            myVideoPlayer.Pause();
        }
        else if (stop)
        {
            myVideoPlayer.Stop();
            playing = false;
        }
        else if (playOneFrameForward)
        {
            myVideoPlayer.frame++;
            playOneFrameForward = false;
        }
        else if (playOneFrameBackwards)
        {
            myVideoPlayer.frame--;
            playOneFrameBackwards = false;
        }
        
        // draw video texture

        GUIStyle style = new GUIStyle();
        style.fixedHeight = videoDisplayHeightProperty.intValue;
        style.fixedWidth = videoDisplayWidthProperty.intValue;
        style.alignment = TextAnchor.UpperCenter;
        

        if (videoProperty != null)
        {
            if (myVideoPlayer.texture != null && playing)
            {
                previewTexture = myVideoPlayer.texture;
                Repaint();
            }
        }
        
        
        ////////// VIDEO CONTROLS AREA ////////// 
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Video", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(previewTexture, style);
        EditorGUILayout.EndHorizontal();

        
        // buttons
        if (GUILayout.Button("Play"))
        {
            play = true;
            pause = false;
            stop = false;
        }
        
        if (GUILayout.Button("Play One Frame Forward"))
        {
            playOneFrameForward = true;
            play = false;
            pause = false;
            stop = false;
        }
        
        if (GUILayout.Button("Play One Frame Backwards"))
        {
            playOneFrameBackwards = true;
            play = false;
            pause = false;
            stop = false;
        }

        if (GUILayout.Button("Pause"))
        {
            pause = true;
            play = false;
            stop = false;
        }

        if (GUILayout.Button("Stop"))
        {
            pause = false;
            play = false;
            stop = true;
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif
