using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(FlyCamera))]
/// <summary>
/// This control does not have a start/end condition, but it must be blocked by other controls
/// </summary>
public class CameraZoom : CameraControls
{
    
    private bool isActive;
    private bool zoomInputKey;
    private float scrollWheelAxis; // input

    private const string mouseScrollWheel = "Mouse ScrollWheel";

    private float speed;
    
    [SerializeField] private ViewCameraSettings settings;


    public override bool IsActive()
    {
         return controlWorking; 
    }
    
    private bool controlStarted, controlEnded, controlWorking;


    public override void ReceiveInput(bool rectNotBlocked)
    {
        if (IsBlocked) return;
        if (!rectNotBlocked) return;
        

        scrollWheelAxis = Input.GetAxis(mouseScrollWheel);
        
        zoomInputKey = scrollWheelAxis != 0;

        // activate when the zoom input key is pressed
        if (zoomInputKey)
            controlWorking = true;
        else
            controlWorking = false;

        if (zoomInputKey && !controlStarted)
        {
            controlStarted = true;
            Activate(true);
        }

        if (controlStarted && !controlWorking)
        {
            Activate(false);
            // do control end things
            controlStarted = false;
        }


    }

    private float previousValidDistance;
    private float newDistance;

    public override void MoveCamera(string affectedProperty)
    {
        float currentDistance = GetGetPropertyFunc<float>(affectedProperty).Invoke(); //(float) GetPropertyValue(affectedProperty);
        
        speed = scrollWheelAxis * Time.deltaTime * settings.zoomRate;
        newDistance = Mathf.Clamp(speed, -settings.zoomSpeedCap, settings.zoomSpeedCap) * Mathf.Abs(currentDistance);
        currentDistance -= Mathf.Clamp(newDistance, -currentDistance * settings.allowedDistanceMarginPerFrame,
            currentDistance * settings.allowedDistanceMarginPerFrame);
        
        //clamp the zoom min/max
        currentDistance = Mathf.Clamp(currentDistance, settings.minDistance, settings.maxDistance);
        
        GetSetPropertyAction<float>(affectedProperty).Invoke(currentDistance);
        // SetPropertyValue(affectedProperty, currentDistance);
    }
    
    public override void Filter(bool pass, string affectedProperty)
    {
        if (pass)
            previousValidDistance =  GetGetPropertyFunc<float>(affectedProperty).Invoke();//(float) GetPropertyValue(affectedProperty);
        else
            // SetPropertyValue(affectedProperty, previousValidDistance);
            GetSetPropertyAction<float>(affectedProperty).Invoke(previousValidDistance);

    }
}
