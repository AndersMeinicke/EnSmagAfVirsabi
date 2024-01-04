using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using static OVRInput;

public class ClipSender : MonoBehaviour
{
    [SerializeField] public bool isOn;
    [SerializeField] public bool isDone;
    public VideoClip clip;
    [SerializeField] private AudioSource clipAudio;
    public Controller thisController;
    UnityEvent DoneWithVideo = new UnityEvent();
    public FadeInOut fadeInOut;
    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
        isDone = false;
        ContinueStart_AddListener("DoneWithVideo", GameObject.Find("360"));
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn == true && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, thisController))
        {
            GameObject.Find("VideoPlayer").GetComponent<VideoPlayer>().clip = clip;
            GameObject.Find("VideoPlayer").GetComponent<VideoPlayer>().Play();
            clipAudio.Play();
            //DoneWithVideo.Invoke();

        }
        else if(isDone == true && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, thisController)){
            fadeInOut.levelLoader(2);
        }
    }
    public void ContinueStart_AddListener(string message, GameObject machine)
    {
        DoneWithVideo.AddListener(() => machine.GetComponent<ScriptMachine>().TriggerUnityEvent(message));
    }
}
