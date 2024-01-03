using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Receives input and returns a Vector2 value for the rotation of the camera
/// </summary>
public class CameraOrbit : CameraControls
{
    
    private float xAxis, yAxis;
    private Vector2 previousAngleRotation;
    
    [SerializeField] private ViewCameraSettings settings;
    [SerializeField] private KeyCode cameraMoveKey;
    
    private bool moveMode;
    private bool moveInputKey, moveInputKeyDown, moveInputKeyUp; // move input flags

    [SerializeField] private Vector2 moveIdleMousePosition; // the v2 position of the mouse when no click is detected
    
    private Vector2 moveMouseCurrentPosition; // the current position of the mouse during a drag
    private Vector2 positionDelta; // the difference of the above 2 values
    
    private float onClickStartXDeg, onClickStartYDeg; // the degree values at the beginning of a drag
    
    private bool controlStarted, controlEnded, controlWorking;

    public override bool IsActive()
    {
         return controlStarted; 
    }
    public override void ReceiveInput(bool rectTransformNotBlocked)
    {
        if (IsBlocked) return;

        moveInputKeyDown = Input.GetKeyDown(cameraMoveKey);
        moveInputKeyUp = Input.GetKeyUp(cameraMoveKey);
        moveInputKey = Input.GetKey(cameraMoveKey);

        if (moveInputKey)
            controlWorking = true;
        else
            controlWorking = false;
        
        
        if (moveInputKeyDown && !controlStarted && rectTransformNotBlocked)
        {
            controlStarted = true;
            Activate(true);
            moveIdleMousePosition = Input.mousePosition;
            
            onClickStartXDeg = _flyCamera.AngleRotationBuffer.x;
            onClickStartYDeg = _flyCamera.AngleRotationBuffer.y; 
        }

        
        if (moveInputKeyUp && !controlWorking)
        {
            controlStarted = false;
            Activate(false);
        }
        
        if (controlWorking)
        {
            moveMouseCurrentPosition = Input.mousePosition;
            positionDelta = new Vector2(moveMouseCurrentPosition.x - moveIdleMousePosition.x, moveMouseCurrentPosition.y - moveIdleMousePosition.y);
        }
    }
    
    public override void MoveCamera(string affectedProperty)
    {
        // type casting is for poppers, generics are for programmers :P 
        
        Vector2 angleRotation = GetGetPropertyFunc<Vector2>(affectedProperty).Invoke();
        // Vector2 angleRotation = (Vector2) GetPropertyValue(affectedProperty);
        
        xAxis = positionDelta.x;
        yAxis = -positionDelta.y;
            
        angleRotation.x = onClickStartXDeg + (xAxis * settings.xSpeed * 0.02f);
        angleRotation.y = onClickStartYDeg + (yAxis * settings.ySpeed * 0.02f);
        
        //Clamp the vertical axis for the orbit
        angleRotation.y = CameraExtensionMethods.ClampAngle(angleRotation.y, settings.yMinLimit, settings.yMaxLimit);

        GetSetPropertyAction<Vector2>(affectedProperty).Invoke(angleRotation);
        
        // SetPropertyValue(affectedProperty, angleRotation);
    }

    public override void Filter(bool pass, string affectedProperty)
    {
        if (pass)
            previousAngleRotation = GetGetPropertyFunc<Vector2>(affectedProperty).Invoke(); //(Vector2) GetPropertyValue(affectedProperty);
        else
        {
            GetSetPropertyAction<Vector2>(affectedProperty).Invoke(previousAngleRotation); //SetPropertyValue(affectedProperty, previousAngleRotation);
        }
        
        Vector2 angle = GetGetPropertyFunc<Vector2>(affectedProperty).Invoke(); //(Vector2) GetPropertyValue(affectedProperty);
        angle.y = CameraExtensionMethods.ClampAngle(angle.y, settings.yMinLimit, settings.yMaxLimit);
        // SetPropertyValue(affectedProperty, angle);
        GetSetPropertyAction<Vector2>(affectedProperty).Invoke(angle);
    }
    
}
