using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

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

    // Start is called before the first frame update
    void Start()
    {
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

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i < toSpawn.Length)
            {
                GameObject newObject = Instantiate(draggablePrefab, spawnPoints[i].transform);
                newObject.GetComponent<Draggable>().SetTypes(draggableType, toSpawn[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToFoods(int newFood)
    {
        availableIngredients.Add(newFood);
    }
}