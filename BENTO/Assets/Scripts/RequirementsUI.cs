using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequirementsUI : MonoBehaviour
{
    [SerializeField] private Sprite[] requirementSprites;

    Image[] slots;

    private void Awake()
    {
        // get all the slots
        slots = GetComponentsInChildren<Image>();
    }

    // requirements UI updates to show new requirements
    public void UpdateRequirements(List<int> newValues)
    {
        for (int i = 0; i < slots?.Length; i++)
        {
            // disable all
            slots[i].enabled = false;

            // update value and activate used slots
            if (i < newValues.Count)
            {
                slots[i].sprite = requirementSprites[newValues[i]];
                slots[i].enabled = true;
            }
        }
    }
}
