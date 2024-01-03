using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombinedHeight : MonoBehaviour
{
    public List<Image> targetImage;

    private void Update()
    {
        float combinedHeight = 0;
        VerticalLayoutGroup layoutGroup = GetComponent<VerticalLayoutGroup>();

        foreach (RectTransform child in transform)
        {
            combinedHeight += child.rect.height;
        }

        combinedHeight += layoutGroup.spacing * (transform.childCount - 1);
        foreach(Image image in targetImage) {
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, combinedHeight);
        }
    }
}
