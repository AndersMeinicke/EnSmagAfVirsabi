using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LoadSceneBubble : MonoBehaviour
{
    public Material Enter;
    public Material Exit;
    public AudioSource AudioSource;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "LeftHandAnchor" || other.gameObject.name == "RightHandAnchor")
        {
            GetComponent<Renderer>().material = Enter;
            Transform controller = other.transform.GetChild(0).transform;
            controller.gameObject.GetComponent<ClipSender>().isDone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "LeftHandAnchor" || other.gameObject.name == "RightHandAnchor")
        {
            GetComponent<Renderer>().material = Exit;
            Transform controller = other.transform.GetChild(0).transform;
            controller.gameObject.GetComponent<ClipSender>().isDone = false;
        }
    }
}
