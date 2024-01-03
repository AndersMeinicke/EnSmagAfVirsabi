using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class FadeInOut : MonoBehaviour
{
    public Image blackImage;
    public float fadeDuration = 5.0f;
    private bool readyToLoad;
    private bool firstLoad;
    private Sequence sequence;
    private AsyncOperation asyncOperation;
    [SerializeField] private AudioMixerController audioMixerController;

    private void Start()
    {
        FadeIn();
        readyToLoad = false;
        firstLoad = true;
    }
    public void FadeIn()
    {
        StartCoroutine(Fade(1, 0));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(0, 1));
    }
    public void levelLoader(int sceneNumber)
    {
        if(firstLoad) {
            readyToLoad = false;
            firstLoad = false;
            FadeOut();
            StartCoroutine(LoadLevelAsync(sceneNumber));
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0.0f;

        Color startColor = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, startAlpha);
        Color endColor = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, endAlpha);
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            blackImage.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            yield return null;
        }
            readyToLoad = true;
    }
    IEnumerator LoadLevelAsync(int sceneNumber)
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneNumber);
        asyncOperation.allowSceneActivation = false;
        if (audioMixerController != null) { audioMixerController.turnOff(fadeDuration); }
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f && readyToLoad)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}