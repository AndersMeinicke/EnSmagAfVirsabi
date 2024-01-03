using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] GameObject backgroundFound;
    [SerializeField] GameObject backgroundMissed;
    [SerializeField] GameObject ScoreBoard;
    [SerializeField] GameObject Stats;
    [SerializeField] GameObject MissedHeader;
    private List<string> nameFinder;
    public GameObject foundList;
    public GameObject notFoundList;
    private Vector3 foundListOrigin;
    private Vector3 notFoundListOrigin;
    public List<GameObject> listOfThreats;
    private int counterFound;
    private int counterNotFound;
    private float scalingFound;
    private float scalingNotFound;
    // Start is called before the first frame update
    void Start()
    {
        scalingFound = 1;
        scalingNotFound = 1;
        RectTransform scale = backgroundFound.GetComponent<RectTransform>();
        RectTransform scaleNot = backgroundMissed.GetComponent<RectTransform>();
        RectTransform header = MissedHeader.GetComponent<RectTransform>();
        RectTransform scoreRect = ScoreBoard.GetComponent<RectTransform>();
        Debug.Log(foundList.transform.position);
        Vector3 foundListVector = foundList.transform.position;
        Vector3 notFoundListVector = notFoundList.transform.position;
        foundListOrigin = foundList.transform.position;
        notFoundListOrigin = notFoundList.transform.position;
        nameFinder = FoundThreats.Instance.FinishSceneHelper();
        foreach (GameObject threat in listOfThreats) {
            if (nameFinder.Contains(threat.GetComponent<BeingHighlighted>().nameOfThreat))
            {
                counterFound++;
                if(counterFound > 4)
                {
                foundListVector = new Vector3(foundListOrigin.x, foundListVector.y - 0.3f, foundListOrigin.z);
                    counterFound = 0;
                    scalingFound++;
                }
                threat.transform.SetParent(foundList.transform);
                threat.transform.position = foundListVector;
                GameObject victoryQueue = new GameObject("VictoryBeforeQueue");
                victoryQueue.transform.SetParent(foundList.transform);
                victoryQueue.transform.position = foundListVector;
                GameObject victoryQueueAfter = new GameObject("VictoryAfterQueue");
                victoryQueueAfter.transform.SetParent(foundList.transform);
                victoryQueueAfter.transform.position = new Vector3(foundListVector.x,foundListVector.y,foundListVector.z-0.05f);
                threat.GetComponent<BeingHighlighted>().SetLocations(victoryQueue, victoryQueueAfter);
                foundListVector = new Vector3(foundListVector.x + 0.25f, foundListVector.y, foundListVector.z);

            }
            else
            {
                counterNotFound++;
                if (counterNotFound > 4)
                {
                    notFoundListVector = new Vector3(notFoundListOrigin.x, notFoundListVector.y - 0.3f, notFoundListOrigin.z);
                    counterNotFound = 0;
                    scalingNotFound++;
                }
                threat.transform.SetParent(notFoundList.transform);
                threat.transform.position = notFoundListVector;
                GameObject losersQueue = new GameObject("LosersBeforeQueue");
                losersQueue.transform.SetParent(notFoundList.transform);
                losersQueue.transform.position = notFoundListVector;
                GameObject losersQueueAfter = new GameObject("LosersAfterQueue");
                losersQueueAfter.transform.SetParent(notFoundList.transform);
                losersQueueAfter.transform.position = new Vector3(notFoundListVector.x, notFoundListVector.y, notFoundListVector.z - 0.05f);
                threat.GetComponent<BeingHighlighted>().SetLocations(losersQueue, losersQueueAfter);
                notFoundListVector = new Vector3(notFoundListVector.x + 0.25f,notFoundListVector.y, notFoundListVector.z);
            }
        }
        scaleNot.SetHeight(40f * scalingFound);
        scale.SetHeight(40f * scalingNotFound);
        /*float scaleHeight = (scale.rect.height * scale.pivot.y)*scale.localScale.y;
        float scaleNotHeight = scaleNot.rect.height * (1 - scaleNot.pivot.y);
        float headerHeight = header.rect.height * (1 - header.pivot.y);
        float headerY = scale.anchoredPosition.y;
        float newYPos = scale.anchoredPosition.y - scaleHeight - scaleNotHeight-header.rect.height;
        header.anchoredPosition = new Vector2(header.anchoredPosition.x, headerY);
        scaleNot.anchoredPosition = new Vector2(scaleNot.anchoredPosition.x,newYPos);
        backgroundMissed.GetComponent<RectTransform>().localScale = scaleNot.localScale;
        scoreRect.SetHeight(backgroundMissed.GetComponent<RectTransform>().rect.height + MissedHeader.GetComponent<RectTransform>().rect.height + backgroundFound.GetComponent<RectTransform>().rect.height);
        Stats.GetComponent<RectTransform>().SetHeight(scoreRect.rect.height);
        ScoreBoard.GetComponent<RectTransform>().SetHeight(scoreRect.rect.height);
        backgroundMissed.GetComponent<RectTransform>().anchoredPosition = scaleNot.anchoredPosition;
        MissedHeader.GetComponent<RectTransform>().anchoredPosition = header.anchoredPosition;*/
        FoundThreats.Instance.DestroyList();
    }
}
