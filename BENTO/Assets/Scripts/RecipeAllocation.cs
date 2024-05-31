using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeAllocation : MonoBehaviour
{
    [SerializeField] private Sprite[] recipeSprites;
    [SerializeField] private Sprite[] infoSprites;

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

    // prepare recipes tabs
    public void InitializeRecipes(List<int> currentRecipes)
    {
        // for each recipe slot
        for (int i = 0; i < transform.childCount; i++)
        {
            // get recipe slot and make inactive
            GameObject recipeButton = transform.GetChild(i).gameObject;
            recipeButton.SetActive(false);

            // for each recipe in current recipes
            if (i < currentRecipes.Count)
            {
                // set image if one available and make active
                if (currentRecipes[i] < recipeSprites.Length)
                {
                    transform.GetChild(i).GetComponentInChildren<Image>().sprite = recipeSprites[currentRecipes[i]-1];
                    recipeButton.SetActive(true);
                }
                else
                {
                    Debug.Log("no valid sprite");
                    recipeButton.SetActive(true);
                }

                // for each recipe info slot
                for (int j = 0; j < recipeButton.transform.childCount ; j++)
                {
                    // get recipe info slot and make inactive
                    GameObject infoSlot = recipeButton.transform.GetChild(j).gameObject;
                    infoSlot.SetActive(false);

                    // set image if one available and needed
                    if (j < recipeInfo[currentRecipes[i]].Count)
                    {
                        if (recipeInfo[currentRecipes[i]][j] != 0)
                        {
                            if (infoSprites[recipeInfo[currentRecipes[i]][j] - 1])
                            {
                                infoSlot.GetComponent<Image>().sprite = infoSprites[recipeInfo[currentRecipes[i]][j] - 1];
                            }
                            else
                            {
                                infoSlot.GetComponent<Image>().sprite = null;
                            }
                        }
                        
                    }
                    else
                    {
                        infoSlot.GetComponent<Image>().sprite = null;
                    }

                }
            }
        }
    }
}
