using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4 : LevelManager
{
    [SerializeField] private GameObject merchant;
    private int patchesRemaining = 10;
    private float stage2TimeLeft = 90;
    float timeElapsed;
    bool introPlayed = false;
    bool stage1Completed = false;

    void Start()
    {
        merchant.SetActive(false);
        messageManager.AddMessage("It's time to get even fatter! Keep eating grass until your fatness level increases.");
        messageManager.AddMessage("You can see your fatness level in top left corner.");
        messageManager.AddMessage("And don't forget to keep out the poor suitors!", "initial");
    }

    public override void OnFatnessLevelChanged(int level)
    {
        if(level == 2)
        {
            stage1Completed = true;
            gameManager.ClearObjective();
            messageManager.AddMessage("Keep in mind that as you get fatter you're also getting slower!");
            messageManager.AddMessage("Now wait for a rich suitor to come and protect him until he gets to the door", "protect");
        }
    }

    public override void OnMessageCompleted(string messageId)
    {
        switch (messageId)
        {
            case "initial":
                gameManager.SetObjective("Get fatter");
                break;
            case "protect":
                gameManager.SetObjective("Get gifted to a rich suitor");
                break;
        }
    }

    void FixedUpdate()
    {
        if (stage1Completed)
        {
            timeElapsed += Time.deltaTime;
        }
        if(timeElapsed > 20)
        {
            merchant.SetActive(true);
        }
    }
}
