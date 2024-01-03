using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRInput;

public class EndScreenHandler : MonoBehaviour
{
    [SerializeField] private AudioClip correctClip;
    private TouchScreenKeyboard keyboard;
    public string nameForScore;
    public LineRenderer lineRenderer;
    public Controller RightHandController;
    public GameObject cursor;

    private GameObject BeingHighlighted;
    // Start is called before the first frame update
    void Start()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    // Update is called once per frame
    void Update()
    {

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            cursor.SetActive(true);
            cursor.transform.position = hit.point;
            if (hit.collider.gameObject.GetComponent<BeingHighlighted>()!= null)
            {
                if(BeingHighlighted == null) { 
                hit.collider.gameObject.GetComponent<BeingHighlighted>().Activate();
                BeingHighlighted = hit.collider.gameObject;
                }
                else if (BeingHighlighted != hit.collider.gameObject)
                {
                    BeingHighlighted.GetComponent<BeingHighlighted>().Deactivate();
                    hit.collider.gameObject.GetComponent<BeingHighlighted>().Activate();
                    BeingHighlighted = hit.collider.gameObject;
                }
            }
            else if (BeingHighlighted != null) 
            {
                BeingHighlighted.GetComponent<BeingHighlighted>().Deactivate();
                BeingHighlighted=null;
            }
            if(hit.collider.gameObject.GetComponent<NextPage>() != null)
            {
                if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, RightHandController) || OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, RightHandController))
                {
                    AudioSource audio = GameObject.Find("Global Sound").GetComponent<AudioSource>();
                    audio.clip = correctClip;
                    audio.Play();
                    hit.collider.gameObject.GetComponent<NextPage>().StartAnimation();
                }
            }
            if (hit.collider.gameObject.GetComponent<SceneLoader>() != null)
            {
               
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, RightHandController) || OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, RightHandController))
                {
                    
                    hit.collider.gameObject.GetComponent<SceneLoader>().Activate();
                }
            }
        }
        }
    }
