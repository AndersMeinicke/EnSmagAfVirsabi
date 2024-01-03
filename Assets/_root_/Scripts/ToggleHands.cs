using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRInput;

public class ToggleHands : MonoBehaviour
{
    [SerializeField] private ToggleHands toggleHands;
    [SerializeField] private GameObject anchor;
    [SerializeField] private SkinnedMeshRenderer mesh;
    [SerializeField] private GameObject tutorial;
    public Controller RightHandController;
    public bool isTutorial;
    private bool active = false;
    // Start is called before the first frame update
    private void Activate()
    {
        if (!active) { 
            toggleHands.DeActivate();
            anchor.GetComponent<LineRenderer>().enabled = true;
            anchor.transform.Find("Cursor").gameObject.SetActive(true);
            anchor.GetComponent<InputHandler>().enabled = true;
            mesh.enabled = true;
            if (isTutorial)
            {
                tutorial.SetActive(true);
            }
            active = true;
        }

    }
    private void DeActivate()
    {
        anchor.GetComponent<LineRenderer>().enabled = false;
        anchor.transform.Find("Cursor").gameObject.SetActive(false);
        anchor.GetComponent<InputHandler>().enabled = false;
        mesh.enabled = false;
        if(isTutorial)
        {
            tutorial.SetActive(false);
        }
        active = false;

    }
    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, RightHandController))
        {
            Activate();
        }
    }
}
