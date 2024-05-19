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
    // reference to max scene duration
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
            paymentText.transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        mainManager = MainManager.Instance;

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

        // if duration reached and round still active
        if (timeElapsed > duration && !roundEnd)
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
                fundsText.transform.parent.localScale *= 1.5f;
                fundsText.fontSize += 10;
            }

            // display round end screen with summary info
            if (roundEndScreen)
            {
                TextMeshProUGUI[] textDisplays = roundEndScreen.GetComponentsInChildren<TextMeshProUGUI>();
                foreach (var textDisplay in textDisplays)
                {
                    ChangeableText script = textDisplay.GetComponent<ChangeableText>();
                    if (script != null)
                    {
                        if (script.GetDisplayType() == DisplayType.Profit)
                        {
                            float profit = mainManager.GetSummary()[0] + mainManager.GetSummary()[1] - mainManager.GetSummary()[2] - mainManager.GetSummary()[3];
                            textDisplay.text = profit.ToString();
                        }
                        else if (script.GetDisplayType() == DisplayType.RunningCost)
                        {
                            textDisplay.text = mainManager.GetSummary()[3].ToString();
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
                            textDisplay.text = mainManager.GetSummary()[(int)script.GetDisplayType()].ToString();
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
        SceneManager.LoadScene("MainMenu");
    }

    // function to update funds UI and display payment info
    private void UpdateFundsAndPaymentText()
    {
        // update funds text
        if (fundsText && !reloadingScene)
        {
            fundsText.text = ("B " + mainManager.GetFunds().ToString());
        }

        // show payment info
        if (paymentText)
        {
            paymentText.transform.parent.gameObject.SetActive(true);

            paymentText.text = (mainManager.GetPayment()[0].ToString() + " + " + mainManager.GetPayment()[1].ToString());

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

    // fucniton to open next scene
    public void LoadNextScene()
    {
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

        paymentText.transform.parent.gameObject.SetActive(false);
    }
}
