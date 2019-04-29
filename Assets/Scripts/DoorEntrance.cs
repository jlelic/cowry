using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEntrance : MonoBehaviour
{
    [SerializeField] private GameObject door;

    private void Awake()
    {
        door.SetActive(false);
    }

    void Start()
    {
        GameManager.Instance.DoorEntranceList.Add(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.Instance.DoorEntranceList.Remove(gameObject);
    }

    public void Open()
    {
        if (!GameManager.Instance.IsPlaying)
        {
            return;
        }
        door.SetActive(true);
        FindObjectOfType<CameraManager>().SetTarget(door.transform.position);
    }
}
