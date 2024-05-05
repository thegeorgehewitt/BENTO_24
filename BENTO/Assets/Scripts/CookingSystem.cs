using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEditor;
using UnityEngine;

public class CookingSystem : MonoBehaviour
{
    // 2D array with recipe number and ingredients
    private int[][] recipes =
    {
        new int[] { 0, 0, 0 },
        new int[] { 1, 0, 0 }, //steamed rice
        new int[] { 2, 0, 0 }, //salad
        new int[] { 3, 0, 0 }, //tofu
        new int[] { 4, 0, 0 }, //seaweed
        new int[] { 5, 0, 0 }, //mushroom soup
        new int[] { 6, 0, 0 }, //bread
        new int[] { 7, 0, 0 }, //lollipop
        new int[] { 1, 4, 0 }, //onigiri
        new int[] { 1, 2, 0 }, //mixed rice
        new int[] { 5, 6, 0 }, //mushroom pasta
        new int[] { 4, 3, 0 }, //miso soup
        new int[] { 1, 5, 0 }, //mushroom rice
        new int[] { 1, 7, 0 }, //sum sum
        new int[] { 1, 3, 8 }, //chili and rice
        new int[] { 5, 7, 0 }, //cookie
        new int[] { 1, 8, 0 }, //banh chay
        new int[] { 1, 2, 4 }, //sushi
        new int[] { 2, 3, 6 }, //sandwich
        new int[] { 3, 6, 8 } //empanadas
    };

    private Droppable droppable;

    // arrays to hold ingredient and recipe names
    private string[] ingredientNames = new string[] { "None", "Rice", "Veg", "Tofu", "Seaweed", "Mushrooms", "Flour", "Sugar", "Beans"  };
    private string[] recipeNames = new string[] { "None", "Steamed Rice", "Salad", "Crispy Tofu", "Seaweed Snack", "Mushroom Soup", "Bread", "Lollipop", "Onigiri", "Mixed Rice", "Mushroom Pasta", "Miso Soup", "Mushroom Rice", "Sum Sum", "Chili and Rice", "Cookie", "Banh Chay", "Sushi", "Sandwich", "Empanadas" };

    // reference to prepped food prefab, for instantiation
    [SerializeField] private GameObject foodPrefab;
    // reference to the object which will be used to store prepped foods
    [SerializeField] private GameObject preppedArea;
    //reference to game manager
    [SerializeField] private MainManager mainManager;

    private void Start()
    {
        mainManager = MainManager.Instance;

        // ref to droppable script on this object
        droppable = GetComponent<Droppable>();

        // sort recipes into numerical order (allows comparison)
        foreach (int[] recipe in recipes)
        {
            Array.Sort(recipe);
        }
    }

    public void CheckRecipies()
    {
        // if valid reference to this objects droppable script held
        if (droppable)
        {
            // fill out array variable with array of contents in slots
            int[] contents = droppable.GetContents();
            // sort into numerical order (allow comparison)
            Array.Sort(contents);

            // loop for number of recipes in list
            for (int i = 1; i < recipes.Length; i++)
            {
                // if there is a match between the recipe and the contents
                if (contents.SequenceEqual(recipes[i]))
                {
                    // checks if matching recipe is present in available recipes
                    if (mainManager.GetCurrentRecipes().Contains(i))
                    {
                        // checks if this recipe has already been created in this phase
                        if (!preppedArea.GetComponent<Droppable>().GetContents().Contains(i))
                        {
                            // attempts to get next free slot for food
                            Transform freeSlot = preppedArea.GetComponent<Droppable>().GetFreeSlot(i);
                            // if slot found
                            if (freeSlot != null)
                            {
                                if(mainManager)
                                {
                                    mainManager.AddToFoods(i);
                                }

                                // spawn new ingredient and save reference
                                GameObject spawnedItem = Instantiate(foodPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                                // attepmt to save ref to new object's draggable script
                                Draggable spawnedItemScript = spawnedItem.GetComponent<Draggable>();

                                // if successfull
                                if(spawnedItemScript != null)
                                {
                                    // set type and subtype variables in new object
                                    spawnedItemScript.SetTypes(2, i, mainManager.foodSprites[i-1]);
                                    // call function to update appearance of new object
                                    spawnedItemScript.UpdateVisual();
                                    // move new object to the aviable slot
                                    spawnedItemScript.StartMoveTo(freeSlot);
                                    // set spawn point to slot
                                    spawnedItemScript.SetSpawnPoint(freeSlot);
                                    // make child of slot
                                    spawnedItem.transform.parent = freeSlot.transform;
                                }

                                // get stored food draggable scripts
                                Draggable[] draggables = GetComponentsInChildren<Draggable>();
                                foreach (Draggable draggable in draggables)
                                {
                                    // reset owning slots and snap back to start
                                    draggable.CheckLocationUp();
                                    draggable.StartMoveTo(null);
                                }
                            }
                        }
                    }
                }
            }
        }
        return;
    }
}
