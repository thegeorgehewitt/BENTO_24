using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using System;

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

    public event Action OnBoxProcessed;

    [SerializeField] private GameObject draggablePrefab;

    List<int> availableIngredients = new List<int>{ 1, 2, 3, 4, 5 };

    List<int> availableFoods = new List<int>();


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

    public void AddToFoods(int newFood)
    {
        availableFoods.Add(newFood);
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

            default:
                return;
        }

        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        spawnPoints = spawnPoints.OrderBy(go => go.transform.position.x).ToArray();
        spawnPoints = spawnPoints.OrderBy(go => -go.transform.position.y).ToArray();

        Debug.Log(spawnPoints.Length);

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
        OnBoxProcessed();
        roundIncome += amount;
        roundTips += tip;
        roundCost += cost;

        return;
    }

    public float[] GetSummary()
    {
        return new float[] {roundIncome, roundTips, -roundCost};
    }

    public void ChangeFunds(float amount)
    {
        funds += amount;
        OnBoxProcessed();
    }
}