using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThreatShower : MonoBehaviour
{
    public GameObject Manager;
    private ThreatManager ThreatManager;
    public TextMeshProUGUI ThreatShowcase;
    // Start is called before the first frame update
    void Start()
    {
        ThreatManager = Manager.GetComponent<ThreatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ThreatShowcase.text = $"{ThreatManager.getThreatsFound()}/{ThreatManager.getTotalThreats()}";
    }
}
