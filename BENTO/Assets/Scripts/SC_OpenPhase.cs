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

    [SerializeField] private GameObject roundEndScreen;

    private bool roundEnd = false;

    Coroutine displayCoroutine = null;

    // reference to main manager script
    private MainManager mainManager;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        mainManager = FindObjectOfType<MainManager>();
        if (mainManager)
        {
            mainManager.OnBoxProcessed += UpdateFundsAndPaymentText;
        }

        UpdateFundsAndPaymentText();

        if(roundEndScreen)
        {
            roundEndScreen.SetActive(false);
        }

        if(paymentText)
        {
            paymentText.transform.parent.gameObject.SetActive(false);
        }
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
        if (timeElapsed > duration && !roundEnd)
        {
            mainManager.ChangeFunds(-mainManager.GetSummary()[2]);

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
                roundEnd = true;

                roundEndScreen.SetActive(true);
            }
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateFundsAndPaymentText()
    {
        if (fundsText)
        {
            fundsText.text = ("B " + mainManager.GetFunds().ToString());
        }
        if (paymentText)
        {
            paymentText.transform.parent.gameObject.SetActive(true);

            paymentText.text = (mainManager.GetPayment()[0].ToString() + " + " + mainManager.GetPayment()[1].ToString());

            if (displayCoroutine != null)
            {
                StopCoroutine(displayCoroutine);
            }
            displayCoroutine = StartCoroutine(DisplayIncome());
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("ManagementPhase");
        return;
    }

    IEnumerator DisplayIncome()
    {
        yield return new WaitForSeconds(1.5f);

        paymentText.transform.parent.gameObject.SetActive(false);

    }
}
