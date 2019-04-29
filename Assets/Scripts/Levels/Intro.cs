using UnityEngine.SceneManagement;

public class Intro : LevelManager
{

    void Start()
    {
        messageManager.AddMessage("They took your baby... again");
        messageManager.AddMessage("Five years in a row and it's still not getting easier");
        messageManager.AddMessage("This time it looked so hopeful too. Plenty of food everywhere around, it's not like they needed more");
        messageManager.AddMessage("But then young prince came, said he wants to take it");
        messageManager.AddMessage("Who can say no to the prince? Who can oppose him?");
        messageManager.AddMessage("...");
        messageManager.AddMessage("You can", "initial");
    }

    public override void OnMessageCompleted(string messageId)
    {
        SceneManager.LoadScene(1);
    }
}
