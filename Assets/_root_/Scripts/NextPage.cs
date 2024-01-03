using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPage : MonoBehaviour
{
   public Animator MakeDisappear;
   public Animator MakeAppear;

    public void StartAnimation()
    {
        MakeDisappear.SetTrigger("Disappear");
        MakeAppear.SetTrigger("Appear");
    }
}
