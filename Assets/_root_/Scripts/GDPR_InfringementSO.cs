using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Virsabi;

[CreateAssetMenu(menuName = "GDPR_InfringementSO", fileName = "GDPR_InfringementSO", order = 1)]
public class GDPR_InfringementSO : IconScriptableObject
{
    [SerializeField]
    private string title;

    [SerializeField, TextArea]
    private string shortDescription, longDescription;

    public string Title { get => title; private set => title = value; }
    public string ShortDescription { get => shortDescription; private set => shortDescription = value; }
    public string LongDescription { get => longDescription; private set => longDescription = value; }

    public delegate void OnValidateDelegate();

    public event OnValidateDelegate OnSOValidate;

    private void OnValidate()
    {
        if (OnSOValidate != null)
            OnSOValidate();
    }
}
