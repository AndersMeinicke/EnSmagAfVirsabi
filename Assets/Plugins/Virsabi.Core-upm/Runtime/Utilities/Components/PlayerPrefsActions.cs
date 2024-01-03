using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for quickly calling playerprefs actions (mostly for debugging)
/// </summary>
public class PlayerPrefsActions : MonoBehaviour
{
    public void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
