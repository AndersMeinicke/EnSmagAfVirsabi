using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LongLoading : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDown;
    [SerializeField] private GameObject enviormentOne;
    [SerializeField] private GameObject enviormentTwo;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject Light;
    private int sceneNumber;
    private Sequence sequence;
    // Start is called before the first frame update
    void Start()
    {
        sequence = DOTween.Sequence();
        sceneNumber = FoundThreats.Instance.sceneNumber;
        sequence.AppendCallback(() => countDown.text = "4");
        sequence.AppendInterval(1.5f);
        sequence.AppendCallback(() => Light.SetActive(false));
        sequence.AppendCallback(() => countDown.text = "3");
        sequence.AppendInterval(1.5f);
        sequence.AppendCallback(() => enviormentOne.SetActive(false));
        sequence.AppendCallback(() => countDown.text = "2");
        sequence.AppendInterval(1.5f);
        sequence.AppendCallback(() => enviormentTwo.SetActive(false));
        sequence.AppendCallback(() => countDown.text = "1");
        sequence.AppendInterval(1.5f);
        sequence.AppendCallback(() => StartCoroutine(LoadLevelAsync()));
        sequence.Play();
    }

    IEnumerator LoadLevelAsync()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneNumber);
        while (!loadOperation.isDone) {
             yield return null;
                }
    }
}
