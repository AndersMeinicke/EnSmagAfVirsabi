#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// Put this script on any static renderer that you only want to be active during baking - remember to set the GO's tag to EditorOnly to avoid build warnings
/// </summary>
[ExecuteInEditMode]
public class EnableOnBake : MonoBehaviour
{
    private void OnEnable()
    {
        Lightmapping.bakeStarted += OnBakeStarted;
        Lightmapping.bakeCompleted += OnBakeCompleted;
    }

    private void OnDisable()
    {
        Lightmapping.bakeStarted -= OnBakeStarted;
        Lightmapping.bakeCompleted -= OnBakeCompleted;
    }

    private void OnBakeCompleted()
    {
        foreach (var item in GetComponentsInChildren<Renderer>())
        {
            item.enabled = false;
        }
    }

    private void OnBakeStarted()
    {
        foreach (var item in GetComponentsInChildren<Renderer>())
        {
            item.enabled = true;
        }
    }
}
#endif