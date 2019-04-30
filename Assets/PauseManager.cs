using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    private bool isPaused;
    private bool pausePressed;
    private float prevTimeScale;

    private void Awake()
    {
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if(isPaused)
        {
            if (Input.GetAxisRaw("Confirm") > 0.5f)
            {
                Application.Quit(0);
            }
        }

        if (Input.GetAxisRaw("Pause") > 0.5f)
        {
            if (!pausePressed)
            {
                isPaused = !isPaused;
                pausePanel.SetActive(isPaused);
                if(isPaused)
                {
                    prevTimeScale = Time.timeScale;
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = prevTimeScale;
                }
            }

            pausePressed = true;
        }
        else
        {
            pausePressed = false;
        }

    }
}
