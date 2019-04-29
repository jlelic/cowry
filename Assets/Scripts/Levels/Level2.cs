using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : LevelManager
{
    public GameObject son;
    int patchesEaten = 0;
    float timeElapsed;
    bool introPlayed = false;
    bool stage1Completed = false;

    void Start()
    {
        son.SetActive(false);
        messageManager.AddMessage("Your owner likes you enough to moove you closer to his house!");
        messageManager.AddMessage("The next step is to establish dominance within the herd");
        messageManager.AddMessage("Eat all of the patches of grass you can find and don't allow other cows to eat them first");
        messageManager.AddMessage("If they eat more than 5 patches you will be forever known as a loser and noone will ever want to buy you", "initial");
    }

    public override void OnGrassEaten(bool isPlayer)
    {
        if(isPlayer || stage1Completed)
        {
            return;
        }
        patchesEaten++;
        if (patchesEaten == 5)
        {
            gameManager.SetObjective("Don't let other cows eat any grass patch!");
            return;
        }
        if (patchesEaten > 5)
        {
            gameManager.ClearObjective();
            gameManager.GameOver("Other cows ate too many of your grass patches, Loser!");
        }

        gameManager.SetObjective(string.Format("Don't let other cows eat grass patches!\n{0} remaining", 5-patchesEaten));
    }

    public override void OnPoorSuitorKilled()
    {
        gameManager.LevelCompleted(2);
    }

    public override void OnMessageCompleted(string messageId)
    {
        switch (messageId)
        {
            case "initial":
                gameManager.SetObjective("Don't let other cows eat grass patches!\n5 remaining");
                break;
            case "sonComing":
                son.SetActive(true);
                cameraManager.SetTarget(son.transform.position);
                break;
            case "killSon":
                cameraManager.ClearTarget();
                gameManager.SetObjective("Stop the poor suitor before he get's to the door!");
                break;
        }
    }

    public void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;
        if(!stage1Completed && timeElapsed > 60)
        {
            gameManager.ClearObjective();
            stage1Completed = true;
            messageManager.AddMessage("Oh no! Neighbor's son is coming to ask for your owner's daughter's hand!", "sonComing");
            messageManager.AddMessage("Since you are the best cow around here, chances are you'll be gifted as a dowry");
            messageManager.AddMessage("Neighbor's son is very poor, there's no way you could get to royal barn if he gets you! Stop him at any cost!", "killSon");
        }
    }
}
