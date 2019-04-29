using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int LevelHalfWidth = 7;
    public int LevelHalfHeight = 4;
    [SerializeField] private float grassSpawnInterval = 10;
    [SerializeField] private float suitorsSpawnInterval = 1;
    [SerializeField] private float initialFatness = 1;
    [SerializeField] private float grassFatIncrease = 7;

    protected GameManager gameManager;
    protected MessageManager messageManager;
    protected CameraManager cameraManager;

    void Awake()
    {
        messageManager = FindObjectOfType<MessageManager>();
        cameraManager = FindObjectOfType<CameraManager>();
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
    }

    public float GrassFatIncrease
    {
        get { return grassFatIncrease; }
        private set { grassFatIncrease = value; }
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
        gameManager.GameOver("YOU DESTROYED YOUR ONLY WAY OUT", 2);
    }

    virtual public void OnGrassEaten(bool isPlayer)
    {

    }

    virtual public void OnCowStunned()
    {

    }

    virtual public void OnPoorSuitorEntered()
    {
        gameManager.GameOver("You got gifted as a dowry to a poor person. Now you will never see your baby again.", 3);
    }

    virtual public void OnRichSuitorEntered()
    {
        gameManager.LevelCompleted(3);
    }

    virtual public void OnFatnessLevelChanged(int currentFatnessLevel)
    {

    }

    virtual public void OnMessageCompleted(string messageId) {

    }
}
