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
    [SerializeField] private float funds = 2;

    // hold info on payments
    private float lastPayment;
    private float lastTip;
    private float[] paymentInfo;

    // track player progression
    private int day = 1;

    // action for change in funds, to update UI etc.
    public event Action OnFundsChange;

    // prefab for spawning new draggables
    [SerializeField] private GameObject draggablePrefab;

    // track which ingredients the player can use
    List<int> availableIngredients = new List<int>{ 1, 2, 3 };

    // track which foods have been prepped this round
    List<int> availableFoods = new List<int>();

    // holding available recipes
    List<int> currentRecipes = new List<int>() { 1, 2, 3 };

    // hold num of slots available for prepped food
    int foodSlots = 3;

    [SerializeField] public Sprite[] ingredientSprites;
    [SerializeField] public Sprite[] foodSprites;

    // track rounds stats
    public Dictionary<DisplayType, float> scores = new Dictionary<DisplayType, float>()
    {
        { DisplayType.Revenue, 0f },
        { DisplayType.Tips, 0f },
        { DisplayType.Profit, 0f },
        { DisplayType.RunningCost, 5f }
    };

    // used to track if upgrades have been purchased
    private bool[] isPurchased = new bool[23];

    private void Awake()
    {
        for(int i = 0; i < isPurchased.Count(); i++)
        {
            isPurchased[i] = false;
        }

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

                // get the specific prepped food droppable
                Droppable preppedScript = FindObjectOfType<PositionControl>().gameObject.GetComponent<Droppable>();
                if (preppedScript != null)
                {
                    preppedScript.PrepSlots(foodSlots);
                }

                break;

            // if Open Level - set draggable type to prepped food (2) and set prepped food from previous round to be spawned, then clear this for the next time
            case "OpenLevel":
                draggableType = 2;
                toSpawn = availableFoods.ToArray();

                // reset values of round costs and income
                scores[DisplayType.Revenue] = 0f;
                scores[DisplayType.Tips] = 0f;
                scores[DisplayType.Cost] = 0f;

                break;

            // if Management Phase - get access to scene control script and call function to set up upgrade options
            // also, set draggable to 0 as non will be used and keep the toSpawn array empty as non should be spawned
            case "ManagementPhase":
                SC_ManagementPhase sceneControl = FindObjectOfType<SC_ManagementPhase>();
                if(sceneControl)
                {
                    sceneControl.Initialize(isPurchased);
                }
                draggableType = 0;

                // successfully cleared level - increase running cost, clear stored foods and increase day count
                scores[DisplayType.RunningCost] += 0.5f;
                availableFoods.Clear();
                day++;

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
        // only process if round not ended
        SC_OpenPhase sceneScript = FindObjectOfType<SC_OpenPhase>();
        if(sceneScript != null && !sceneScript.GetRoundEnd())
        {
            // track rounds costs etc. and update funds
            lastPayment = amount;
            lastTip = tip;
            funds += amount + tip;
            funds = Mathf.Round(funds * 100f) / 100f;
            if (OnFundsChange != null)
            {
                OnFundsChange();
            }

            scores[DisplayType.Revenue] += amount;
            scores[DisplayType.Revenue] = Mathf.Round(scores[DisplayType.Revenue] * 100f) / 100f;
            scores[DisplayType.Tips] += tip;
            scores[DisplayType.Tips] = Mathf.Round(scores[DisplayType.Tips] * 100f) / 100f;
            scores[DisplayType.Cost] += cost;
            scores[DisplayType.Cost] = Mathf.Round(scores[DisplayType.Cost] * 100f) / 100f;
        }

        return;
    }

    // return round totals
    public float[] GetSummary()
    {
        return new float[] { scores[DisplayType.Revenue], scores[DisplayType.Tips], scores[DisplayType.Cost], scores[DisplayType.RunningCost] };
    }

    // update funds amount and call update funds action for UI update
    public void ChangeFunds(float amount)
    {
        funds += amount;
        funds = Mathf.Round(funds * 100f) / 100f;
        OnFundsChange();
    }

    public int GetDay()
    {
        return day;
    }

    // take the upgrade selected and call the corresponding function to implement the upgradeval
    public void ProcessUpgrade(int upgradeIndex)
    {
        isPurchased[upgradeIndex] = true;

        switch (upgradeIndex)
        {
            case 0:
                AddToRecipes(4);
                break;
            case 1:
                AddToRecipes(5);
                break;
            case 2:
                AddToIngredients(4);
                break;
            case 3:
                AddToRecipes(6);
                break;
            case 4:
                foodSlots++;
                break;
            case 5:
                AddToRecipes(7);
                break;
            case 6:
                AddToRecipes(8);
                break;
            case 7:
                AddToIngredients(5);
                break;
            case 8:
                AddToRecipes(9);
                break;
            case 9:
                AddToRecipes(10);
                break;
            case 10:
                AddToRecipes(11);
                break;
            case 11:
                AddToIngredients(6);
                break;
            case 12:
                foodSlots++;
                break;
            case 13:
                AddToRecipes(12);
                break;
            case 14:
                AddToRecipes(13);
                break;
            case 15:
                AddToRecipes(14);
                break;
            case 16:
                AddToIngredients(7);
                break;
            case 17:
                foodSlots++;
                break;
            case 18:
                AddToRecipes(15);
                break;
            case 19:
                AddToRecipes(16);
                break;
            case 20:
                AddToRecipes(17);
                break;
            case 21:
                AddToRecipes(18);
                break;
            case 22:
                AddToRecipes(19);
                break;
            default:
                break;
        }
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
}