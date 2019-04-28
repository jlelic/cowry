using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MessageManager : MonoBehaviour
{
    class GameMessage
    {
        public readonly string message;
        public readonly string id;
        public GameMessage(string message, string id = null)
        {
            this.message = message;
            this.id = id;
        }
    }

    [SerializeField] GameObject messagePanel;
    [SerializeField] Text messageText;

    Queue<GameMessage> messageQueue = new Queue<GameMessage>();
    private GameMessage currentMessage;
    private PlayerController playerController;
    private bool holdingConfirmButton = false;

    void Awake()
    {
        messagePanel.SetActive(false);
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
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
        Time.timeScale = 0;
        messageText.text = currentMessage.message;

        if (Input.GetAxisRaw("Confirm")>=1)
        {
            if(holdingConfirmButton)
            {
                return;
            }
            holdingConfirmButton = true;
            if (currentMessage.id != null)
            {
                GameManager.Instance.LevelManager.OnMessageCompleted(currentMessage.id);
            }
            currentMessage = null;
            Time.timeScale = 1;
        }
        else
        {
            holdingConfirmButton = false;
        }
    }

    public void AddMessage(string message, string messageId = null)
    {
        messageQueue.Enqueue(new GameMessage(message, messageId));
    }
}
