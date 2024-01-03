using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(order = 1,fileName = "ViewCameraSettings", menuName = "Camera/ViewCameraSettings")]
public class ViewCameraSettings : ScriptableObject
{
    public Vector3 targetOffset;

    public float xDegreesInitial;
    public float yDegreesInitial;

    public float panSpeed = 0.3f;
    public float arrowMoveSpeed = 100f;
    public float arrowPanSpeed = 100f;
    public float keyboardPanSpeed = 30;
    
    [Header("Camera min, max and initial distance")]
    public float startingDistance;
    public float minDistance = .6f;
    public float maxDistance = 20;
    
    [Header("Initial height")]
    public float startingHeight;

    
    [Header("Speed on the X and Y axis")]
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    
    [Header("Limit the Y axis angle")]
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    
    [Header("Zoom settings")]
    public int zoomRate = 40;
    public float zoomDampening = 5.0f;
    public int arrowZoomRate = 40;

    [Range(0f,1f)]
    public float allowedDistanceMarginPerFrame = 0.04f;
    public float zoomSpeedCap = 2f;
}
