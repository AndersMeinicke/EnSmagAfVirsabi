using Oculus.Interaction.Body.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundThreats : MonoBehaviour
{
    public static FoundThreats Instance { get; private set; }
    public List<string> foundObjects;
    public float time;
    public int threats;
    public int totalThreats;
    public int sceneNumber;
    public int misclicks;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void FoundAThreat(string threat)
    {
        foundObjects.Add(threat);
    }
    public void FinishUp()
    {
        GameObject manager = GameObject.Find("Manager");
        time = manager.GetComponent<TimeManager>().getCalculatedTime();
        threats = manager.GetComponent<ThreatManager>().getThreatsFound();
        totalThreats = manager.GetComponent<ThreatManager>().getTotalThreats();
        misclicks = manager.GetComponent<TimeManager>().GetMisclicks();
    }
    public List<string> FinishSceneHelper()
    { return foundObjects; }
    public void DestroyList()
    {
        foundObjects.Clear();
    }

}
