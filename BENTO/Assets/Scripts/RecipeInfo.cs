using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// store recipes and names
public class RecipeInfo : MonoBehaviour
{
    public int[,] recipes = {
        { 0, 0, 0 },
        { 1, 0, 0 },
        { 2, 0, 0 },
        { 3, 0, 0 },
        { 4, 0, 0 },
        { 5, 0, 0 },
        { 1, 2, 0 }
        };


    public string[] ingredientNames = { "None", "Rice", "Veg", "Tofu", "Seaweed", "Nuts" };
    public string[] recipeNames = { "None", "Steamed Rice", "Stir Fried Veg", "Crispy Tofu", "Seaweed Snack", "Roasted Peanuts", "Fried Rice" };
}
