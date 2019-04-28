using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MessageManager : MonoBehaviour
{
    [SerializeField] GameObject messagePanel;
    [SerializeField] Text messageText;

    Queue<string> messageQueue = new Queue<string>();
    private string currentMessage;
    private PlayerController playerController;

    void Awake()
    {
        messagePanel.SetActive(false);
        playerController = FindObjectOfType<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (currentMessage == null && messageQueue.Count > 0)
        {
            currentMessage = messageQueue.Dequeue();
        }
        var active = currentMessage != null;
        messagePanel.SetActive(active);
        playerController.enabled = !active;
        if(!active)
        {
            return;
        }
        messageText.text = currentMessage;

        if (Input.anyKeyDown)
        {
            currentMessage = null;
        }
    }

    public void AddMessage(string message)
    {
        messageQueue.Enqueue(message);
    }
}
