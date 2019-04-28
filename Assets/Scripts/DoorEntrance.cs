using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEntrance : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private GameObject door;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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

    public void Knock()
    {
        if(!GameManager.Instance.IsPlaying)
        {
            return;
        }
        audioSource.Play();
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
