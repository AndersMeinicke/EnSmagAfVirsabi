using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Virsabi;

[RequireComponent(typeof(BoxCollider))]
public class RectTrans3DBoxCollider : MonoBehaviour
{
    [SerializeField]
    private Vector2 margin;

    private void OnValidate()
    {
        GetComponent<BoxCollider>().size = GetComponent<RectTransform>().rect.size + margin;
    }

    //Needed if using layouts (their size is only set on start
    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        GetComponent<BoxCollider>().size = GetComponent<RectTransform>().rect.size + margin;
    }
}
