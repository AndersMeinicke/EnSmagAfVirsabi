using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public bool finishUp;
    public ParticleSystem particleSystem;
    public bool party;
    public int scene;
    public FadeInOut fadeToScene;
    public Material oldMaterial;
    public Material newMaterial;
    [SerializeField] private GameObject orb;
    // Start is called before the first frame update
    public void Activate()
    {
        if(party == true)
        {
            this.GetComponent<AudioSource>().Play();
            particleSystem.Play();
        }
        if(finishUp) { 
        FoundThreats.Instance.FinishUp();
        }
        fadeToScene.levelLoader(scene);
    }
    public void Highlight()
    {
        if (oldMaterial != null)
        {
            orb.GetComponent<MeshRenderer>().material = newMaterial;
        }
    }
    public void nonHighlight()
    {

        orb.GetComponent<MeshRenderer>().material = oldMaterial;
    }
}
