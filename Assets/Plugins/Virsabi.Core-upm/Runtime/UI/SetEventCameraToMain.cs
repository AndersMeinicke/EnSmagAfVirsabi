using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class SetEventCameraToMain : MonoBehaviour
{
    [SerializeField, HideInInspector]
    private Canvas canvas;
    [SerializeField]
    private bool runInUpdate;

    private void OnValidate()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    private void Start()
    {
        canvas.worldCamera = Camera.main;
    }

    private void Update()
    {
        if(runInUpdate)
            canvas.worldCamera = Camera.main;
    }
}
