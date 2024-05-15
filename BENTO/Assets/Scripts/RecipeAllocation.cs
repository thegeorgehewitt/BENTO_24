using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeAllocation : MonoBehaviour
{
    [SerializeField] private Sprite[] recipeSprites;
    [SerializeField] private Sprite[] infoSprites;

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
                if (currentRecipes[i] <  recipeSprites.Length)
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
                        if (infoSprites[recipeInfo[currentRecipes[i]][j] - 1])
                        {
                            infoSlot.GetComponent<Image>().sprite = infoSprites[recipeInfo[currentRecipes[i]][j] - 1];
                        }
                        else
                        {
                            infoSlot.GetComponent<Image>().sprite = null;
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
