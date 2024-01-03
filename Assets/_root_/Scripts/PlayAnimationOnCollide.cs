using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnCollide : MonoBehaviour
{
    public ParticleSystem particleSystem;
    private void OnCollisionEnter(Collision collision)
    {
        particleSystem.Play();
    }
}
