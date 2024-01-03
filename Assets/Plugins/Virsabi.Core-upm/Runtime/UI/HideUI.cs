using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script for hiding UI Elements - Layermask only works if canvas is set to "Screen Space - Camera"
/// </summary>
public class HideUI : MonoBehaviour
{

    [SerializeField]
    private LayerMask layerMaskWhenUIHidden;

    [SerializeField, ReadOnly]
    private LayerMask originalLayerMask;

    [SerializeField]
    private List<Canvas> overlayCanvasesToHide;

    private void OnValidate()
    {
        originalLayerMask = Camera.main.cullingMask;
    }

    [ButtonMethod]
    public void ToggleUI()
    {
        if(Camera.main.cullingMask == originalLayerMask)
        {
            foreach (var item in overlayCanvasesToHide)
            {
                item.enabled = false;
            }
            Camera.main.cullingMask = layerMaskWhenUIHidden;
        }
        else
        {
            foreach (var item in overlayCanvasesToHide)
            {
                item.enabled = true;
            }
            Camera.main.cullingMask = originalLayerMask;
        }
    }
}
