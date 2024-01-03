using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArrowZoom : CameraControls
{
    private bool isActive;
    
    private float speed;
    
    [SerializeField] private ViewCameraSettings settings;

    [SerializeField] private KeyCode alternativeZoomKeyPlus; 
    [SerializeField] private KeyCode alternativeZoomKeyMinus;
    [SerializeField] private KeyCode[] alternativeZoomKeyExtra;
    
    // ARROW ZOOM
    private bool arrowZoomMode;
    private Vector2 arrowZoomIdlePosition, arrowZoomPos;
    private bool arrowZoom, arrowZoomDown, arrowZoomUp;
    private bool arrowZoomPlus, arrowZoomMinus, arrowZoomPlusDown, arrowZoomMinusDown;
    
    private bool controlStarted, controlEnded, controlWorking;
    
    public override bool IsActive()
    {
        return arrowZoom;
    }
    
    private bool GetKeyFromArray(KeyCode[] keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKey(keys[i]))
            {
                return true;
            }
        }

        return false;
    }
    
    private bool GetKeyDownFromArray(KeyCode[] keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                return true;
            }
        }

        return false;
    }
    

    public override void ReceiveInput(bool rectNotBlocked)
    {
        if (IsBlocked) return;

        
        /*if (!arrowZoom)
            arrowZoomPos = Vector2.zero; // sanity check*/
        
        if (!arrowZoom)
            arrowZoomDown = (Input.GetKeyDown(alternativeZoomKeyMinus) || Input.GetKeyDown(alternativeZoomKeyPlus)) &&
                            GetKeyFromArray(alternativeZoomKeyExtra);

        arrowZoomUp = (Input.GetKeyUp(alternativeZoomKeyMinus) || Input.GetKeyUp(alternativeZoomKeyMinus)) && GetKeyFromArray(alternativeZoomKeyExtra);
        
        arrowZoom = (Input.GetKey(alternativeZoomKeyMinus) || Input.GetKey(alternativeZoomKeyPlus)) && GetKeyFromArray(alternativeZoomKeyExtra);
        
        arrowZoomPlus = Input.GetKey(alternativeZoomKeyPlus) && GetKeyFromArray(alternativeZoomKeyExtra);
        arrowZoomMinus = Input.GetKey(alternativeZoomKeyMinus) && GetKeyFromArray(alternativeZoomKeyExtra);
        arrowZoomMinusDown = Input.GetKeyDown(alternativeZoomKeyMinus);
        arrowZoomPlusDown = Input.GetKeyDown(alternativeZoomKeyPlus);

        // control is working when either keys have been pressed
        if (/*Input.GetKey(alternativeZoomKeyMinus) || Input.GetKey(alternativeZoomKeyPlus) || */GetKeyFromArray(alternativeZoomKeyExtra))
            controlWorking = true;
        else
            controlWorking = false;
        
        if (controlWorking && !controlStarted)
        {
            controlStarted = true;
            arrowZoomPos = Vector2.zero;
            Activate(true);
        }

        if (controlStarted && !controlWorking)
        {
            controlStarted = false;
            Activate(false);
        }
        
        if (controlStarted && controlWorking)
        {
            arrowZoomPos = arrowZoomPlus ? Vector2.one : arrowZoomPos;
            arrowZoomPos = arrowZoomMinus ? -Vector2.one : arrowZoomPos;
        }

    }

    private float previousValidDistance;
    private float newDistance;

    public override void MoveCamera(string affectedProperty)
    {
        float currentDistance = GetGetPropertyFunc<float>(affectedProperty).Invoke(); //(float) GetPropertyValue(affectedProperty);

        speed = arrowZoomPos.x * Time.deltaTime * settings.arrowZoomRate;
        newDistance = Mathf.Clamp(speed, -settings.zoomSpeedCap, settings.zoomSpeedCap) * Mathf.Abs(currentDistance);
        currentDistance -= Mathf.Clamp(newDistance, -currentDistance * settings.allowedDistanceMarginPerFrame,
            currentDistance * settings.allowedDistanceMarginPerFrame);
        
        //clamp the zoom min/max
        currentDistance = Mathf.Clamp(currentDistance, settings.minDistance, settings.maxDistance);
        
        // SetPropertyValue(affectedProperty, currentDistance);
        GetSetPropertyAction<float>(affectedProperty).Invoke(currentDistance);

    }

    public override void Filter(bool pass, string affectedProperty)
    {
        if (pass)
            previousValidDistance = GetGetPropertyFunc<float>(affectedProperty).Invoke();//(float) GetPropertyValue(affectedProperty);
        else
            // SetPropertyValue(affectedProperty, previousValidDistance);
            GetSetPropertyAction<float>(affectedProperty).Invoke(previousValidDistance);
    }
}
