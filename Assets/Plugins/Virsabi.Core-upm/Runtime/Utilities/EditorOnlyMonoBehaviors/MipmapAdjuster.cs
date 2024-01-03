using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MipmapAdjuster : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("A positive bias makes a texture appear extra blurry, while a negative bias sharpens the texture. Note that using large negative bias can reduce performance, so it's not recommended to use more than -0.5 negative bias.)")]
    public float _target = -2f;

    [Header("References")]
    public Texture[] _texturesToAdjust;

    [ButtonMethod]
    public void Adjust()
    {
        foreach (Texture tex in _texturesToAdjust)
        {
            tex.mipMapBias = _target;
        }
    }
}
