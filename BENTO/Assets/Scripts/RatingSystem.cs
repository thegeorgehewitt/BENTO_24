using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RatingSystem : MonoBehaviour
{
    // list of nutrients/flavours in recipe (1 - carbs, 2 - protein, 3 - fats, 4 - fibre, 5 - salty/umami, 6 - sweet, 7 - sour, 8 - bitter/spicy)
    private List<int>[] recipeInfo = {
        new List<int>{ 0 }, 
        new List<int>{ 1 }, //steamed rice
        new List<int>{ 4 }, //salad
        new List<int>{ 2, 3 }, //tofu
        new List<int>{ 4, 5 }, //seaweed
        new List<int>{ 4, 5 }, //mushroom soup
        new List<int>{ 1 }, //bread
        new List<int>{ 1, 6 }, //lollipop
        new List<int>{ 1, 4, 5 }, //onigiri
        new List<int>{ 1, 4, 6 }, //mixed rice
        new List<int>{ 1, 4, 5, 6 }, //mushroom pasta
        new List<int>{ 2, 4, 5 }, //miso soup
        new List<int>{ 1, 4, 5 }, //mushroom rice
        new List<int>{ 1, 6 }, //sum sum
        new List<int>{ 1, 2, 3, 8 }, //chili and rice
        new List<int>{ 1, 6 }, //cookie
        new List<int>{ 1, 2, 4 }, //banh chay
        new List<int>{ 1, 4, 5, 8 }, //sushi
        new List<int>{ 1, 2, 4 }, //sandwich
        new List<int>{ 1, 2, 8 } //empanadas

    };

    // 2D array with food cost and price
    private float[][] foodCostPrice =
    {       
        new float[] { 0.6f, 1 }, //steamed rice
        new float[] { 0.6f, 1 }, //salad
        new float[] { 0.6f, 1 }, //tofu
        new float[] { 0.6f, 1 }, //seaweed
        new float[] { 0.6f, 1 }, //mushroom soup
        new float[] { 0.6f, 1 }, //bread
        new float[] { 0.6f, 1 }, //lollipop
        new float[] { 1.4f, 2 },  //onigiri
        new float[] { 1.4f, 2 }, //mixed rice
        new float[] { 1.4f, 2 }, //mushroom pasta
        new float[] { 1.4f, 2 }, //miso soup
        new float[] { 1.4f, 2 }, //mushroom rice
        new float[] { 1.4f, 2 }, //sum sum
        new float[] { 1.4f, 2 }, //chili and rice
        new float[] { 1.4f, 2 }, //cookie
        new float[] { 1.4f, 2 }, //banh chay
        new float[] { 1.4f, 2 }, //sushi
        new float[] { 1.4f, 2 }, //sandwich
        new float[] { 1.4f, 2 } //empanadas
    };

    [SerializeField] private List<int> requirements = new List<int>(); // 1-7 relating to food info

    private MainManager mainManager;

    [SerializeField] private UnityEvent<List<int>> updateUI = new UnityEvent<List<int>>();

    private int rating = 0;
    private float boxPrice = 0;
    private float boxCost = 0;


    // Start is called before the first frame update
    void Start()
    {
        GenerateRequirements();
        updateUI?.Invoke(requirements);
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

            //foreach(int foodHeldItem in foodHeld)
            //{
            //    Debug.Log("Food held " + foodHeldItem);
            //    for (int i = 0; i < recipeInfo[foodHeldItem].Count; i++)
            //    {
            //        Debug.Log("Food info " + recipeInfo[foodHeldItem][i]);
            //    }
            //}


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
                newValue = Random.Range(1, 8);
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
