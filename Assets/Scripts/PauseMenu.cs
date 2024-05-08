using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] public GameObject pausePanel;
    Image brightnessOverlay;
    private bool paused;

    //[SerializeField] public Slider brightnessSlider;


    void Awake()
    {
        pausePanel.SetActive(false);
        paused = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Debug.Log("Escape Key Pressed");
            if(paused)
            {
                Resume();
                Debug.Log("Resume");
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        // We probably don't want to keep using timescale = 0, will bite us in the ass 
        // Use flags to freeze everything in the game, disable player input, and snapshot data
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        paused = true;

    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

}
