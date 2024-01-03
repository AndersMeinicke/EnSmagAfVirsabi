#if PLAYMAKER_EXTENSIONS
using HutongGames.PlayMaker.Actions;
#endif
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Captures trigger collission events.
/// Collission and Trigger events doesn't work for children of a gameobject if there's intermediate rigidbodies. This scripts allows forwarding from a child rigidbody to a parent script.
/// (Usefull for individual fingers on OVR hand tracking).
/// 
/// TODO: Add OnCollision events and delegates.
/// 
/// Author: mu@Virsabi.com
/// 
/// </summary>
/// 
public class RigidBodyProxy : MonoBehaviour
{
    public ColliderDelegate ProxyOnTriggerEnter;
    public ColliderDelegate ProxyOnTriggerStay;
    public ColliderDelegate ProxyOnTriggerExit;

    [SerializeField, ReadOnly]
    public new Collider collider;

    private void OnValidate()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);
        ProxyOnTriggerEnter?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        ProxyOnTriggerStay?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);
        ProxyOnTriggerExit?.Invoke(other);
    }
}

public delegate void ColliderDelegate(Collider other);
