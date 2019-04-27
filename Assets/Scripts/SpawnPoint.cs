using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.SpawnPointList.Add(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.Instance.SpawnPointList.Remove(gameObject);
    }
}
