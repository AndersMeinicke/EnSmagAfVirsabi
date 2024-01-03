using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Virsabi.Utility;

/// <summary>
/// A scriptable object for paring and automating loading of obb files - can be extended to support other file types than .obb. 
/// The VRSceneChanger and VRSceneLoader handles loading the obb file on device, while using the scene reference in editor.
/// 
/// Could be improved by actually loading the assetbundle from disk in editor as well to better reflect the device environment (in editor the scene needs to be active in build settings to change).
/// </summary>
[CreateAssetMenu(fileName = "SceneName", menuName = "SceneRefObbPathPair", order = 0)]
public class SceneRefObbPathPair : ScriptableObject
{
    [SerializeField, ReadOnly]
    private string obbFileName;

    [SerializeField, TextArea, ReadOnly]
    private string obbFilePath;

    [SerializeField]
    private SceneReference scene;

    [SerializeField]
    private bool useCustomBundleName;

    [SerializeField, ConditionalField(nameof(useCustomBundleName))]
    private string BundleName;

    public string ObbFilePath { get => obbFilePath; private set => obbFilePath = value; }
    public SceneReference Scene { get => scene; private set => scene = value; }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(Scene != null && !useCustomBundleName)
        {
            obbFileName = AssetBundleUtilities.GenerateObbFileName(Scene.SceneName.ToLower());
            obbFilePath = AssetBundleUtilities.GenerateFullObbFilePath(Scene.SceneName.ToLower());
        }
        else if (Scene != null)
        {
            obbFileName = AssetBundleUtilities.GenerateObbFileName(BundleName);
            obbFilePath = AssetBundleUtilities.GenerateFullObbFilePath(BundleName);
        }

    }
#endif

}
