using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Windows;
using Object = UnityEngine.Object;

//TODO: remove static fields so serialization works
//TODO: Playing one frame backwards needs to move mouse to repaint
[Serializable]
public class VideoThumbnailMakerEditor : EditorWindow
{
    private Object videoObject;
    private Object rawImageObject;
    private VideoPlayer videoPlayer;
    private RawImage rawImage; // todo: reference lost during playmode

    
    private int thumbnailWidth = 100;
    private int thumbnailHeight = 100;

    private static float downSampleFactor = 0.5f; // todo: add option for downsampling current thumbnail dimensions
    
    private int videoDisplayWidth = 300;
    private int videoDisplayHeight = 200;

    private string thumbnailDirectory = "Assets/_root_/Graphics/VideoThumbnails/";

    private bool play, pause, stop, playing;
    private bool playOneFrameForward, playOneFrameBackwards;

    private static Texture previewTexture;
    private static GUIStyle videoTextureStyle;
    private static int encodingChosen;
    private string[] encodingLabels = {"PNG", "JPG"};

    private bool SaveThumbnail(out Texture2D exportedTexture, out string fileLocation)
    {
        fileLocation = String.Empty;

        int downSampledThumbnailWidth = (int) (thumbnailWidth * downSampleFactor);
        int downSampledThumbnailHeight = (int) (thumbnailHeight * downSampleFactor);

        exportedTexture = new Texture2D(downSampledThumbnailWidth, downSampledThumbnailHeight, TextureFormat.RGB24, false);
        RenderTexture currentRenderTexture = RenderTexture.active; // set this back to the active later
        RenderTexture myRenderTexture = new RenderTexture(downSampledThumbnailWidth, downSampledThumbnailHeight, 32);
        
        Graphics.Blit(previewTexture, myRenderTexture);
        
        RenderTexture.active = myRenderTexture;
        
        exportedTexture.ReadPixels(new Rect(0,0, downSampledThumbnailWidth, downSampledThumbnailHeight),0,0);
        exportedTexture.Apply();

        RenderTexture.active = currentRenderTexture;
        
        // Texture2D resizedThumbnail = new Texture2D(thumbnailWidth, thumbnailHeight, TextureFormat.RGB24, false);
        // resizedThumbnail.SetPixels(exportedTexture.GetPixels());
        
        // bool success = exportedThumbnail.Resize(thumbnailWidth, thumbnailHeight, TextureFormat.RGB24, false); // does not work
        byte[] array = encodingChosen == 0 ? exportedTexture.EncodeToPNG() : exportedTexture.EncodeToJPG();

        if (Directory.Exists(thumbnailDirectory))
        {
            string encodingName = encodingChosen == 0 ? ".png" : ".jpg";
            string thumbnailName = videoPlayer.clip.name + "_thumbnail" + encodingName;
            
            fileLocation = thumbnailDirectory + thumbnailName;
            
            System.IO.File.WriteAllBytes(fileLocation, array);
            Debug.Log("Thumbnail " + thumbnailName + " saved to " + thumbnailDirectory);
            AssetDatabase.Refresh();

            
            return true;
        }
        else
        {
            Debug.LogError("Directory does not exist");
            return false;
        }
        
    }

    private void SetThumbnailSize()
    {
        // AspectRatioFitter fitter = rawImage.GetComponent<AspectRatioFitter>();
        if (!rawImage)
        {
            Debug.LogError("Raw image not specified");
            return;
        }
        
        thumbnailWidth = (int) rawImage.rectTransform.rect.width;
        thumbnailHeight = (int) rawImage.rectTransform.rect.height;
        
        // EditorUtility.SetDirty(this);
    }
    
    
    [MenuItem("Virsabi Tools/Utilities/Thumbnail Maker")]
    public static void Init()
    {
        GetWindow(typeof(VideoThumbnailMakerEditor));
        
        encodingChosen = 0;
    }

