using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRInput;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private AudioClip correctClip;
    [SerializeField] private AudioClip otherClip;
    [SerializeField] private GameObject mesh;
    private GameObject objectHandler = null;
    private GameObject color;
    private Transform endTransform;
    public LineRenderer lineRenderer;
    public Controller RightHandController;
    public GameObject cursor;

    private void Start()
    {
       endTransform = transform.Find("RotationSetter");
    }
    // Update is called once per frame
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
                if(hit.collider.gameObject.name != "CursorHit" ) { 
                cursor.transform.position = hit.point;
                }
                if ((hit.collider.gameObject.GetComponent<SceneLoader>() != null))
                {
                    hit.collider.gameObject.GetComponent<SceneLoader>().Highlight();
                    color = hit.collider.gameObject;
                }
                else {
                if(color != null)
                    {
                        color.GetComponent<SceneLoader>().nonHighlight();
                        color = null;
                    }
                }

                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, RightHandController) || OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, RightHandController))
                {

                    if (hit.collider.gameObject.GetComponent<ObjectHandler>() != null)
                    {
                        SetValues(hit.collider.gameObject);
                        
                        if (hit.collider.gameObject.GetComponent<ObjectHandler>().getIsThreat() == true)
                        {
                            AudioSource audio = GameObject.Find("Global Volume").GetComponent<AudioSource>();
                            audio.clip = correctClip;
                            audio.Play();
                            hit.collider.gameObject.GetComponent<ObjectHandler>().Activate();
                        }
                        else if (hit.collider.gameObject.GetComponent<ObjectHandler>().getIsNeutral() == true)
                        {
                            AudioSource audio = GameObject.Find("Global Volume").GetComponent<AudioSource>();
                            audio.clip = otherClip;
                            audio.Play();
                            hit.collider.gameObject.GetComponent<ObjectHandler>().Activate();
                        }
                        else
                        {
                            hit.collider.gameObject.GetComponent<ObjectHandler>().Activate();
                            GameObject.Find("Manager").GetComponent<TimeManager>().Punishment();
                        }
                    }
                    else if ((hit.collider.gameObject.GetComponent<SceneLoader>() != null))
                    {
                        hit.collider.gameObject.GetComponent<SceneLoader>().Activate();
                    }
                    else
                    {
                        if (GameObject.Find("Manager") != null)
                        {
                            GameObject.Find("Manager").GetComponent<TimeManager>().Punishment();
                        }
                    }
                }

            }
            else
            {
                if(color != null) { 
                color.GetComponent<SceneLoader>().nonHighlight();
                    color = null;
                }
                cursor.SetActive(false);
            }
        }
        else if(objectHandler != null)
        {
            cursor.SetActive(false);

            if(objectHandler.GetComponent<ObjectHandler>().getIsActive() == true && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, RightHandController) 
                || objectHandler.GetComponent<ObjectHandler>().getIsActive() == true && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, RightHandController))
            {
                objectHandler.GetComponent<ObjectHandler>().Deactivate();
                objectHandler = null;
            }
        }
        
    }
    private void SetValues(GameObject objectHandeling) {
        objectHandeling.GetComponent<ObjectHandler>().SetAnchor(gameObject);
        objectHandeling.GetComponent<ObjectHandler>().SetMesh(mesh);
        objectHandeling.GetComponent<ObjectHandler>().SetEndTransform(endTransform);
        objectHandler = objectHandeling;
    }
}
