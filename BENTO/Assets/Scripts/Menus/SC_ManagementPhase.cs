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
         "New Recipe: Banana Bread",
         "New Recipe: Banana Porridge",
         "New Ingredient: Blueberries",
         "New Recipe: Blueberry Bowl",
         "Additional Prepped Food Slot",
         "New Recipe: Blueberry Porridge",
         "New Recipe: Fruit Salad",
         "New Ingredient: Milk",
         "New Recipe: Banana Milk",
         "New Recipe: Fruit Porridge",
         "New Recipe: Blueberry Milk",
         "New Ingredient: Sugar",
         "Additional Prepped Food Slot",
         "New Recipe: Pancakes",
         "New Recipe: Smoothie",
         "New Recipe: Banana Pancakes",
         "New Ingredient: Butter",
         "Additional Prepped Food Slot",
         "New Recipe: French Toast",
         "New Recipe: Flatbread",
         "New Recipe: Blueberry Pancake",
         "New Recipe: Banana French Toast",
         "New Recipe: Blueberry Frencg Toast"
    };

    // array of upgrade costs to display/charge
    private float[] upgradeCost =
    {
        2,
        2,
        10,
        2,
        6,
        3,
        3,
        10,
        3,
        3,
        4,
        12,
        8,
        4,
        4,
        4,
        12,
        8,
        6,
        6,
        6,
        6,
        6

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
        if(mainManager != null)
        {
            mainManager.OnFundsChange -= UpdateFundsText;
        }
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

        UpdateFundsText();
    }

    // fucntion for button press
    public void OnPurchase(int buttonIndex)
    {
        if(mainManager==null)
        {
            Debug.Log("Main Menu null");
        }
        if (mainManager?.GetFunds() < upgradeCost[availableUpgrades[buttonIndex]])
        {
            SoundManager.instance?.PlaySFX("MenuInteract");

            string previousText = upgradeSlots[buttonIndex].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;
            upgradeSlots[buttonIndex].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Not Enough Funds";
            StartCoroutine(ButtonTextReset(upgradeSlots[buttonIndex].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>(), previousText));
            return;
        }

        // update button visuals to show item purchased
        upgradeSlots[buttonIndex].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Purchased";
        upgradeSlots[buttonIndex].transform.GetChild(0).GetComponent<Image>().color = new Color(0.8235295f, 0.6941177f, 0.6431373f);

        // use main manager to change funds for upgrade charge and implement upgrade effect
        // then update funds UI
        if (mainManager != null)
        {
            mainManager.ChangeFunds(-upgradeCost[availableUpgrades[buttonIndex]]);
            mainManager.ProcessUpgrade(availableUpgrades[buttonIndex]);
            UpdateFundsText();

            SoundManager.instance?.PlaySFX("Payment");
        }
    }

    // function for continue button - continue to next phase
    public void LoadNextScene()
    {
        SoundManager.instance?.PlaySFX("MenuInteract");

        SceneManager.LoadScene("PrepLevel");
        return;
    }

    // function to update UI for funds to a the new amount
    private void UpdateFundsText()
    {
        if (fundsText && mainManager != null)
        {
            fundsText.text = ("B " + mainManager.GetFunds().ToString("F2"));
        }
    }

    IEnumerator ButtonTextReset(TextMeshProUGUI buttonText, string text)
    {
        yield return new WaitForSeconds(1f);

        buttonText.text = text;
    }
}
