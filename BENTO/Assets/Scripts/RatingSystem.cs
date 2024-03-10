using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RatingSystem : MonoBehaviour
{
    public List<int>[] recipeInfo = {
        new List<int>{ 0 },
        new List<int>{ 1 },
        new List<int>{ 3 },
        new List<int>{ 4 },
        new List<int>{ 2, 6, 7 },
        new List<int>{ 2, 4, 6 },
        new List<int>{ 1, 2, 3, 6, 7 }
    };

    private Droppable droppable;

    private int rating = 0;

    // Start is called before the first frame update
    void Start()
    {
        // ref to droppable script on this object
        droppable = GetComponent<Droppable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RateBento(List<int> requirements)
    {
        if (droppable)
        {
            int[] bentoContents = droppable.GetContents();

            for (int req = 0; req < requirements.Count; req++)
            {            
                for (int slot = 0; slot < bentoContents.Length; slot++)
                {
                    //eek
                }
                if (requirements.Contains(bentoContents[req]))
                {
                    rating++;
                }
            }

            
        }
    }
}
