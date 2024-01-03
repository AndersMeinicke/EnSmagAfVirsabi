using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTransformer : MonoBehaviour
{
    [SerializeField]
    private MinMaxFloat scaleLimiter = new MinMaxFloat(0.3f, 1.5f);

    [ButtonMethod]
    private void RotateRandomly()
    {
        transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }

    [ButtonMethod]
    private void ScaleRandomly()
    {
        float factor = Random.Range(scaleLimiter.Min, scaleLimiter.Max);

        transform.localScale = new Vector3(factor, factor, factor);
    }
}
