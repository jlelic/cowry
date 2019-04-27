using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEntrance : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.DoorEntranceList.Add(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.Instance.DoorEntranceList.Remove(gameObject);
    }
}
