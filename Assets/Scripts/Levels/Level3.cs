using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3 : LevelManager
{
    [SerializeField] private int patchesRemaining = 10;
    [SerializeField] private float stage1TimeLeft = 90;
    [SerializeField] private GameObject merchant;
    float timeElapsed;
    bool introPlayed = false;
    bool stage1Completed = false;

    void Start()
    {
        merchant.SetActive(false);
        messageManager.AddMessage("Good news! Your owner is doing so well he managed to expand this property");
        messageManager.AddMessage("Bad news! Expansion means more competition for you. More cows to eat your grass, more poor suitors to come for owner's daughter");
        messageManager.AddMessage("Continue establishing dominance and keep out the poor suitors!", "initial");
    }

    public override void OnGrassEaten(bool isPlayer)
    {
        if(isPlayer)
        {
            return;
        }
        patchesRemaining--;
        if(patchesRemaining < 0)
        {
            gameManager.GameOver("Other cows ate too many of your grass patches, Loser!");
        }
    }

    public override void OnMessageCompleted(string messageId)
    {
        switch (messageId)
        {
            case "initial":
                introPlayed = true;
                break;
            case "merchantComing":
                merchant.SetActive(true);
                cameraManager.SetTarget(merchant.transform.position);
                break;
            case "protect":
                cameraManager.ClearTarget();
                gameManager.SetObjective("Let merchant get to the door first");
                break;
        }
    }

    void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;
        if (!stage1Completed && stage1TimeLeft <= 0)
        {
            gameManager.ClearObjective();
            stage1Completed = true;
            messageManager.AddMessage("A rich merchant from another village heard about daughter's beauty, he's coming to ask for her hand!", "merchantComing");
            messageManager.AddMessage("Make sure he's the only one who manages to get to the door!", "protect");
        }
    }

    void Update()
    {
        if(introPlayed && !stage1Completed)
        {
            stage1TimeLeft -= Time.deltaTime;
            gameManager.SetObjective(string.Format("{0}, Patches remaining: {1}", Utils.ToTimeString(stage1TimeLeft), patchesRemaining));
        }
    }
}
