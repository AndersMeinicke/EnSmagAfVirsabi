using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Calculates a bounded polygon depending on two points todo: make it a convex polygon
/// </summary>
public class CameraPositionBounds : MonoBehaviour
{
    [SerializeField] private Transform limit1; // upper
    [SerializeField] private Transform limit2; // lower

    private Vector3 maxPos, minPos;

    private void Awake()
    {
        maxPos = limit1.position;
        minPos = limit2.position;
    }

    public bool IsPointInBounds(Vector3 point)
    {
        bool isBeforeLimit1 = maxPos.x > point.x && maxPos.y > point.y && maxPos.z > point.z;

        bool isBeforeLimit2 = minPos.x < point.x && minPos.y < point.y && minPos.z < point.z;

        return (isBeforeLimit1 && isBeforeLimit2);
    }

    public Vector3 ClampYPosition(Vector3 inputPosition)
    {
        return new Vector3(inputPosition.x, Mathf.Clamp(inputPosition.y, minPos.y + 0.01f, maxPos.y - 0.01f), inputPosition.z);
    }

    private void OnDrawGizmos()
    {
        if (limit1 == null || limit2 == null)
        {
            return;
        }
        
        Vector3 centroid = Vector3.Lerp(limit1.position, limit2.position, 0.5f);
        Debug.DrawLine(centroid, centroid + Vector3.up, UnityEngine.Color.blue);
        
        Vector3 size = new Vector3((Mathf.Abs(limit1.position.x - limit2.position.x)), Mathf.Abs(limit1.position.y - limit2.position.y), Mathf.Abs(limit1.position.z - limit2.position.z) );
        Gizmos.color = new UnityEngine.Color(0.8f, 0f,0f,0.5f);
        Gizmos.DrawCube(centroid, size );
    }
}
