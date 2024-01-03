using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FindAssetByGUI : MonoBehaviour
{
    public string guid;

    public string foundAtpath;

    [ContextMenu("search")]
    private void FindByGuid()
    {
        foundAtpath = AssetDatabase.GUIDToAssetPath(guid);
    }
}
