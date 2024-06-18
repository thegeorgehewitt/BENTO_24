using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SC_PrepPhase : MonoBehaviour
{
    // reference to time since scene opened
    private float timeElapsed;

    // reference to max scene baseDuration
    [SerializeField] private float baseDuration = 7;
    private float duration;

    //field to hold image in timer
    [SerializeField] private Image timerBar;

    // reference to UI text field for funds
    [SerializeField] private TextMeshProUGUI fundsText;

    // reference to day text
    [SerializeField] private TextMeshProUGUI dayText;

    // reference to main manager script
    private MainManager mainManager;

    private void Start()
    {
        // set time to standard
        Time.timeScale = 1.0f;

        // update day UI
        if (dayText)
        {
            dayText.text = ("Day " + mainManager.GetDay().ToString());
        }

        // update UI
        UpdateFundsText();
    }

    private void OnEnable()
    {
        mainManager = MainManager.Instance;

        // subcribe to funds change to keep funds UI up to date
        if (mainManager)
        {
            mainManager.OnFundsChange += UpdateFundsText;
        }
    }

    private void OnDisable()
    {
        mainManager.OnFundsChange -= UpdateFundsText;
    }

    void Update()
    {
        // update elapsed time
        timeElapsed += Time.deltaTime;

        // update timer UI
        if (timerBar)
        {
            timerBar.fillAmount = 1 - (timeElapsed / duration);
        }

        // if baseDuration reached
        if (timeElapsed > duration)
        {
            // load next scene
            if (mainManager.GetDay() != 1)
            {
                SceneManager.LoadScene("OpenLevel");
            }
            else
            {
                SceneManager.LoadScene("TutorialOpen");
            }
            
        }
    }

    // main menu button function
    public void ReturnToMenu()
    {
        SoundManager.instance?.PlaySFX("MenuInteract");

        SceneManager.LoadScene("MainMenu");
    }

    // function to update funds UI
    private void UpdateFundsText()
    {
        if (fundsText)
        {
            fundsText.text = ("B " + mainManager.GetFunds().ToString("F2"));
        }
    }

    public void SetDuration(float slotCount)
    {
        duration = baseDuration + (2 * slotCount);
    }
}
