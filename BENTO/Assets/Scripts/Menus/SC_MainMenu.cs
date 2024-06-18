using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;

    // reference to main manager script
    private MainManager mainManager;

    private void OnEnable()
    {
        mainManager = MainManager.Instance;
    }

    // function for play button
    public void PlayButton()
    {
        SoundManager.instance?.PlaySFX("MenuInteract");

        if(mainManager == null)
        {
            SceneManager.LoadScene("TutorialPrep");
        }
        else
        {
            SceneManager.LoadScene("PrepLevel");
        }
    }

    // function for options button
    public void ToggleOptions()
    {
        SoundManager.instance?.PlaySFX("MenuInteract");

        if (optionsMenu && optionsMenu.activeSelf)
        {
            optionsMenu.SetActive(false);
        }
        else if (optionsMenu)
        {
            optionsMenu.SetActive(true);
        }
    }
}
