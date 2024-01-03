using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArrowOrbit : CameraControls
{
    [SerializeField] private KeyCode moveKeyPlus;
    [SerializeField] private KeyCode moveKeyMinus;
    [SerializeField] private ViewCameraSettings settings;
    
    private float xAxis, yAxis;
    private Vector2 previousAngleRotation;
    

    private bool arrowMoveMode;
    private Vector2 arrowMoveIdlePosition, arrowMovePos;
    private bool arrowMove, arrowMoveDown, arrowMoveUp;
    private bool arrowMovePlus, arrowMoveMinus;

    private bool controlStarted, controlEnded, controlWorking;
    
    private float onClickStartXDeg; // the degree values at the beginning of a drag

    public override bool IsActive()
    {
        return controlWorking && controlStarted; 
    }

    
    public override void ReceiveInput(bool rectTransformNotBlocked)
    {
        if (IsBlocked) return;
        
        if (!arrowMove)
            arrowMovePos = Vector2.zero;
        
        // read keys
        arrowMove = Input.GetKey(moveKeyMinus) || Input.GetKey(moveKeyPlus);
        arrowMoveDown = (Input.GetKeyDown(moveKeyMinus) || Input.GetKeyDown(moveKeyPlus));
        arrowMovePlus = Input.GetKey(moveKeyPlus);
        arrowMoveMinus = Input.GetKey(moveKeyMinus);
        arrowMoveUp = Input.GetKeyUp(moveKeyPlus) || Input.GetKeyUp(moveKeyMinus);

        // when should the control be in working state
        if (arrowMovePlus || arrowMoveMinus) // control will work because one key is still pressed
            controlWorking = true;
        else
            controlWorking = false;

        // when should the control start
        if (arrowMoveDown && !controlStarted)
        {
            controlStarted = true;

            // do control start things
            Activate(true);
            onClickStartXDeg = _flyCamera.AngleRotationBuffer.x;
        }

        // when should the control end
        if (arrowMoveUp && !controlWorking)
        {
            controlStarted = false;
            // do control end things
            Activate(false);
        }

        if (controlWorking && controlStarted)
        {
            arrowMovePos += arrowMovePlus ? Vector2.one * settings.arrowMoveSpeed : Vector2.zero;
            arrowMovePos -= arrowMoveMinus? Vector2.one * settings.arrowMoveSpeed: Vector2.zero;
        }
    }

    private Quaternion desiredRotation;
    

    public override void MoveCamera(string affectedProperty)
    {
        // Vector2 angleRotation = (Vector2) GetPropertyValue(affectedProperty);

        Vector2 angleRotation = GetGetPropertyFunc<Vector2>(affectedProperty).Invoke();

        xAxis = arrowMovePos.x;
        angleRotation.x = onClickStartXDeg + (xAxis * settings.xSpeed * 0.02f);
        
        GetSetPropertyAction<Vector2>(affectedProperty).Invoke(angleRotation);
    }

    public override void Filter(bool pass, string affectedProperty)
    {
        if (pass)
            previousAngleRotation = GetGetPropertyFunc<Vector2>(affectedProperty).Invoke();
        else
        {
            GetSetPropertyAction<Vector2>(affectedProperty).Invoke(previousAngleRotation);
        }
        
        Vector2 angle = GetGetPropertyFunc<Vector2>(affectedProperty).Invoke();
        angle.y = CameraExtensionMethods.ClampAngle(angle.y, settings.yMinLimit, settings.yMaxLimit);
        GetSetPropertyAction<Vector2>(affectedProperty).Invoke(angle);
    }
}

/*public Vector2 CalculateExpectedRotation(Vector2 angleRotation) // arrow orbiting only affects x in angle rotation
    {
        xAxis = arrowMovePos.x;
            
        angleRotation.x = onClickStartXDeg + (xAxis * settings.xSpeed * 0.02f);
        // angleRotation.y = onClickStartYDeg + (yAxis * settings.ySpeed * 0.02f);

        return angleRotation;
    }*/
