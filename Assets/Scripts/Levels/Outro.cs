using UnityEngine;
using UnityEngine.SceneManagement;

public class Outro : LevelManager
{

    void Start()
    {
        messageManager.AddMessage("The last suitor that got you as a dowry killed you right after the wedding");
        messageManager.AddMessage("You never got to see your baby again");
        messageManager.AddMessage("\t\t\tthe end");
        messageManager.AddMessage(@"
        Credits:

    Jozef Lelic - development
    joseph.lelic@gmail.com

    michal durovec - lead artist
    vakovlk@gmail.com

    denis fedor - artist
    fedor.de@gmail.com
        ");
        messageManager.AddMessage("", "initial");
    }

    public override void OnMessageCompleted(string messageId)
    {
        Application.Quit();
    }
}
