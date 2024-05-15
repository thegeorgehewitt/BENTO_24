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

    // reference to max scene duration
    [SerializeField] private float duration;

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

        // if duration reached
        if (timeElapsed > duration)
        {
            // load next scene
            SceneManager.LoadScene("OpenLevel");
        }
    }

    // main menu button function
    public void ReturnToMenu()
    {
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
}
