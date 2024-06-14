using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public enum DisplayType
{
    Revenue,
    Tips,
    Cost,
    Profit,
    RunningCost,
    ButtonText
}

public class SC_OpenPhase : MonoBehaviour
{
    // reference to time since scene opened
    private float timeElapsed;
    // reference to max scene baseDuration
    [SerializeField] private float duration;

    //field to hold image in timer
    [SerializeField] private Image timerBar;

    // reference to UI text field for funds
    [SerializeField] private TextMeshProUGUI fundsText;
    // reference to UI text field for funds
    [SerializeField] private TextMeshProUGUI paymentText;

    // reference to round end screen
    [SerializeField] private GameObject roundEndScreen;
    [SerializeField] private GameObject bankruptIcon;

    // track which scene should be opened (continue/restart)
    [SerializeField] private string roundToLoad;

    // reference to day text
    [SerializeField] private TextMeshProUGUI dayText;

    private bool reloadingScene = false;

    // bool to check if round had ended
    private bool roundEnd = false;

    // to hold ref to UI coroutine
    Coroutine displayCoroutine = null;

    // reference to main manager script
    private MainManager mainManager;

    private void Awake()
    {
        mainManager = MainManager.Instance;
    }

    void Start()
    {
        // set time to standard
        Time.timeScale = 1.0f;

        // update day UI
        if(dayText)
        {
            if (mainManager)
            {
                dayText.text = ("Day " + mainManager.GetDay().ToString());
            }
        }

        // update UI
        UpdateFundsAndPaymentText();

        // deactivate round end screen
        if(roundEndScreen)
        {
            roundEndScreen.SetActive(false);
        }

        // deactivate payment messgae
        if(paymentText)
        {
            paymentText.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        // subcribe to funds change to keep funds UI up to date
        if (mainManager)
        {
            mainManager.OnFundsChange += UpdateFundsAndPaymentText;
        }
    }

    private void OnDisable()
    {
        if(mainManager)
        {
            mainManager.OnFundsChange -= UpdateFundsAndPaymentText;
        }
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

        // if baseDuration reached and round still active
        if (timeElapsed > duration && !roundEnd && mainManager)
        {
            // update funds amount with costs
            mainManager.ChangeFunds(-mainManager.GetSummary()[2] - mainManager.GetSummary()[3]);

            if(mainManager.GetFunds() < 0)
            {
                //bankruptsy
                roundToLoad = "OpenLevel";
            }
            else
            {
                roundToLoad = "ManagementPhase";
            }

            if (fundsText)
            {
                fundsText.transform.parent.GetChild(1).gameObject.SetActive(false);
            }

            // display round end screen with summary info
            if (roundEndScreen)
            {
                SoundManager.instance.PlaySFX("Payment");

                TextMeshProUGUI[] textDisplays = roundEndScreen.GetComponentsInChildren<TextMeshProUGUI>();
                foreach (var textDisplay in textDisplays)
                {
                    ChangeableText script = textDisplay.GetComponent<ChangeableText>();
                    if (script != null)
                    {
                        if (script.GetDisplayType() == DisplayType.Profit)
                        {
                            float profit = Mathf.Round((mainManager.GetSummary()[0] + mainManager.GetSummary()[1] - mainManager.GetSummary()[2] - mainManager.GetSummary()[3]) * 100) / 100;
                            textDisplay.text = profit.ToString("F2");
                        }
                        else if (script.GetDisplayType() == DisplayType.RunningCost)
                        {
                            textDisplay.text = mainManager.GetSummary()[3].ToString("F2");
                        }
                        else if (script.GetDisplayType() == DisplayType.ButtonText)
                        {
                            if (roundToLoad == "OpenLevel")
                            {
                                textDisplay.text = "Retry";
                                if (bankruptIcon != null)
                                {
                                    bankruptIcon.SetActive(true);
                                }
                            }
                            else
                            {
                                textDisplay.text = "Continue";
                                if (bankruptIcon != null)
                                {
                                    bankruptIcon.SetActive(false);
                                }
                            }
                        }    
                        else
                        {
                            textDisplay.text = mainManager.GetSummary()[(int)script.GetDisplayType()].ToString("F2");
                        }
                    }
                }
                // set round end marker
                roundEnd = true;

                // display the end screen
                roundEndScreen.SetActive(true);
            }
            else
            {
                LoadNextScene();
            }
        }
    }

    // functionality for 'return to menu' button
    public void ReturnToMenu()
    {
        SoundManager.instance?.PlaySFX("MenuInteract");

        SceneManager.LoadScene("MainMenu");
    }

    // function to update funds UI and display payment info
    private void UpdateFundsAndPaymentText()
    {
        // update funds text
        if (fundsText && !reloadingScene && mainManager)
        {
            fundsText.text = ("B " + mainManager.GetFunds().ToString("F2"));
        }

        // show payment info
        if (paymentText && mainManager)
        {
            paymentText.gameObject.SetActive(true);

            paymentText.text = (mainManager.GetPayment()[0].ToString("F2") + " + " + mainManager.GetPayment()[1].ToString("F2"));

            // if already displaying - cancel
            if (displayCoroutine != null)
            {
                StopCoroutine(displayCoroutine);
            }
            // start coroutine to remove payment text after 1.5s
            displayCoroutine = StartCoroutine(DisplayIncome());
        }
    }

    public bool GetRoundEnd()
    {
        return roundEnd;
    }

    // funciton to open next scene
    public void LoadNextScene()
    {
        SoundManager.instance?.PlaySFX("MenuInteract");

        if (roundToLoad == "OpenLevel")
        {
            reloadingScene = true;
            mainManager.ChangeFunds(-mainManager.GetSummary()[0] - mainManager.GetSummary()[1] + mainManager.GetSummary()[2] + mainManager.GetSummary()[3]);
        }
        SceneManager.LoadScene(roundToLoad);
        return;
    }

    // coroutine to remove payment text after 1.5s
    IEnumerator DisplayIncome()
    {
        yield return new WaitForSeconds(1.5f);

        paymentText.gameObject.SetActive(false);
    }
}
