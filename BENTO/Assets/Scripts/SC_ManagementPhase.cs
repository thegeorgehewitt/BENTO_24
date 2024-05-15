using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_ManagementPhase : MonoBehaviour
{
    // hold ref to 
    [SerializeField] private GameObject[] upgradeSlots;

    // reference to main manager script
    private MainManager mainManager;

    // reference to UI text field for funds
    [SerializeField] private TextMeshProUGUI fundsText;

    // array of upgrade names to display
    private string[] upgradeNames =
    {
         "New Ingredient: 6",
         "New Ingredient: 7",
         "New Recipe: 6",
         "New Recipe: 7",
         "Upgrade 5",
         "Upgrade 6",
         "Upgrade 7"
    };

    // array of upgrade costs to display/charge
    private float[] upgradeCost =
    {
        120,
        100,
        100,
        1,
        1,
        1,
        1
    };

    // array to track which upgrades are available to purchase this round
    private int[] availableUpgrades;

    // set up list of upgrades
    protected virtual void Awake()
    {
        foreach (var upgradeSlot in upgradeSlots)
        {
            upgradeSlot.SetActive(false);
        }

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

    // function to set up updgrade UI
    public void Initialize(bool[] purchased)
    {
        bool[] placed = new bool[purchased.Length];

        //transfer parameter data into local variable
        for (int purchaseIndex = 0; purchaseIndex < purchased.Length; purchaseIndex++)
        {
            placed[purchaseIndex] = purchased[purchaseIndex];
        }

        availableUpgrades = new int[upgradeSlots.Length];

        // populate and activate default UI
        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            for (int j = 0; j < placed.Length; j++)
            {
                if (!placed[j])
                {
                    availableUpgrades[i] = j;
                    upgradeSlots[i].SetActive(true);
                    upgradeSlots[i].GetComponent<TextMeshProUGUI>().text = upgradeNames[j];
                    upgradeSlots[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = upgradeCost[j].ToString();
                    placed[j] = true;
                    j = placed.Length;
                }

            }
        }
    }

    // fucntion for button press
    public void OnPurchase(int buttonIndex)
    {
        if(mainManager==null)
        {
            Debug.Log("Main Man null");
        }
        if (mainManager?.GetFunds() < upgradeCost[availableUpgrades[buttonIndex]])
        {
            string previousText = upgradeSlots[buttonIndex].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;
            upgradeSlots[buttonIndex].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Not Enough Funds";
            StartCoroutine(ButtonTextReset(upgradeSlots[buttonIndex].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>(), previousText));
            return;
        }

        // update button visuals to show item purchased
        upgradeSlots[buttonIndex].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Purchased";
        upgradeSlots[buttonIndex].transform.GetChild(0).GetComponent<Image>().color = new Color(0.9450981f, 0.9137256f, 0.7607844f);

        // use main manager to change funds for upgrade charge and implement upgrade effect
        // then update funds UI
        if (mainManager != null)
        {
            mainManager.ChangeFunds(-upgradeCost[availableUpgrades[buttonIndex]]);
            mainManager.ProcessUpgrade(availableUpgrades[buttonIndex]);
            UpdateFundsText();
        }
    }

    // function for continue button - continue to next phase
    public void LoadNextScene()
    {
        SceneManager.LoadScene("PrepLevel");
        return;
    }

    // function to update UI for funds to a the new amount
    private void UpdateFundsText()
    {
        if (fundsText)
        {
            fundsText.text = ("B " + mainManager.GetFunds().ToString("F2"));
        }
    }

    IEnumerator ButtonTextReset(TextMeshProUGUI buttonText, string text)
    {
        Debug.Log("coroutine running");
        yield return new WaitForSeconds(1f);

        buttonText.text = text;
    }
}
