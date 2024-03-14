using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RatingSystem : MonoBehaviour
{
    private List<int>[] recipeInfo = {
        new List<int>{ 0 },
        new List<int>{ 1 },
        new List<int>{ 3 },
        new List<int>{ 4 },
        new List<int>{ 2, 6, 7 },
        new List<int>{ 2, 4, 6 },
        new List<int>{ 1, 2, 3, 6, 7 }
    };

    [SerializeField] private List<int> requirements = new List<int>();

    //[SerializeField] private GameObject ReqUI;

    public UnityEvent<List<int>> updateUI = new UnityEvent<List<int>>();

    private int rating = 0;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRequirements();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int RateBento(Droppable BENTODroppable)
    {
        // reset rating
        rating = 0;

        if (BENTODroppable)
        {
            // get refs to the foods types stored
            int[] foodHeld = BENTODroppable.GetContents();

            //for each requirement from custoemr, counts the number of occurances in the foods
            for (int req = 0; req < requirements.Count; req++)
            {            
                // repeat for each slot
                for (int slot = 0; slot < foodHeld.Length; slot++)
                {
                    // get food type for current slot
                    int foodItem = foodHeld[slot];

                    // if food contains current requirement check
                    if (recipeInfo[foodItem].Contains(requirements[req]))
                    {
                        // increase rating
                        rating++;
                    }
                }
            }
        }

        // reset requirements
        GenerateRequirements();
        return rating;
    }

    // reset requirements
    private void GenerateRequirements()
    {
        requirements.Clear();

        int quantity = Random.Range(1, 4);
        for (int i = 0; i < quantity; i++)
        {
            int newValue = 0;
            while (!requirements.Contains(newValue))
            {
                newValue = Random.Range(1, 7);
                if (!requirements.Contains(newValue))
                {
                    requirements.Add(newValue);
                }
            }
        }

        updateUI.Invoke(requirements);
    }
}
