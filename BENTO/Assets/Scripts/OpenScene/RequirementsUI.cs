using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequirementsUI : MonoBehaviour
{
    [SerializeField] private Sprite[] requirementSprites;

    List<Image> slots;

    private void Awake()
    {
        // get all the slots (take out the text bubble image)
        slots = GetComponentsInChildren<Image>().ToList();
        foreach (Image slot in slots)
        {
            if (slot.transform == transform)
            {
                slots.Remove(slot);
                break;
            }
        }
    }

    // requirements UI updates to show new requirements
    public void UpdateRequirements(List<int> newValues)
    {
        GetComponent<Image>().enabled = false;
        for (int i = 0; i < slots?.Count; i++)
        {
            // disable all
            slots[i].enabled = false;
        }
        StartCoroutine(WaitAndUpdate(newValues));
    }

    IEnumerator WaitAndUpdate(List<int> newValues)
    {
        yield return new WaitForSeconds(0.8f);

        GetComponent<Image>().enabled = true;

        for (int i = 0; i < slots?.Count; i++)
        {
            // update value and activate used slots
            if (i < newValues.Count)
            {
                slots[i].sprite = requirementSprites[newValues[i] - 1];
                slots[i].enabled = true;
            }
        }
    }
}
