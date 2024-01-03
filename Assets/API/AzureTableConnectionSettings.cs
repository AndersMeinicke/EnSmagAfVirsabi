using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AzureTableConnectionSettings", menuName = "AzureTableConnectionSettings", order = 0)]
public class AzureTableConnectionSettings : ScriptableObject
{
    [Foldout("Setup", true)]
    [SerializeField]
    private string storageAccount = "hbrtopscores", sasKey = "", table = "TopScoresCyberSecurity";

    [SerializeField]
    private int maxSecondsUntilTimeout = 5, retries = 2;

    [Foldout("Debug", true)]
    [SerializeField, TextArea]
    private string uri;

    public string Uri { get => uri; private set => uri = value; }
    public string StorageAccount { get => storageAccount; private set => storageAccount = value; }
    public string SasKey { get => sasKey; private set => sasKey = value; }
    public string Table { get => table; private set => table = value; }
    public int MaxSecondsUntilTimeout { get => maxSecondsUntilTimeout; private set => maxSecondsUntilTimeout = value; }
    public int Retries { get => retries; private set => retries = value; }

    private void OnValidate()
    {
        Uri = "https://" + StorageAccount + ".table.core.windows.net/" + Table;
    }
}
