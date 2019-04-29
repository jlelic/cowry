using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level6 : LevelManager
{
    [SerializeField] private GameObject merchant;
    float timeElapsed;
    bool introPlayed = false;
    bool stage1Completed = false;
    bool chonkyShown = false;

    void Start()
    {
        merchant.SetActive(false);
        messageManager.AddMessage("Fill the fatness meter completely!");
        messageManager.AddMessage("Oh and be careful, now there are two houses to  keep an eye on!", "initial");
    }

    public override void OnFatnessChanged(float fatness)
    {
        if(fatness >= 99 && !chonkyShown)
        {
            chonkyShown = true;
            stage1Completed = true;
            gameManager.ClearObjective();
            messageManager.AddMessage("Awesome! Now you don't even need to charge to attack. Your mere presence is enough to stun anybody who get too close", "protect");
        }
    }

    public override void OnMessageCompleted(string messageId)
    {
        switch (messageId)
        {
            case "initial":
                gameManager.SetObjective("Get as fat as possible");
                break;
            case "protect":
                gameManager.SetObjective("Wait for a rich suitor");
                break;
        }
    }

    void FixedUpdate()
    {
        suitorsSpawnInterval -= Time.deltaTime/40f;
        if (stage1Completed)
        {
            timeElapsed += Time.deltaTime;
        }
        if(timeElapsed > 30)
        {
            merchant.SetActive(true);
        }
    }
}
