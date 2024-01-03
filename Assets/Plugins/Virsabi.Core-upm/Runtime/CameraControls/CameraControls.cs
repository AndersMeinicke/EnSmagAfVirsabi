using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(FlyCamera))]
public abstract class CameraControls : MonoBehaviour
{
    // static list of all controls
    public static List<CameraControls> allControls = new List<CameraControls>();
    
    // blocks all controls that are not the current activated one
    private static void HandleControls(bool state, int index)
    {
        if (state)
        {
            for (int i = 0; i < allControls.Count; i++)
            {
                if (i != index)
                {
                    allControls[i].Block();
                }
            }
        }
        else
        {
            for (int i = 0; i < allControls.Count; i++)
            {
                allControls[i].UnBlock();
            }
        }
    }
    
    
    protected FlyCamera _flyCamera;
    protected Transform cameraTr;
    protected PropertyInfo propertyInfo;

    #region Activation and blocking logic
    public delegate void IsActivated(bool state);
    public event IsActivated OnActivation;
    
    private bool isBlocked, isActivated;

    public bool IsBlocked => isBlocked;

    public void Activate(bool state)
    {
        if (isBlocked) return; // cant activate a blocked control
        isActivated = state;
        OnActivation?.Invoke(isActivated);
    }

    public void Block()
    {
        isBlocked = true;
    }

    public void UnBlock()
    {
        isBlocked = false;
    }
    
    #endregion
    
    protected virtual void Awake()
    {
        _flyCamera = GetComponent<FlyCamera>();
        cameraTr = _flyCamera.transform;
        
        // register
        CameraControls.allControls.Add(this);
        int modifiedIndex = allControls.Count - 1;
        OnActivation += state => HandleControls( state,modifiedIndex);
    }
    
    protected virtual void OnEnable()
    {
        _flyCamera.OnReceiveInput += ReceiveInput;
    }

    protected virtual void OnDisable()
    {
        _flyCamera.OnReceiveInput -= ReceiveInput;
    }

    /*protected MethodInfo method;
    protected bool initialized;*/

    private Delegate SetMethod;
    private Delegate GetMethod;
    
    


    public abstract bool IsActive();

    public abstract void ReceiveInput(bool rectNotBlocked);

    // affects a buffer position in fly camera
    public abstract void MoveCamera(string affectedProperty);

    public abstract void Filter(bool pass, string affectedProperty);

    // reflection stuff, creates garbage
    protected void SetPropertyValue(string propName, object value)
    {
        if (propertyInfo == null)
            propertyInfo = _flyCamera.GetType().GetProperty(propName);
        
        propertyInfo.SetValue(_flyCamera,Convert.ChangeType(value, propertyInfo.PropertyType));
    }

    // creates garbage
    protected object GetPropertyValue (string propName)
    {
        if (propertyInfo == null)
            propertyInfo = _flyCamera.GetType().GetProperty(propName);
        
        return propertyInfo.GetValue(_flyCamera, null);
    }
    
    // optimized: creates a delegate
    protected Func<T> GetGetPropertyFunc<T> (string propName)
    {
        if (GetMethod == null)
        {
            propertyInfo = _flyCamera.GetType().GetProperty(propName);
            GetMethod = Delegate.CreateDelegate(typeof(Func<T>), _flyCamera, propertyInfo.GetGetMethod());
        }
        return (Func<T>) GetMethod;
    }
    
    // todo: reflection can fail in builds? Mathias had a problem with it 
    protected Action<T> GetSetPropertyAction<T> (string propName)
    {
        if (SetMethod == null)
        {
            propertyInfo = _flyCamera.GetType().GetProperty(propName);
            SetMethod = Delegate.CreateDelegate(typeof(Action<T>), _flyCamera, propertyInfo.GetSetMethod());
        }

        return (Action<T>) SetMethod;
    }
}
