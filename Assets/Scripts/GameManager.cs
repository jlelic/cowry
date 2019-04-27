using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    const float GAME_HALF_WIDTH = 7;
    const float GAME_HALF_HEIGHT = 4;

    public static GameManager Instance { get; private set; }

    public HashSet<GameObject> GrassList { get; private set; } = new HashSet<GameObject>();
    public HashSet<GameObject> NavPointList { get; private set; } = new HashSet<GameObject>();
    public HashSet<GameObject> DoorEntranceList { get; private set; } = new HashSet<GameObject>();

    [SerializeField] private GameObject grassPrefab;
    [SerializeField] private float grassSpawnInterval = 10;


    Coroutine spawnGrassCoroutine;

    private void Awake()
    {
        GameManager.Instance = this;
        spawnGrassCoroutine = StartCoroutine(StartSpawnGrass());
    }

    IEnumerator StartSpawnGrass()
    {
        while(true)
        {
            SpawnGrass();
            yield return new WaitForSeconds(grassSpawnInterval);
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
                grassObject.transform.position = possiblePosition;
                return;
            }
        }
        Destroy(grassObject);
        Debug.LogWarning("Couldn't find place to spawn grass");
    }
}
