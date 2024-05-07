using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject pausePanel;
    [SerializeField] Slider brightnessSlider;
    Image brightnessOverlay;

    private bool paused;

    void Start()
    {
        pausePanel.SetActive(false);
        paused = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if(paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        // We probably don't want to use timescale = 0, will bite us in the ass 
        // Use flags to freeze everything in the game, disable player input, and snapshot data
        pausePanel.SetActive(true);
        
        paused = true;

        // Tint screen darkness 



    }

    public void Resume()
    {
        pausePanel.SetActive(false);

        paused = false;
    }

}
