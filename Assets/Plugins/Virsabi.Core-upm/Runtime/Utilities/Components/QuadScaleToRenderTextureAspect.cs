using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadScaleToRenderTextureAspect : MonoBehaviour
{
    [SerializeField]
    private RenderTexture renderTexture;

    [SerializeField]
    private float Scale;

    [SerializeField, ReadOnly]
    float aspectRatio, width, height;

    private void OnValidate()
    {
        if (renderTexture == null)
            return;

        UpdateScale();
    }

    private void UpdateScale()
    {
        width = renderTexture.width;
        height = renderTexture.height;
        aspectRatio = renderTexture.width / renderTexture.height;

        transform.localScale = new Vector3(renderTexture.width * Scale, renderTexture.height * Scale, 1);
    }
}
