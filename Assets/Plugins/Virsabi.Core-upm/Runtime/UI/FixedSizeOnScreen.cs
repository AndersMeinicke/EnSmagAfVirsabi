using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Virsabi;


[ExecuteInEditMode]
public class FixedSizeOnScreen : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private float distanceToCamera;

    [SerializeField]
    private bool activeInEditor = true;

    [SerializeField]
    public float FixedSize = 1f;
    
    [SerializeField]
    public bool faceCamera;

    [SerializeField]
    private UpdateMethod UpdateIn;

    private void Update()
    {
        if (UpdateIn != UpdateMethod.update || UpdateIn == UpdateMethod.dontUpdate)
            return;
        Sync();
    }

    private void LateUpdate()
    {
        if (UpdateIn != UpdateMethod.lateUpdate || UpdateIn == UpdateMethod.dontUpdate)
            return;
        Sync();
    }

    private void FixedUpdate()
    {
        if (UpdateIn != UpdateMethod.fixedUpdate || UpdateIn == UpdateMethod.dontUpdate)
            return;
        Sync();
    }

    private void Sync()
    {
        if (!Application.isPlaying && !activeInEditor)
        {
            transform.localScale = new Vector3(1, 1, 1);
            return;
        }
        if (Camera.main == null)
            return;

        distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
        float size = distanceToCamera * FixedSize * 0.00001f * Camera.main.fieldOfView;
        transform.localScale = Vector3.one * size;
        if (faceCamera)
            transform.forward = transform.position - Camera.main.transform.position;
    }
}
