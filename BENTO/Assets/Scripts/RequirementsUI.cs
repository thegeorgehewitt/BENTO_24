using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequirementsUI : MonoBehaviour
{

    TextMeshProUGUI[] slots;

    //RatingSystem ratingSystem;

    // reference to main manager script
    private MainManager mainManager;

    // requirements UI updates to show new requirements
    public void UpdateRequirements(List<int> newValues)
    {
        slots = GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < slots?.Length; i++)
        {
            // disable all
            slots[i].transform.parent.gameObject.GetComponent<Image>().enabled = false;
            // update value and activate used slots
            if (i < newValues.Count)
            {
                slots[i].SetText(newValues[i].ToString());
                slots[i].transform.parent.gameObject.GetComponent<Image>().enabled = true;
            }
            else
            {
                // reset value of unused slots
                slots[i].SetText("");
            }
        }
    }

}
