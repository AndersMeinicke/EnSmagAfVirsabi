using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(AmbisonicAudioClip))]
public class AmbisonicAudioClipDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PropertyField(position, property.FindPropertyRelative("clip"), new GUIContent(property.name));
    }
}
