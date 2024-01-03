using DG.Tweening.Core;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    public GameObject TimeShower;
    private float timeElapsed;
    private float calculationTime;
    public FadeInOut fadeToScene;
    public int scene;
    private int misclicks;
    private bool noMultiLoad = true;
    private bool stopTheTimer = false;
    void Start()
    {
        timeElapsed = calculationTime = 0f;
    }

    void Update()
    {
        if (stopTheTimer == false) {
            timeElapsed += Time.deltaTime;
            calculationTime += Time.deltaTime;
        }
        if (timeElapsed > Time.deltaTime + 180f && noMultiLoad == true)
        {

            noMultiLoad = false;
            stopTheTimer = true;
            EndScene();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            noMultiLoad = false;
            stopTheTimer = true;
            EndScene();
        }
    }


    public float GetTimeElapsed()
    {
        return timeElapsed;
    }
    public float getCalculatedTime()
    {
        return calculationTime;
    }
    public void Punishment()
    {
        AudioSource audio = GameObject.Find("Global Volume").GetComponent<AudioSource>();
        audio.clip = clip;
        calculationTime = calculationTime + 5;

        audio.Play();
        misclicks++;
    }
    public int GetMisclicks()
    {
        return misclicks;
    }
    public void EndScene()
    {
        FoundThreats.Instance.FinishUp();
        fadeToScene.levelLoader(scene);
    }
    public void MakeTimerStop()
    {
        TimeShower.GetComponent<TimeShowcase>().stopCounting = true;
    }
}