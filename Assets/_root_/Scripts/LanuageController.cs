using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using static OVRInput;

public class LanuageController : MonoBehaviour
{
    [SerializeField] public bool isOn;
    [SerializeField] public bool isDone;
    public int scene;
    public TextLanguageChanger changer;
    public Controller thisController;
    public FadeInOut fadeInOut;
    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
        isDone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn == true && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, thisController))
        {
            changer.Activate();
        }
        else if (isDone == true && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, thisController))
        {
            fadeInOut.levelLoader(scene);
        }
    }
}
