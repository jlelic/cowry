using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : LevelManager
{
    int patchesEaten = 0;
    float timeWithoutAction = 0;
    [SerializeField] private GameObject aiCow;

    void Start()
    {
        aiCow.SetActive(false);
        messageManager.AddMessage("After all the years you have lived here you know there's no way you can escape out of here to get to your baby.");
        messageManager.AddMessage("However maybe if you got sold somewhere else...");
        messageManager.AddMessage("Look at you! Who would buy such skinny sickly cow as you are? You need to eat something!");
        messageManager.AddMessage("There a patch of grass right next to you. Move closer using W, A, S, D and then pres E to eat it.", "initial");
    }

    public override void OnGrassEaten(bool isPlayer)
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
            aiCow.SetActive(true);
            cameraManager.SetTarget(aiCow.transform.position);
            messageManager.AddMessage("Oh no! Another cow came here to eat your grass!");
            messageManager.AddMessage("Charge into her to stun her!");
            messageManager.AddMessage("Be careful though, if you hit a big rock while charging you'll be stunned instead!", "chargeIntoCow");
        }
    }

    public override void OnCowStunned()
    {
        gameManager.LevelCompleted(1);
    }

    public override void OnMessageCompleted(string messageId)
    {
        switch (messageId)
        {
            case "initial":
                gameManager.SetObjective("Eat 3 patches of grass");
                break;
            case "chargeIntoCow":
                cameraManager.ClearTarget();
                gameManager.SetObjective("Show the other cow who's the boss");
                break;
        }
    }

    private void FixedUpdate()
    {
        if(patchesEaten > 0)
        {
            return;
        }
        timeWithoutAction += Time.deltaTime;
        if(timeWithoutAction > 20)
        {
            messageManager.AddMessage("Move close to a patch of grass using W, A, S, D and then pres E to eat it.");
            timeWithoutAction = 0;
        }
    }
}
