using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [ButtonMethod]
    private void Spawn()
    {
        Instantiate(prefab, transform);
    }
}
