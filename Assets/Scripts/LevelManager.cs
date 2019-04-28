using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private float grassSpawnInterval = 10;
    [SerializeField] private float suitorsSpawnInterval = 1;
    [SerializeField] private float initialFatness = 1;

    protected GameManager gameManager;
    protected MessageManager messageManager;

    void Awake()
    {
        messageManager = FindObjectOfType<MessageManager>();
        gameManager = FindObjectOfType<GameManager>();
        if (!gameManager)
        {
            throw new System.Exception("GAME MANAGER NOT FOUND!");
        }
    }

    void Start()
    {
        messageManager.AddMessage("Hello world!");
    }

    public float GrassSpawnInterval
    {
        get { return grassSpawnInterval; }
        private set { grassSpawnInterval = value; }
    }

    public float SuitorSpawnInterval
    {
        get { return suitorsSpawnInterval; }
        private set { suitorsSpawnInterval = value; }
    }

    public float InitialFatness
    {
        get { return initialFatness; }
        private set { initialFatness = value; }
    }

    virtual public void OnSuitorSpawned()
    {

    }

    virtual public void OnGrassSpawned()
    {

    }

    virtual public void OnPoorSuitorKilled()
    {

    }

    virtual public void OnRichSuitorKilled()
    {
        gameManager.GameOver("YOU DESTROYED YOUR ONLY WAY OUT");
    }

    virtual public void OnGrassEaten()
    {

    }

    virtual public void OnCowStunned()
    {

    }

    virtual public void OnPoorSuitorEntered()
    {
        gameManager.GameOver("YOU GOT SOLD TO A POOR PERSON, GOOD LUCK");
    }

    virtual public void OnRichSuitorEntered()
    {
        gameManager.LevelCompleted();
    }
}
