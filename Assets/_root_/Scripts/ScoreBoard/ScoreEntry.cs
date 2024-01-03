using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyBox;

public class ScoreEntry : MonoBehaviour
{
    public TextMeshProUGUI userName, score;

    [SerializeField, ReadOnly]
    private Animator anim;

    private void OnValidate()
    {
        anim = GetComponent<Animator>();
    }

    public void ShowEntry() => anim.SetBool("Shown", true);
    public void HideEntry() => anim.SetBool("Shown", false);

}
