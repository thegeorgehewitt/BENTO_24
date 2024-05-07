using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using System;

public class MainManager : MonoBehaviour
{
    // make singleton
    public static MainManager Instance;

    // hold player funds
    [SerializeField] private float funds = 1500;

    // hold info on payments
    private float lastPayment;
    private float lastTip;
    private float[] paymentInfo;

    // track round costs/income
    private float roundCost = 0;
    private float roundTips = 0;
    private float roundIncome = 0;

    // action for change in funds, to update UI etc.
    public event Action OnFundsChange;

    // prefab for spawning new draggables
    [SerializeField] private GameObject draggablePrefab;

    // track which ingredients the player can use
    List<int> availableIngredients = new List<int>{ 1, 2, 3, 4, 5 };

    // track which foods have been prepped this round
    List<int> availableFoods = new List<int>();

    // holding available recipes
    List<int> currentRecipes = new List<int>() { 1, 2, 3, 4, 5 };

    [SerializeField] public Sprite[] ingredientSprites;
    [SerializeField] public Sprite[] foodSprites;


    // used to track if upgrades have been purchased
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
        // prevent multiple instances from existing
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // subcribe to the event of a scene loading
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // when scene loaded - take actions based on the typ eof scene loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // reset values of round costs and income
        roundCost = 0;
        roundTips = 0;
        roundIncome = 0;

        int draggableType;
        int[] toSpawn;

        switch (SceneManager.GetActiveScene().name)
        {
            // if Main Menu - no actions needed
            case "MainMenu":
                return;

            // if Prep Level - set draggable type to ingredients (1) and set the ingredients list to be spawned
            case "PrepLevel":
                draggableType = 1;
                toSpawn = availableIngredients.ToArray();

                // get recipes UI script
                RecipeAllocation recipeAllocation = FindObjectOfType<RecipeAllocation>();
                if (recipeAllocation != null)
                {
                    // prepare visuals
                    recipeAllocation.InitializeRecipes(currentRecipes);
                }
                break;

            // if Open Level - set draggable type to prepped food (2) and set prepped food from previous round to be spawned, then clear this for the next time
            case "OpenLevel":
                draggableType = 2;
                toSpawn = availableFoods.ToArray();
                availableFoods.Clear();
                break;

            // if Management Phase - get access to scene control script and call function to set up upgrade options
            // also, set draggable to 0 as non will be used and keep the toSpawn array empty as non should be spawned
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

        // get spawn points in scene and order based on location (left to right and top to bottom)
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            spawnPoints = spawnPoints.OrderBy(go => go.transform.position.x).ToArray();
            spawnPoints = spawnPoints.OrderBy(go => -go.transform.position.y).ToArray();
        }

        // spawn selected items at ordered spawn points
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i < toSpawn.Length)
            {
                GameObject newObject = Instantiate(draggablePrefab, spawnPoints[i].transform.position, Quaternion.identity);
                if (draggableType == 1)
                {
                    if (i < ingredientSprites.Length)
                    {
                        newObject.GetComponent<Draggable>().SetTypes(draggableType, toSpawn[i], ingredientSprites[toSpawn[i]-1]);
                    }
                }
                else if (draggableType == 2)
                {
                    if (i < foodSprites.Length)
                    {
                        newObject.GetComponent<Draggable>().SetTypes(draggableType, toSpawn[i], foodSprites[toSpawn[i]-1]);
                    }
                }
                newObject.GetComponent<Draggable>().UpdateVisual();
                newObject.GetComponent<Draggable>().SetSpawnPoint(spawnPoints[i].transform);
            }
        }
    }

    // return list of currently avaialable recipes
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

    // add to available foods lists, following upgrade
    public void AddToFoods(int newFood)
    {
        if (!availableFoods.Contains(newFood))
        {
            availableFoods.Add(newFood);
        }
    }

    // add to available ingredients following upgrade
    public void AddToIngredients(int newIngredient)
    {
        if (!availableIngredients.Contains(newIngredient))
        {
            availableIngredients.Add(newIngredient);
        }
    }

    // return current funds
    public float GetFunds()
    {
        return funds;
    }

    // reutrn the last payment and tip amount
    public float[] GetPayment()
    {
        paymentInfo = new float[] { lastPayment , lastTip };
        return paymentInfo;
    }

    // gather info on bento box exhange, update funds and round totals
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

    // return round totals
    public float[] GetSummary()
    {
        return new float[] {roundIncome, roundTips, roundCost};
    }

    // update funds amount and call update funds action for UI update
    public void ChangeFunds(float amount)
    {
        funds += amount;
        OnFundsChange();
    }
    
    // take the upgrade selected and call the corresponding function to implement the upgradeval
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