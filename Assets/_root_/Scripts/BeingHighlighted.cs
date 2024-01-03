using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeingHighlighted : MonoBehaviour
{
    private Sequence sequence;
    [SerializeField] private Animator animator;
    private GameObject beforeLocation = null;
    private GameObject afterLocation = null;
    public string nameOfThreat;
    private bool isActive = false;
    private bool sticky = false;
    public Canvas canvas;
    [SerializeField] private float moveDuration = 0.1f;
    private void Start()
    {
    }
    private void Update()
    {
        if (sticky) {
        this.transform.position = transform.parent.transform.position;
        }
    }
    public void Activate() {
        if (isActive == false) {
        sequence = DOTween.Sequence();
            sequence.AppendCallback(() => sticky = false);
            sequence.AppendCallback(() => canvas.overrideSorting = true);
            sequence.AppendCallback(() => canvas.sortingOrder =3);
            sequence.AppendCallback(() => isActive = true);
            sequence.AppendCallback(() => this.transform.SetParent(afterLocation.transform));
            //sequence.Append(gameObject.transform.DOMove(afterLocation.transform.position, 0.1f));
            sequence.AppendCallback(() => StartCoroutine(MoveToTargetPosition(afterLocation)));
            sequence.AppendCallback(() => sticky = true);
            sequence.AppendCallback(() => animator.SetTrigger("Highlighted"));
            sequence.Play();
            
        }
    }

public void Deactivate() {
        if (isActive) { 
        sequence = DOTween.Sequence();
            sequence.AppendCallback(() => sticky = false);
            sequence.AppendCallback(() => canvas.sortingOrder = 1);
            sequence.AppendCallback(() => animator.SetTrigger("NotHighlighted"));
        sequence.AppendInterval(0.2f);
            sequence.AppendCallback(() => this.transform.SetParent(beforeLocation.transform));
            //sequence.Append(gameObject.transform.DOMove(beforeLocation.transform.position, 0.1f));
            sequence.AppendCallback(() => StartCoroutine(MoveToTargetPosition(beforeLocation)));
            sequence.AppendCallback(() => sticky = true);
            sequence.AppendCallback(() => isActive =false);
            sequence.Play();
        }
    }
public void SetLocations(GameObject first, GameObject last) {
        beforeLocation = first;
        this.transform.SetParent(beforeLocation.transform);
        afterLocation = last;
    }
    private IEnumerator MoveToTargetPosition(GameObject targetObject)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            Vector3 targetPosition = targetObject.transform.position;
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object reaches the target position exactly at the end of the movement.
    }

}
