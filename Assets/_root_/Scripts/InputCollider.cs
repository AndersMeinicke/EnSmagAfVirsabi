using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static OVRInput;
public class InputCollider : MonoBehaviour
{
    [SerializeField] public bool isOn;
    public Controller thisController;
    UnityEvent ContinueStart = new UnityEvent();
    [SerializeField] private GameObject leftTutorial;
    [SerializeField] private GameObject rightTutorial;
    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
        ContinueStart_AddListener("ContinueStart", GameObject.Find("StarterFlow"));
    }

    // Update is called once per frame
    void Update()
    {
        if(isOn == true && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, thisController))
        {
            ContinueStart.Invoke();
            leftTutorial.SetActive(false);
            rightTutorial.SetActive(false);
        }
    }
    public void ContinueStart_AddListener(string message, GameObject machine)
    {
        ContinueStart.AddListener(() => machine.GetComponent<ScriptMachine>().TriggerUnityEvent(message));
    }
}