    private void PingObject(string fileLocation)
    {
        EditorUtility.FocusProjectWindow();
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Texture>(fileLocation));
    }

    private void ThumbnailMakerGUI()
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider); // split line
        EditorGUILayout.LabelField("Thumbnail maker", EditorStyles.boldLabel);
        
        thumbnailWidth = EditorGUILayout.IntField("Thumbnail Width",thumbnailWidth);
        thumbnailHeight = EditorGUILayout.IntField("Thumbnail Height",thumbnailHeight);
        
        rawImageObject = EditorGUILayout.ObjectField(rawImageObject, typeof(RawImage), true);
        rawImage = rawImageObject as RawImage;
        
        if (rawImage && GUILayout.Button("Use raw image size"))
        {
            SetThumbnailSize();
        }
        
        
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Encoding");
            encodingChosen = GUILayout.SelectionGrid(encodingChosen, encodingLabels, 2, new GUIStyle(GUI.skin.button));
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
            thumbnailDirectory = EditorGUILayout.TextField("Thumbnail Directory",thumbnailDirectory);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Export thumbnail from video"))
            {
                Texture2D exportedTexture;
                string fileLocation;
                SaveThumbnail(out exportedTexture, out fileLocation);
                
                // focus project to the created thumbnail
                PingObject(fileLocation);
            }

            if (GUILayout.Button("Export and set to raw image"))
            {
                Texture2D exportedTexture;
                string fileLocation;
                
                SaveThumbnail(out exportedTexture, out fileLocation);
                
                if (rawImage == null)
                {
                    Debug.LogError("Raw image not set");
                    return;
                }
                
                // needed to properly set the raw image texture location
                rawImage.texture = AssetDatabase.LoadAssetAtPath<Texture>(fileLocation);
                PingObject(fileLocation);
            }
        EditorGUILayout.EndHorizontal();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Video Display", EditorStyles.boldLabel);
        
        videoObject = EditorGUILayout.ObjectField(videoObject, typeof(VideoPlayer), true);
        if (!videoObject) return;
        videoPlayer = videoObject as VideoPlayer;
        videoPlayer.isLooping = false;

        ThumbnailMakerGUI();


        if (play && !playing)
        {
            playing = true;
            videoPlayer.Play();
        }
        else if (!play && playing)
        {
            playing = false;
            videoPlayer.Pause();
        }
        else if (stop)
        {
            videoPlayer.Stop();
            playing = false;
        }
        else if (playOneFrameForward)
        {
            videoPlayer.frame++;
            playOneFrameForward = false;
            Repaint();
        }
        else if (playOneFrameBackwards)
        {
            videoPlayer.frame--;
            playOneFrameBackwards = false;
            Repaint();
        }
        
        // draw video texture
        videoTextureStyle = new GUIStyle(GUI.skin.label);
        videoTextureStyle.fixedHeight = videoDisplayHeight;
        // videoTextureStyle.fixedWidth = videoDisplayWidth;
        videoTextureStyle.alignment = TextAnchor.MiddleCenter;
        

        if (videoPlayer != null)
        {
            if (videoPlayer.texture != null && playing)
            {
                previewTexture = videoPlayer.texture;
                Repaint();
            }
        }
        
        ////////// VIDEO DISPLAY AREA ////////// 
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Video", EditorStyles.boldLabel);
        
        GUILayout.Label(previewTexture, videoTextureStyle, GUILayout.ExpandWidth(true));
        // GUILayout.Label("Test", videoTextureStyle, GUILayout.ExpandWidth(true));

        ////////// BUTTONS AREA ////////// 
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
        
            // video buttons
            if (GUILayout.Button("Play"))
            {
                play = true;
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
        
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
        
            // step buttons
            if (GUILayout.Button("Play One Frame Forward"))
            {
                playOneFrameForward = true;
                play = false;
                pause = false;
                stop = false;
                
                Repaint(); // maybe not needed
            }
            
            if (GUILayout.Button("Play One Frame Backwards"))
            {
                playOneFrameBackwards = true;
                play = false;
                pause = false;
                stop = false;
                
                Repaint(); // maybe not needed
            }
        
            EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
    }
}
