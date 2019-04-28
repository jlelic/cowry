using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEntrance : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        GameManager.Instance.DoorEntranceList.Add(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.Instance.DoorEntranceList.Remove(gameObject);
    }

    public void Knock()
    {
        audioSource.Play();
    }
}
