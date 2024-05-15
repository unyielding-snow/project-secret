using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public UnityEvent pauseEvent = new UnityEvent();
    [SerializeField] private GameObject pausePanel;
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
                pausePanel.SetActive(false);
            }
            else
            {
                Pause();
                pausePanel.SetActive(true);
            }
        }
    }

    public void Pause()
    {
        // We probably don't want to keep using timescale = 0, will bite us in the ass 
        // Use unity events to freeze everything in the game, disable player input, and snapshot data?
        Time.timeScale = 0f;
        paused = true;

    }

    public void Resume()
    {
        Time.timeScale = 1f;
        paused = false;
    }

}
