using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArrowPan : CameraControls
{

    [SerializeField] private KeyCode panKeyPlus;
    [SerializeField] private KeyCode panKeyMinus;
    
    private bool arrowPanMode;
    private Vector2 arrowPanIdlePosition, arrowPanPos;
    private bool arrowPan, arrowPanDown, arrowPanUp; // input flags
    private bool arrowPanPlus, arrowPanMinus;
    
    [SerializeField] private ViewCameraSettings settings;
    
    
    private float panStartHeight;
    private Vector2 lastPanInput; // for clamping value
    
    private bool controlStarted, controlEnded, controlWorking;

    public override bool IsActive()
    {
        return controlWorking && controlStarted; 
    }


    public override void ReceiveInput(bool rectNotBlocked)
    {
        if (IsBlocked) return;
        if (!arrowPan)
            arrowPanPos = Vector2.zero;

        arrowPanDown = Input.GetKeyDown(panKeyPlus) || Input.GetKeyDown(panKeyMinus);
        arrowPanUp = Input.GetKeyUp(panKeyPlus) || Input.GetKeyUp(panKeyMinus);
        arrowPan = Input.GetKey(panKeyMinus) || Input.GetKey(panKeyPlus);
        arrowPanPlus = Input.GetKey(panKeyPlus);
        arrowPanMinus = Input.GetKey(panKeyMinus);
        
        if (arrowPanPlus || arrowPanMinus) // control will work because one key is still pressed
            controlWorking = true;
        else
            controlWorking = false;

        if (arrowPanDown && !controlStarted) //todo: make virtual methods
        {
            controlStarted = true;
            Activate(true);
            panStartHeight = _flyCamera.CurrentHeightBuffer; // current height does not know if the pan was not accepted because of the bounds
        }

        if (arrowPanUp && !controlWorking)
        {
            Activate(false);
            // do control end things
            controlStarted = false;
        }
        
        if (controlWorking && controlStarted)
        {
            arrowPanPos += arrowPanPlus ? Vector2.one * settings.arrowPanSpeed: Vector2.zero;
            arrowPanPos -= arrowPanMinus ? Vector2.one  * settings.arrowPanSpeed: Vector2.zero;
        }
    }


    public override void MoveCamera(string affectedProperty)
    {
        float currentHeight;// = (float) GetPropertyValue(affectedProperty);
        
        float axisMouseY = arrowPanPos.y / Screen.width;
        currentHeight = panStartHeight + (cameraTr.up.y * -axisMouseY * settings.keyboardPanSpeed);
        
        // SetPropertyValue(affectedProperty, currentHeight);
        GetSetPropertyAction<float>(affectedProperty).Invoke(currentHeight);
    }

    private float previousValidHeight;
    
    public override void Filter(bool pass, string affectedProperty)
    {
        if (pass)
            previousValidHeight =
                GetGetPropertyFunc<float>(affectedProperty).Invoke(); //(float) GetPropertyValue(affectedProperty);
        else
            // SetPropertyValue(affectedProperty, previousValidHeight);
            GetSetPropertyAction<float>(affectedProperty).Invoke(previousValidHeight);
    }

}
