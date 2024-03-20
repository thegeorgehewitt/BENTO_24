using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_OpenPhase : MonoBehaviour
{
    // reference to time since scene opened
    private float timeElapsed;
    // reference to max scene duration
    [SerializeField] private float duration;

    //field to hold image in timer
    [SerializeField] private Image timerBar;

    // reference to UI text field for funds
    [SerializeField] private TextMeshProUGUI fundsText;

    // reference to main manager script
    private MainManager mainManager;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        mainManager = FindObjectOfType<MainManager>();

        if (mainManager)
        {
            mainManager.OnFundsChange += UpdateFundsText;
        }

        UpdateFundsText();
    }

    // Update is called once per frame
    void Update()
    {
        // update elapsed time
        timeElapsed += Time.deltaTime;

        if (timerBar)
        {
            timerBar.fillAmount = 1 - (timeElapsed / duration);
        }

        // if duration reached
        if (timeElapsed > duration)
        {
            // load next scene
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateFundsText()
    {
        if (fundsText)
        {
            fundsText.text = ("B " + mainManager.GetFunds().ToString());
        }
    }
}
