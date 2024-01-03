using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectionFeedbackController : MonoBehaviour
{
    [SerializeField]
    private string NoInternetAtAllMessage = "Headsettet har ikke adgang til internettet. Vær venlig at log på et WiFi netwærk.",
        serverDownMessage = "Det ser ud som om vores service er under vedligeholdelse. Prøv igen senere.";

    [SerializeField]
    private Animator ProcessingFeedback, SuccessPanel, FailPanel;

    [SerializeField, ReadOnly]
    private Animator panelAnimator;

    [SerializeField]
    private TextMeshProUGUI successMessage, FailMessage;

    private void OnValidate()
    {
        panelAnimator = GetComponent<Animator>();
    }

    public void HandleFail()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            FailMessage.text = NoInternetAtAllMessage;
        }
        else //if we have internet but we still got an error, we assume the server we are trying to communicate with is down
        {
            FailMessage.text = serverDownMessage;
        }

        FailPanel.SetBool("Shown", true);
    }

    /// <summary>
    /// Call this to show a message on success
    /// </summary>
    /// <param name="message"></param>
    public void ShowSuccess(string message, float autoHideAfter)
    {
        ProcessingFeedback.SetBool("Shown", false);
        SuccessPanel.SetBool("Shown", true);
        successMessage.text = message;
        Invoke(nameof(HideConnectionFeedbackPanel), autoHideAfter);
    }

    [ButtonMethod]
    public void ShowConnectionFeedbackPanelWithLoading()
    {
        panelAnimator.SetBool("Shown", true);
        ProcessingFeedback.SetBool("Shown", true);
        SuccessPanel.SetBool("Shown", false);
        FailPanel.SetBool("Shown", false);
    }

    [ButtonMethod]
    public void HideConnectionFeedbackPanel()
    {
        panelAnimator.SetBool("Shown", false);
        ProcessingFeedback.SetBool("Shown", false);
        SuccessPanel.SetBool("Shown", false);
        FailPanel.SetBool("Shown", false);
    }
}
