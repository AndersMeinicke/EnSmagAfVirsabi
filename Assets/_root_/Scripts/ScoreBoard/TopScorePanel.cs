//using ElRaccoone.Timers;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Virsabi;

public class TopScorePanel : MonoBehaviour
{
    [SerializeField]
    private RectTransform TopScoresHolder;

    [SerializeField]
    private GameObject ScoreEntryPrefab;

    [SerializeField]
    private UnityEvent OnEachSpwawn;

    [SerializeField]
    private TMPro.TextMeshProUGUI yourScore;

    [SerializeField, ReadOnly]
    private List<ScoreEntry> topScores = new List<ScoreEntry>();


    [SerializeField, ReadOnly]
    private ConnectionFeedbackController connectionFeedbackController;

    private void OnValidate()
    {
        connectionFeedbackController = FindObjectOfType<ConnectionFeedbackController>();
    }

    [ButtonMethod]
    public void TryGetTopScores()
    {
        connectionFeedbackController.ShowConnectionFeedbackPanelWithLoading();

        yourScore.text = "" + FeedbackSession.Instance.TotalScore;

        ServiceLocator.Instance.GetService<IDBConnection>()
            .GetTableAsList<DataClasses.TopScoreEntry>(OnTopScoresUpdated, ResponseHandler);
    }

    [ButtonMethod]
    private void ShowTopscores()
    {
        StartCoroutine(DelayedAnimationTrigger());
    }
    private IEnumerator DelayedAnimationTrigger()
    {
        foreach (var item in topScores)
        {
            item.ShowEntry();
            OnEachSpwawn.Invoke();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ResponseHandler(bool successFull, string response)
    {
        if (!successFull)
        {
            Debug.LogError("Response: " + response);
            connectionFeedbackController.HandleFail();
        }
        else
        {
            Debug.Log("downloaded list");
            connectionFeedbackController.HideConnectionFeedbackPanel();
        }
    }

    private void OnTopScoresUpdated(List<DataClasses.TopScoreEntry> listResult)
    {
        if (listResult == null)
        {
            Debug.Log("FATAL ERROR - list is null");
            return;
        }
        Debug.Log("OnTopscoresUpdated: " + listResult.Count);
        //There's no complex querry function yet, so we just get all entries on the database currently. Here we remove all entries that are not matching with the score we just made.
        listResult.RemoveAll(x => x.ChallengeID != FeedbackSession.Instance.challenge);
        Debug.Log("after sort: " + listResult.Count);
        List<DataClasses.TopScoreEntry> top10Scores = listResult.OrderByDescending(x => x.Score).Take(10).ToList();


        //topScores.DeleteAllGameObjects();

        foreach (DataClasses.TopScoreEntry item in top10Scores)
        {
            ScoreEntry entry = Instantiate(ScoreEntryPrefab, TopScoresHolder).GetComponent<ScoreEntry>();
            entry.userName.text = item.UserName;
            entry.score.text = "" + item.Score;
            topScores.Add(entry);
        }

        ShowTopscores();
    }
}
