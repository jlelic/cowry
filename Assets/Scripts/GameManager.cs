using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool CanEndGame = false;

    const float GAME_HALF_WIDTH = 7;
    const float GAME_HALF_HEIGHT = 4;

    public static GameManager Instance { get; private set; }

    public bool IsPlaying { get; private set; } = true;
    public LevelManager LevelManager { get; private set; }
    public UiManager UiManager { get; private set; }
    public HashSet<GameObject> GrassList { get; private set; } = new HashSet<GameObject>();
    public HashSet<GameObject> NavPointList { get; private set; } = new HashSet<GameObject>();
    public HashSet<GameObject> DoorEntranceList { get; private set; } = new HashSet<GameObject>();
    public List<GameObject> SpawnPointList { get; private set; } = new List<GameObject>();

    [SerializeField] private GameObject grassPrefab;
    [SerializeField] private GameObject suitorPrefab;

    private PlayerController playerController;
    private MovementManager playerMovement;
    private Coroutine spawnGrassCoroutine;
    private Coroutine spawnSuitorsCoroutine;
    private int killStreakCount = 0;

    private void Awake()
    {
        GameManager.Instance = this;
        UiManager = GetComponent<UiManager>();

        var levelManagers = FindObjectsOfType<LevelManager>();
        if(levelManagers.Length == 0)
        {
            throw new System.Exception("LEVEL MANAGER NOT FOUND IN THE LEVEL!");
        }
        else if(levelManagers.Length > 1)
        {
            foreach(var manager in levelManagers)
            {
                Debug.LogWarning(manager);
            }
            throw new System.Exception("FOUND MULTIPLE LEVEL MANAGERS");
        }
        LevelManager = levelManagers[0];

        spawnGrassCoroutine = StartCoroutine(StartSpawnGrass());
        spawnSuitorsCoroutine = StartCoroutine(StartSpawnSuitors());

        playerController = FindObjectOfType<PlayerController>();
        if(!playerController)
        {
            throw new System.Exception("PLAYER CONTROLLER NOT FOUND!");
        }
        playerMovement = playerController.GetComponent<MovementManager>();
    }

    private void FixedUpdate()
    {
        if(!playerMovement.IsCharging)
        {
            if(killStreakCount > 1)
            {
                UiManager.ShowMultiKillMessage(killStreakCount);
            }
            killStreakCount = 0;
        }
    }

    public void LevelCompleted()
    {
        if (!IsPlaying || !CanEndGame)
        {
            return;
        }
        IsPlaying = false;
        playerController.enabled = false;
        UiManager.ShowLevelCompletedScreen();
    }

    public void GameOver(string reason = "")
    {
        if(!IsPlaying || !CanEndGame)
        {
            return;
        }
        IsPlaying = false;
        playerController.enabled = false;
        UiManager.ShowGameOverScreen(reason);
    }

    public void OnSuitorKilled(bool isRich)
    {
        killStreakCount++;
        if (isRich)
        {
            LevelManager.OnRichSuitorKilled();
        }
        else
        {
            LevelManager.OnPoorSuitorKilled();
        }
    }



    IEnumerator StartSpawnSuitors()
    {
        while(true)
        {
            yield return new WaitWhile(() => LevelManager.SuitorSpawnInterval <= 0);
            yield return new WaitForSeconds(LevelManager.SuitorSpawnInterval);
            var spawnPoint = SpawnPointList[(int)(Random.value * SpawnPointList.Count)];
            Instantiate(suitorPrefab);
            suitorPrefab.transform.position = spawnPoint.transform.position;
            LevelManager.OnSuitorSpawned();
        } 
    }

    IEnumerator StartSpawnGrass()
    {
        while(true)
        {
            yield return new WaitWhile(() => LevelManager.GrassSpawnInterval <= 0);
            yield return new WaitForSeconds(LevelManager.GrassSpawnInterval);
            SpawnGrass();
        }
    }

    void SpawnGrass()
    {
        var grassObject = Instantiate(grassPrefab);
        var collider = grassObject.GetComponent<CircleCollider2D>();
        for (int i = 0; i < 20; i++)
        {
            var possiblePosition = new Vector3(
                Random.Range(-GAME_HALF_WIDTH, +GAME_HALF_WIDTH),
                Random.Range(-GAME_HALF_HEIGHT, +GAME_HALF_HEIGHT),
                grassPrefab.transform.position.z
            );
            if(Physics2D.OverlapCircle(possiblePosition, collider.radius) == null)
            {
                LevelManager.OnGrassSpawned();
                grassObject.transform.position = possiblePosition;
                return;
            }
        }
        Destroy(grassObject);
        Debug.LogWarning("Couldn't find place to spawn grass");
    }
}
