﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This service locator does not work with interfaces - it only works for Monobehaviors.
/// </summary>
//-----------------------------------------------------------------------------------------------------
public static class MonoServiceLocator
{
    //Hold Reference to all found services as type as key , and reference to the concerete object
    static Dictionary<object, object> servicecontainer = null;

    //-----------------------------------------------------------------------------------------------------
    /// <summary>
    /// Find a service/script in current scene and return reference of it , Note: it will still find the service even if it's unactive
    /// </summary>
    /// <typeparam name="T">Type of service to find</typeparam>
    /// <returns></returns>
    public static T GetMonoService<T>(bool createObjectIfNotFound = true) where T : Object
    {
        //Init the dictionary
        if (servicecontainer == null)
            servicecontainer = new Dictionary<object, object>();

        try
        {
            //Check if the key exist in the dictionary
            if (servicecontainer.ContainsKey(typeof(T)))
            {
                T service = (T)servicecontainer[typeof(T)];
                if (service != null)  //If Key exist and the object it reference to still exist
                {
                    return service;
                }
                else                  //The key exist but reference object doesn't exist anymore
                {
                    servicecontainer.Remove(typeof(T));           //Remove this key from the dictonary
                    return FindMonoService<T>(createObjectIfNotFound);        //Try and find the service in current scene
                }
            }
            else
            {
                return FindMonoService<T>(createObjectIfNotFound);
            }
        }
        catch (System.Exception ex)
        {
            throw new System.NotImplementedException("Can't find requested service, and create new one is set to " + createObjectIfNotFound + 
                "\n" + ex.Message);
        }
    }

    //-----------------------------------------------------------------------------------------------------
    /// <summary>
    /// Look for a game object with type required
    /// </summary>
    /// <typeparam name="T">Type to look for</typeparam>
    /// <param name="createObjectIfNotFound">Either create a gameobject with type if not exist</param>
    /// <returns></returns>
    static T FindMonoService<T>(bool createObjectIfNotFound = true) where T : Object
    {
        T type = GameObject.FindObjectOfType<T>();
        if (type != null)
        {
            //If found add it to the dictonary
            servicecontainer.Add(typeof(T), type);
        }
        else if (createObjectIfNotFound)
        {
            //If not found and set to create new gameobject , create a new gameobject and add the type to it
            GameObject go = new GameObject(typeof(T).Name, typeof(T));
            servicecontainer.Add(typeof(T), go.GetComponent<T>());
        }
        return (T)servicecontainer[typeof(T)];
    }
}

