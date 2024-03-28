using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    [SerializeField] private float funds = 1500;

    private float lastPayment;
    private float lastTip;
    private float[] paymentInfo;

    private float roundCost = 0;
    private float roundTips = 0;
    private float roundIncome = 0;

    public event Action OnFundsChange;

    [SerializeField] private GameObject draggablePrefab;

    List<int> availableIngredients = new List<int>{ 1, 2, 3, 4, 5 };

    List<int> availableFoods = new List<int>();

    // holding available recipes
    List<int> currentRecipes = new List<int>() { 1, 2, 3, 4, 5 };

    private bool[] isPurhased =
    {
        false,
        false,
        false,
        false,
        false,
        false,
        false
    };

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        roundCost = 0;
        roundTips = 0;
        roundIncome = 0;

        int draggableType;
        int[] toSpawn;

        switch (SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
                return;

            case "PrepLevel":
                draggableType = 1;
                toSpawn = availableIngredients.ToArray();
                break;

            case "OpenLevel":
                draggableType = 2;
                toSpawn = availableFoods.ToArray();
                availableFoods.Clear();
                break;

            case "ManagementPhase":
                SC_ManagementPhase sceneControl = FindObjectOfType<SC_ManagementPhase>();
                if(sceneControl)
                {
                    sceneControl.Initialize(isPurhased);
                }
                draggableType = 0;
                toSpawn = Array.Empty<int>();
                return;

            default:
                return;
        }

        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            spawnPoints = spawnPoints.OrderBy(go => go.transform.position.x).ToArray();
            spawnPoints = spawnPoints.OrderBy(go => -go.transform.position.y).ToArray();
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i < toSpawn.Length)
            {
                GameObject newObject = Instantiate(draggablePrefab, spawnPoints[i].transform.position, Quaternion.identity);
                newObject.GetComponent<Draggable>().SetTypes(draggableType, toSpawn[i]);
                newObject.GetComponent<Draggable>().SetSpawnPoint(spawnPoints[i].transform);
            }
        }
    }


    public void AddToFoods(int newFood)
    {
        if (!availableFoods.Contains(newFood))
        {
            availableFoods.Add(newFood);
        }
    }

    public void AddToIngredients(int newIngredient)
    {
        if (!availableIngredients.Contains(newIngredient))
        {
            availableIngredients.Add(newIngredient);
        }
    }


    public float GetFunds()
    {
        return funds;
    }

    public float[] GetPayment()
    {
        paymentInfo = new float[] { lastPayment , lastTip };
        return paymentInfo;
    }

    public void ProcessBox(float amount, float tip, float cost)
    {
        lastPayment = amount;
        lastTip = tip;
        funds += amount + tip;
        OnFundsChange();
        roundIncome += amount;
        roundTips += tip;
        roundCost += cost;

        return;
    }

    public float[] GetSummary()
    {
        return new float[] {roundIncome, roundTips, roundCost};
    }

    public void ChangeFunds(float amount)
    {
        funds += amount;
        OnFundsChange();
    }

    public List<int> GetCurrentRecipes()
    {
        return currentRecipes;
    }

    // used to add to available recipes
    public void AddToRecipes(int newRecipe)
    {
        if (!currentRecipes.Contains(newRecipe))
        {
            currentRecipes.Add(newRecipe);
        }
        return;
    }

    public void ProcessUpgrade(int upgradeIndex)
    {
        isPurhased[upgradeIndex] = true;

        switch (upgradeIndex)
        {
            case 0:
                AddToIngredients(6);
                break;
            case 1:
                AddToIngredients(7);
                break;
            case 2:
                AddToRecipes(6);
                break;
            case 3:
                AddToRecipes(7);
                break;
            default:
                break;
        }
    }
}