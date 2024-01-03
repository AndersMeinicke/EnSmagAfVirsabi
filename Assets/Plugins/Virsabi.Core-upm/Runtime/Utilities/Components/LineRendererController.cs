using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Virsabi;

[ExecuteInEditMode, RequireComponent(typeof(LineRenderer))]
public class LineRendererController : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private LineRenderer lineRenderer;

    [SerializeField]
    private UpdateMethod updateMethod = UpdateMethod.lateUpdate;

    [SerializeField]
    private Transform startPosition, endPosition;

    
    private void OnValidate()
    {
        if(!lineRenderer)
            lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (updateMethod != UpdateMethod.update)
            return;
        Sync();
    }

    private void FixedUpdate()
    {
        if (updateMethod != UpdateMethod.fixedUpdate)
            return;
        Sync();
    }

    private void LateUpdate()
    {

        if (updateMethod != UpdateMethod.lateUpdate)
            return;
        Sync();
    }

    private void Sync()
    {
        if (lineRenderer.useWorldSpace)
        {
            lineRenderer.SetPosition(0, startPosition.position);
            lineRenderer.SetPosition(1, endPosition.position);
        }
        else
        {
            lineRenderer.SetPosition(0, lineRenderer.worldToLocalMatrix.MultiplyPoint(startPosition.position));
            lineRenderer.SetPosition(1, lineRenderer.worldToLocalMatrix.MultiplyPoint(endPosition.position));
        }

    }
}
