using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : LevelManager
{
    int patchesEaten = 0;
    [SerializeField] private GameObject aiCow;

    void Start()
    {
        aiCow.SetActive(false);
        messageManager.AddMessage("After all the years you have lived here you know there's no way you can escape out of here to get to your baby.");
        messageManager.AddMessage("However maybe if you got sold somewhere else...");
        messageManager.AddMessage("Look at you! Who would buy such skinny sickly cow as you are? You need to eat something!");
        messageManager.AddMessage("There a patch of grass in front of you. Move closer using W, A, S, D and then pres E to eat it.", "initial");
    }

    public override void OnGrassEaten()
    {
        patchesEaten++;
        if (patchesEaten == 1)
        {
            gameManager.SetObjective("Eat 2 more patches of grass");
        }
        if (patchesEaten == 2)
        {
            gameManager.SetObjective("Eat 1 last patch of grass");
            messageManager.AddMessage("You can use SPACE to charge towards mouse cursor");
            messageManager.AddMessage("That way you can move faster or even deal damage to whatever you hit!");
        }
        if (patchesEaten == 3)
        {
            gameManager.ClearObjective();
            messageManager.AddMessage("Oh no! Another cow came here to eat your grass!");
            messageManager.AddMessage("Charge into her to stun her!");
            messageManager.AddMessage("Be careful though, if you hit a big rock while charging you'll be stunned instead!", "chargeIntoCow");
        }
    }

    public override void OnCowStunned()
    {
        gameManager.LevelCompleted();
    }

    public override void OnMessageCompleted(string messageId)
    {
        switch (messageId)
        {
            case "initial":
                gameManager.SetObjective("Eat 3 patches of grass");
                break;
            case "chargeIntoCow":
                aiCow.SetActive(true);
                gameManager.SetObjective("Show the other cow who's the boss");
                break;
        }
    }
}
