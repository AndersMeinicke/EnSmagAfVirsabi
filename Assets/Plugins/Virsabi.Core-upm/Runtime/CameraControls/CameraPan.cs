using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : CameraControls
{
    [SerializeField] private KeyCode cameraPanKey;
    
    // [SerializeField] private float minPosition;
    [SerializeField] private ViewCameraSettings settings;

    private bool panInputKey, panInputKeyDown, panInputKeyUp; // input flags
    
    private Vector2 panIdleMousePosition; // the v2 position of the mouse when no click is detected
    private Vector2 panMouseCurrentPosition; // the current position of the mouse during a drag
    private Vector2 positionDelta; // the difference of the above 2 values

    private float panStartHeight;
    private Vector2 lastPanInput; // for clamping value
    
    private bool isDragging;
    
    // cached
    // private Transform tr;
    
    private bool controlStarted, controlEnded, controlWorking;
    
    public override bool IsActive ()
    {
       return isDragging;
    }
 
    public override void ReceiveInput(bool rectNotBlocked)
    {
        if (IsBlocked) return;
        
        panInputKeyDown = Input.GetKeyDown(cameraPanKey);
        panInputKey = Input.GetKey(cameraPanKey);
        panInputKeyUp = Input.GetKeyUp(cameraPanKey);
        
        if (panInputKey) // control will work because one key is still pressed
            controlWorking = true;
        else
            controlWorking = false;

        if (panInputKeyDown && !controlStarted) //todo: make virtual methods
        {
            controlStarted = true;
            Activate(true);
            panStartHeight = _flyCamera.CurrentHeightBuffer; // current height does not know if the pan was not accepted because of the bounds
        }
        
        if (panInputKeyUp && !controlWorking)
        {
            Activate(false);
            // do control end things
            controlStarted = false;
        }

        if (panInputKeyDown && rectNotBlocked)
            isDragging = true; // begin dragging
        
        if (panInputKeyUp)
            isDragging = false; // end drag

        
        if (controlWorking && controlStarted)
        {
            panMouseCurrentPosition = Input.mousePosition;
            positionDelta = new Vector2(panMouseCurrentPosition.x - panIdleMousePosition.x, panMouseCurrentPosition.y - panIdleMousePosition.y);
        }
        else
        {
            panIdleMousePosition = Input.mousePosition;
        }
    }
    
    public override void MoveCamera(string affectedProperty)
    {
        
        float currentHeight =  GetGetPropertyFunc<float>(affectedProperty).Invoke();//(float) GetPropertyValue(affectedProperty);
        float axisMouseY = positionDelta.y / Screen.width;
        currentHeight = panStartHeight + (cameraTr.up.y * -axisMouseY * settings.panSpeed);
        GetSetPropertyAction<float>(affectedProperty).Invoke(currentHeight);
        
        // SetPropertyValue(affectedProperty, currentHeight); // CAUSES CG Alloc
    }

    private float previousValidHeight;


    public override void Filter(bool pass, string affectedProperty)
    {
        if (pass)
            previousValidHeight = GetGetPropertyFunc<float>(affectedProperty).Invoke(); //(float) GetPropertyValue(affectedProperty);
        else
            // SetPropertyValue(affectedProperty, previousValidHeight); // CAUSES CG Alloc
            GetSetPropertyAction<float>(affectedProperty).Invoke(previousValidHeight);
    }
}
