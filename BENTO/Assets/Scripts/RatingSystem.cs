using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RatingSystem : MonoBehaviour
{
    // list of nutrients/flavours in recipe (1 - grains, 2 - fruits, 3 - fibre, 4 - sweet, 5 - drink)
    private List<int>[] recipeInfo = {
        new List<int>{ 0 },
        new List<int>{ 0 }, //bread
        new List<int>{ 2, 3 }, //fried banana
        new List<int>{ 1, 3 }, //porridge
        new List<int>{ 1, 2 }, //banana bread
        new List<int>{ 1, 2, 3 }, //banana porridge
        new List<int>{ 2, 4 }, //blueberry bowl
        new List<int>{ 1, 2, 3 }, //blueberry porridge
        new List<int>{ 2, 4 }, //fruit salad
        new List<int>{ 2, 3, 5 }, //banana milk
        new List<int>{ 1, 2, 3 }, //fruit porridge
        new List<int>{ 2, 5 }, //blueberry milk
        new List<int>{ 1, 4 }, //pancakes
        new List<int>{ 2, 5 }, //smoothie
        new List<int>{ 1, 2, 4 }, //banana pancakes
        new List<int>{ 1, 5 }, //french toast
        new List<int>{ 1 }, //flatbread
        new List<int>{ 1, 2, 4 }, //fruit pancakes
        new List<int>{ 1, 2, 3, 4 }, //banana french toast
        new List<int>{ 1, 2, 4 } //blueberry french toast
    };

    // 2D array with food cost and price
    private float[][] foodCostPrice =
    {       
        new float[] { 0.6f, 1 }, //bread
        new float[] { 0.6f, 1 }, //fried banana
        new float[] { 0.6f, 1 }, //porridge
        new float[] { 1.0f, 1.5f }, //banana bread
        new float[] { 1.0f, 1.5f }, //banana porridge
        new float[] { 0.6f, 1 }, //blueberry bowl
        new float[] { 1.0f, 1.5f }, //blueberry porridge
        new float[] { 1.0f, 1.5f }, //fruit salad
        new float[] { 1.0f, 1.5f }, //banana milk
        new float[] { 1.4f, 2 }, //fruit porridge
        new float[] { 1.0f, 1.5f }, //blueberry milk
        new float[] { 1.0f, 1.5f }, //pancakes
        new float[] { 1.4f, 2 }, //smoothie
        new float[] { 1.4f, 2 }, //banana pancakes
        new float[] { 1.0f, 1.5f }, //french toast
        new float[] { 1.4f, 2 }, //flatbread
        new float[] { 1.4f, 2 }, //fruit pancakes
        new float[] { 1.4f, 2 }, //banana french toast
        new float[] { 1.4f, 2 } //blueberry french toast
    };

    [SerializeField] private List<int> requirements = new List<int>(); // 1-5 relating to food info

    private MainManager mainManager;

    [SerializeField] private UnityEvent<List<int>> updateUI = new UnityEvent<List<int>>();

    private int rating = 0;
    private float boxPrice = 0;
    private float boxCost = 0;


    // Start is called before the first frame update
    void Start()
    {
        GenerateRequirements();
        mainManager = MainManager.Instance;
    }

    public int RateBento(Droppable BENTODroppable)
    {
        // reset rating
        rating = 0;

        if (BENTODroppable)
        {
            // get refs to the foods types stored
            int[] foodHeld = BENTODroppable.GetContents();

            // if BENTO empty
            if(foodHeld.Sum() == 0)
            {
                return 0;
            }

            //for each requirement from custoemr, counts the number of occurances in the foods
            for (int reqIndex = 0; reqIndex < requirements.Count; reqIndex++)
            {            
                // repeat for each slot
                for (int slot = 0; slot < foodHeld.Length; slot++)
                {
                    // get food type for current slot
                    int foodItem = foodHeld[slot];

                    // only on first loop
                    if (reqIndex == 0)
                    {
                        // if a food item is present in the current slot
                        if (foodItem != 0)
                        {
                            // add price of food to cost/price
                            boxPrice += foodCostPrice[foodItem][1];
                            boxCost += foodCostPrice[foodItem][0];
                        }
                    }
                    
                    // if food contains current requirement check
                    if (recipeInfo[foodItem].Contains(requirements[reqIndex]))
                    {
                        // increase rating
                        rating++;
                    }
                }
            }

            // update main manager with results
            if(mainManager)
            {
                mainManager.ProcessBox(boxPrice, rating, boxCost);
            }
            boxPrice = 0;
            boxCost = 0;
        }

        // reset requirements
        GenerateRequirements();

        return rating;
    }

    // reset requirements
    private void GenerateRequirements()
    {
        // clear previous requirements
        requirements.Clear();

        // randomise new values for requirements
        int quantity = Random.Range(1, 4);
        for (int i = 0; i < quantity; i++)
        {
            int newValue = 0;
            while (!requirements.Contains(newValue))
            {
                newValue = Random.Range(1, 6);
                if (!requirements.Contains(newValue))
                {
                    requirements.Add(newValue);
                }
            }
        }

        // display new requirements
        updateUI?.Invoke(requirements);

    }
}

