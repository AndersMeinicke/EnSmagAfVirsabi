using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class ObjectHandler : MonoBehaviour
{

    [SerializeField] private Transform startTransform;
    [SerializeField] private GameObject objectVisual;
    [SerializeField] private GameObject completed;
    [SerializeField] private Animator description;
    [SerializeField] private bool isNonMovable;
    [SerializeField] private bool isNeutral = false;
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float z;
    private Transform endTransform = null;
    private GameObject anchor = null;
    private GameObject mesh = null;
    private bool IsActive = false;
    private float duration = 0.25f;
    private Sequence sequence;
    private bool isMoving = false;

    private bool firstTime = true;
    [SerializeField] private bool isThreat;
    public void Activate()
    {
       sequence = DOTween.Sequence();
        if(!isNonMovable ) {
        sequence.AppendCallback(() => StartCoroutine(MoveToHand()));
        }
        sequence.AppendCallback(() => mesh.GetComponent<SkinnedMeshRenderer>().enabled =false);
        sequence.AppendCallback(() => anchor.GetComponent<LineRenderer>().enabled = false);
        sequence.AppendCallback(() => description.SetTrigger("Show"));
        if (firstTime && isThreat)
        {
            sequence.AppendCallback(() => GameObject.Find("FinishScreenHelper").GetComponent<FoundThreats>().FoundAThreat(objectVisual.name));
            ThreatManager threatManager = GameObject.Find("Manager").GetComponent<ThreatManager>();
            threatManager.setThreatsFound();
            firstTime = false;
        }
        sequence.AppendInterval(0.5f);
        sequence.AppendCallback(() => IsActive = true);
        //Invoke("Deactivate", 7f);
    }

    public void Deactivate()
    {
        sequence = DOTween.Sequence();
        sequence.AppendCallback(() => mesh.GetComponent<SkinnedMeshRenderer>().enabled=true);
        sequence.AppendCallback(() => anchor.GetComponent<LineRenderer>().enabled =true);
        sequence.AppendCallback(() => description.SetTrigger("Hide"));
        sequence.AppendCallback(() => endTransform.localEulerAngles = Vector3.zero);
        sequence.AppendCallback(() => objectVisual.transform.SetParent(transform));
        sequence.Append(objectVisual.transform.DOMove(startTransform.position, 0.5f));
        sequence.Insert(0f, objectVisual.transform.DORotate(startTransform.rotation.eulerAngles, 1f));
        if (isThreat) {
        sequence.AppendCallback(() => completed.GetComponent<MeshRenderer>().enabled = true);
        }
        sequence.AppendCallback(() => IsActive = false);
        sequence.Play();
    }
     public IEnumerator MoveToHand()
    {
        isMoving = true;
        float elapsedTime = 0;
        float speed = Vector3.Distance(objectVisual.transform.position, endTransform.position) / duration;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            objectVisual.transform.position = Vector3.MoveTowards(objectVisual.transform.position, new Vector3(endTransform.position.x, endTransform.position.y, endTransform.position.z), speed * Time.deltaTime);
            yield return null;
        }

        objectVisual.transform.position = endTransform.position;
        endTransform.rotation = endTransform.rotation * Quaternion.Euler(x,y,z);
        objectVisual.transform.rotation = endTransform.rotation;
        objectVisual.transform.SetParent(endTransform);
       // endTransform.rotation = Quaternion.Euler(objectVisual.transform.rotation.x + x, objectVisual.transform.rotation.y + y, objectVisual.transform.rotation.z + z); //new Quaternion(endTransform.rotation.x+x, endTransform.rotation.y+y, endTransform.rotation.z+z, endTransform.rotation.w);
       // objectVisual.transform.rotation = endTransform.rotation;
        isMoving = false;
        }
    public bool getIsThreat()
    {
        return isThreat;
    }
    public bool getIsNeutral()
    {
        return isNeutral;
    }
    public bool getIsActive()
    {
        return IsActive;
    }
    public void SetAnchor(GameObject newAnchor)
    {
        anchor = newAnchor;
    }
    public void SetMesh(GameObject newMesh)
    {
        mesh = newMesh;
    }
    public void SetEndTransform(Transform newEndTransform)
    {
        endTransform = newEndTransform;
    }
}
