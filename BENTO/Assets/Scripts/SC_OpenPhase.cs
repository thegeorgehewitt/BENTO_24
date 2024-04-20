using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

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

    // reference to round end screen =
    [SerializeField] private GameObject roundEndScreen;

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

        // get ref to main manager and subcribe to funds change event
        mainManager = FindObjectOfType<MainManager>();
        if (mainManager)
        {
            mainManager.OnFundsChange += UpdateFundsAndPaymentText;
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
            mainManager.ChangeFunds(-mainManager.GetSummary()[2]);

            // display round end screen with summary info
            if (roundEndScreen)
            {
                TextMeshProUGUI[] textDisplays = roundEndScreen.GetComponentsInChildren<TextMeshProUGUI>();
                foreach (var textDisplay in textDisplays)
                {
                    ChangeableText script = textDisplay.GetComponent<ChangeableText>();
                    if (script != null)
                    {
                        if (script.GetDisplayType() == 3)
                        {
                            float profit = mainManager.GetSummary()[0] + mainManager.GetSummary()[1] - mainManager.GetSummary()[2];
                            textDisplay.text = profit.ToString();
                        }
                        else
                        {
                            textDisplay.text = mainManager.GetSummary()[script.GetDisplayType()].ToString();
                        }
                    }
                }
                // set round end marker
                roundEnd = true;

                // display the end screen
                roundEndScreen.SetActive(true);
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
        if (fundsText)
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


    // fucniton to open next scene
    public void LoadNextScene()
    {
        SceneManager.LoadScene("ManagementPhase");
        return;
    }

    // coroutine to remove payment text after 1.5s
    IEnumerator DisplayIncome()
    {
        yield return new WaitForSeconds(1.5f);

        paymentText.transform.parent.gameObject.SetActive(false);
    }
}
