using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private float grassSpawnInterval = 10;
    [SerializeField] private float suitorsSpawnInterval = 1;

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

    virtual public void OnSuitorSpawned()
    {

    }

    virtual public void OnGrassSpawned()
    {

    }

    virtual public void OnSuitorKilled()
    {

    }

    virtual public void OnGrassEaten()
    {

    }

    virtual public void OnCowStunned()
    {

    }

    virtual public void OnPoorSuitorEntered()
    {
        Debug.Log("GAME OVER");
    }

    virtual public void OnRichSuitorEntered()
    {
        Debug.Log("LEVEL COMPLETED");
    }
}
