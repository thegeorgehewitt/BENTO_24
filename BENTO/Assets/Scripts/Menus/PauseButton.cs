using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    // to hold pause menu object
    [SerializeField] private GameObject pauseMenu;

    // button function - open/close menu and pause/play time
    public void Pressed()
    {
        SoundManager.instance?.PlaySFX("MenuInteract");

        if (Time.timeScale > 0.0f)
        {
            Time.timeScale = 0.0f;
            if(pauseMenu)
            {
                pauseMenu.SetActive(true);
            }
        }
        else
        {
            Time.timeScale = 1.0f;
            if (pauseMenu)
            {
                pauseMenu.SetActive(false);
            }
        }
    }
}
