using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Virsabi;

public class FeedbackSession : MonoBehaviour
{
    public static FeedbackSession Instance;
    [SerializeField]
    private GameObject UploadButton, secondUploadButton;

    [SerializeField, DisplayInspector]
    public float time;
    [SerializeField, DisplayInspector]
    public string challenge;

    [SerializeField, ReadOnly]
    private List<DataClasses.TopScoreEntry> topScores = new List<DataClasses.TopScoreEntry>();

    [SerializeField]
    public string UserName;

    [SerializeField]
    public int TotalScore = 0;

    [SerializeField, HideInInspector]
    private ConnectionFeedbackController connectionFeedbackController;

    [SerializeField]
    private UnityEvent OnSuccessfullTopscorePost;
    #region Mono Methods

    private void OnValidate()
    {
        connectionFeedbackController = FindObjectOfType<ConnectionFeedbackController>();
    }

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);
    }
    #endregion

    #region TopScore Interface

    [ButtonMethod]
    public void SubmitScore()
    {
        connectionFeedbackController.ShowConnectionFeedbackPanelWithLoading();
        DataClasses.TopScoreEntry entry = new DataClasses.TopScoreEntry(UserName, time, TotalScore, challenge);

        ServiceLocator.Instance.GetService<IDBConnection>()
            .PostItem(entry, OnPostResponse);
    }

    private void OnPostResponse(bool successFull, string response)
    {
        Debug.Log("Post Response: " + response);
        if (successFull)
        {
            //if success disable new uploads!
            UploadButton.SetActive(false);
            secondUploadButton.SetActive(false);
            connectionFeedbackController.ShowSuccess("Uploadet!", 1f);
            OnSuccessfullTopscorePost.Invoke();
        }
        else
        {
            connectionFeedbackController.HandleFail();
        }
    }

    [ContextMenu("InvokeOnSuccessfullTopscorePost")]
    private void InvokeOnSuccessfullTopscorePost()
    {
        OnSuccessfullTopscorePost.Invoke();
    }

    [ButtonMethod]
    private void TestGetService()
    {
        ServiceLocator.Instance.GetService<IDBConnection>()
            .GetTableAsList<DataClasses.TopScoreEntry>(OnTopScoresUpdated, ResponseHandler);
    }

    /// <summary>
    /// Handles potential errors
    /// </summary>
    /// <param name="successFull"></param>
    /// <param name="response"></param>
    private void ResponseHandler(bool successFull, string response)
    {

    }

    private void OnTopScoresUpdated(List<DataClasses.TopScoreEntry> listResult)
    {
        topScores = listResult;
    }
    #endregion
}

