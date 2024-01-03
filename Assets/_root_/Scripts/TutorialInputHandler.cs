using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRInput;

public class TutorialInputHandler : MonoBehaviour
{
    [SerializeField] private GameObject mesh;
    [SerializeField] private GameObject tutorial;
    private GameObject objectHandler = null;
    private Transform endTransform;
    public LineRenderer lineRenderer;
    public Controller RightHandController;

    public GameObject cursor;

    private void Start()
    {
        endTransform = transform.Find("RotationSetter");
    }
    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.forward);

        RaycastHit hit;
        if (objectHandler == null)
         {
               if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
               cursor.SetActive(true);
             cursor.transform.position = hit.point;

              if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, RightHandController) || OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, RightHandController))
               {
                    SetValues(hit.collider.gameObject);
                    if (hit.collider.gameObject.GetComponent<TutorialObjectHandler>() != null)
                  {
                   hit.collider.gameObject.GetComponent<TutorialObjectHandler>().Activate();
                  }
               }

            }
            else
            {
                cursor.SetActive(false);
            }
        }
        else if (objectHandler != null)
        {
            cursor.SetActive(false);

            if (objectHandler.GetComponent<TutorialObjectHandler>().getIsActive() == true && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, RightHandController)|| objectHandler.GetComponent<TutorialObjectHandler>().getIsActive() == true && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, RightHandController))
            {
                objectHandler.GetComponent<TutorialObjectHandler>().Deactivate();
                objectHandler = null;
            }
        }
    }
    private void SetValues(GameObject objectHandeling)
    {
        objectHandeling.GetComponent<TutorialObjectHandler>().SetAnchor(gameObject);
        objectHandeling.GetComponent<TutorialObjectHandler>().SetMesh(mesh);
        objectHandeling.GetComponent<TutorialObjectHandler>().SetTutorial(tutorial);
        objectHandeling.GetComponent<TutorialObjectHandler>().SetEndTransform(endTransform);
        objectHandler = objectHandeling;
    }
}
