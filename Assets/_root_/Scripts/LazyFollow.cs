using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LazyFollow : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float distance = 3.0f;

    private bool isCentered = false;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    private void OnBecameInvisible()
    {
        isCentered = false;
        Debug.Log("OH NO");
    }
     private void Update()
    {
        if (!isCentered)
        {
            Vector3 targetPosition = FindTargetPosition();

            MoveTowards(targetPosition);
            transform.LookAt(new Vector3(cameraTransform.position.x,transform.position.y,cameraTransform.position.z));
            transform.forward *= -1; 
            if (ReachedPosition(targetPosition))
            {
                isCentered = true;
            }
        }
    }

    private Vector3 FindTargetPosition()
    {
        return cameraTransform.position + (cameraTransform.forward * distance);
    }
    private void MoveTowards(Vector3 targetPosition)
    {
        transform.position += (targetPosition - transform.position) * 0.025f;
    }

    private bool ReachedPosition(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, transform.position) < 0.1f;
    }
}
