using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Content;

public class CookingSystem : MonoBehaviour
{
    // 2D array with recipe number and ingredients
    private int[][] recipes =
    {
        new int[] { 0, 0, 0 },
        new int[] { 1, 0, 0 },
        new int[] { 2, 0, 0 },
        new int[] { 3, 0, 0 },
        new int[] { 4, 0, 0 },
        new int[] { 5, 0, 0 },
        new int[] { 1, 2, 0 }, 
        new int[] { 1, 3, 0 }
    };

    private Droppable droppable;

    // arrays to hold ingredient and recipe names
    private string[] ingredientNames = new string[] { "None", "Rice", "Veg", "Tofu", "Seaweed", "Nuts" };
    private string[] recipeNames = new string[] { "None", "Steamed Rice", "Stir Fried Veg", "Crispy Tofu", "Seaweed Snack", "Roasted Peanuts", "Fried Rice", "Tofu Rice" };

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
                                    spawnedItemScript.SetTypes(2, i);
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
