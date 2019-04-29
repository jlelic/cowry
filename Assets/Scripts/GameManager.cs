using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public bool CanEndGame = true;

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

    public void LevelCompleted(float delay = 0)
    {
        if (!IsPlaying || !CanEndGame)
        {
            return;
        }
        playerMovement.CanMove = false;
        IsPlaying = false;
        playerController.enabled = false;
        StartCoroutine(LevelCompletedCleanup(delay));
    }

    IEnumerator LevelCompletedCleanup(float delay)
    {
        yield return new WaitForSeconds(delay);
        UiManager.ShowLevelCompletedScreen();
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void GameOver(string reason = "", float delay = 0f)
    {
        if(!IsPlaying || !CanEndGame)
        {
            return;
        }
        IsPlaying = false;
        playerMovement.CanMove = false;
        playerController.enabled = false;
        StartCoroutine(GameOverCleanup(reason, delay));
    }

    IEnumerator GameOverCleanup(string reason, float delay)
    {
        yield return new WaitForSeconds(delay);
        UiManager.ShowGameOverScreen(reason);
        yield return new WaitForSeconds(5 + reason.Length/15f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    public void SetObjective(string objective)
    {
        UiManager.ShowObjective(objective);
    }

    public void ClearObjective()
    {
        UiManager.ClearObjective();
    }

    public void CameraEffect(Color color)
    {
        UiManager.CameraEffect(color);
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
                Random.Range(-LevelManager.LevelHalfWidth, +LevelManager.LevelHalfWidth),
                Random.Range(-LevelManager.LevelHalfHeight, +LevelManager.LevelHalfHeight),
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
