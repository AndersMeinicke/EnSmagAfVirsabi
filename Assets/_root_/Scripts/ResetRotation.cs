using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.localEulerAngles = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
