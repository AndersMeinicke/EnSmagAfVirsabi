using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is supposed to be the start of a centralized interface for controlling colors on any type of material - linerenderers is the first to go down
/// </summary>
[RequireComponent(typeof(Renderer))]
public class MaterialColorController : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private Renderer materialRenderer;

    [SerializeField]
    public Color currentColor;

    [SerializeField, ReadOnly]
    private bool isLineRenderer;

    [SerializeField, ConditionalField(nameof(isLineRenderer))]
    private Shader lineRenderShader;

    [SerializeField]
    public Color startColor;

    private void OnValidate()
    {
        materialRenderer = GetComponent<Renderer>();

        CheckIfLinerenderer();
    }

    private void CheckIfLinerenderer()
    {
        if (materialRenderer is LineRenderer)
        {
            isLineRenderer = true;

            originalgradient = (materialRenderer as LineRenderer).colorGradient;
        }

        isLineRenderer = false;
    }

    [SerializeField, ReadOnly]
    private Gradient originalgradient;

    private void Start()
    {
        if (materialRenderer is LineRenderer)
        {
            //renderer.material = renderer.material;
            //renderer.material.shader = lineRenderShader;
        }

    }

    private void Update()
    {
        if (isLineRenderer)
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(currentColor, 0.0f), new GradientColorKey(currentColor, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(0 * currentColor.a, 0.0f), new GradientAlphaKey(1 * currentColor.a, 1.0f) }
            );


            (materialRenderer as LineRenderer).colorGradient = gradient;
        }
        else
        {

        }
        
    }
}
