using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    public static LevelSwitcher Instance { get; private set; }

    public static int currentLevel = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
//        SceneManager.activeSceneChanged += SceneChanged;
        SceneManager.LoadScene(currentLevel);
    }

    // Start is called before the first frame update
    void SceneChanged(Scene prev, Scene current)
    {
        if(current.buildIndex != 0)
        {
            SceneManager.LoadScene(currentLevel);
        }
    }

    public void Next()
    {
        currentLevel++;
        SceneManager.LoadScene(currentLevel);

    }

    public void Retry()
    {
        SceneManager.LoadScene(currentLevel);
    }
}
