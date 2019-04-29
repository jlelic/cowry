using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5 : LevelManager
{
    [SerializeField] private GameObject merchant;
    float timeElapsed;
    bool introPlayed = false;
    bool stage1Completed = false;

    void Start()
    {
        merchant.SetActive(false);
        messageManager.AddMessage("You know the drill. Now let's get even fatter!", "initial");
    }

    public override void OnFatnessLevelChanged(int level)
    {
        if(level == 3)
        {
            stage1Completed = true;
            gameManager.ClearObjective();
            messageManager.AddMessage("Great job! You are now so chonky that you won't get stunned if you hit a rock!", "protect");
        }
    }

    public override void OnMessageCompleted(string messageId)
    {
        switch (messageId)
        {
            case "initial":
                gameManager.SetObjective("Get even fatter");
                break;
            case "protect":
                gameManager.SetObjective("Get gifted to a rich suitor");
                break;
        }
    }

    void FixedUpdate()
    {
        suitorsSpawnInterval -= Time.deltaTime/30f;
        if (stage1Completed)
        {
            timeElapsed += Time.deltaTime;
        }
        if(timeElapsed > 10)
        {
            merchant.SetActive(true);
        }
    }
}
