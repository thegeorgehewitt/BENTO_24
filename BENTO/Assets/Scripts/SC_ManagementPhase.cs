using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_ManagementPhase : MonoBehaviour
{
    [SerializeField] private GameObject[] upgradeSlots;

    // reference to main manager script
    private MainManager mainManager;

    // reference to UI text field for funds
    [SerializeField] private TextMeshProUGUI fundsText;

    private string[] upgradeNames =
    {
         "New Ingredient: 6",
         "New Ingredient: 7",
         "New Recipe: 6",
         "Upgrade 4",
         "Upgrade 5",
         "Upgrade 6",
         "Upgrade 7"
    };

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

    private int[] availableUpgrades;

    protected virtual void Awake()
    {
        foreach (var upgradeSlot in upgradeSlots)
        {
            upgradeSlot.SetActive(false);
        }

        mainManager = FindObjectOfType<MainManager>();

        if (mainManager)
        {
            mainManager.OnFundsChange += UpdateFundsText;
        }

        UpdateFundsText();
    }

    protected virtual void Start()
    {
    }

    public void Initialize(bool[] purchased)
    {
        bool[] placed = new bool[purchased.Length];
        for (int purchaseIndex = 0; purchaseIndex < purchased.Length; purchaseIndex++)
        {
            placed[purchaseIndex] = purchased[purchaseIndex];
        }

        availableUpgrades = new int[upgradeSlots.Length];

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

    public void OnPurchase(int buttonIndex)
    {
        // update button visuals to show item purchased
        upgradeSlots[buttonIndex].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Purchased";
        upgradeSlots[buttonIndex].transform.GetChild(0).GetComponent<Image>().color = new Color(0.9450981f, 0.9137256f, 0.7607844f);


        if (mainManager != null)
        {
            mainManager.ChangeFunds(-upgradeCost[availableUpgrades[buttonIndex]]);
            mainManager.ProcessUpgrade(availableUpgrades[buttonIndex]);
            UpdateFundsText();
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("PrepLevel");
        return;
    }

    private void UpdateFundsText()
    {
        if (fundsText)
        {
            fundsText.text = ("B " + mainManager.GetFunds().ToString());
        }
    }
}
