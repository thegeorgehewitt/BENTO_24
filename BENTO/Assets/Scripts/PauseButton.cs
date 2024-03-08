using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public void Pressed()
    {

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
