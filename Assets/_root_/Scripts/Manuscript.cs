using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class Manuscript : MonoBehaviour
{

    [SerializeField] public List<AudioClip> SoundParts = new List<AudioClip>();

   void Start()
    {
        
    }
    public void WhichPart(int part)
    {
       AudioClip selectedClip = SoundParts[part];
        if (selectedClip != null)
        {
            this.GetComponent<AudioSource>().clip = selectedClip;
            this.GetComponent<AudioSource>().Play();
        }
    }
}
