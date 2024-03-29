using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequirementsUI : MonoBehaviour
{

    TextMeshProUGUI[] slots;
    RatingSystem ratingSystem;

    // Start is called before the first frame update
    void Start()
    {
        ratingSystem = GameObject.FindGameObjectWithTag("RatingSystem").GetComponent<RatingSystem>();
    }

    public void UpdateRequirements(List<int> newValues)
    {
        slots = GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < slots?.Length; i++)
        {
            slots[i].transform.parent.gameObject.GetComponent<Image>().enabled = false;
            if (i < newValues.Count)
            {
                slots[i].SetText(newValues[i].ToString());
                slots[i].transform.parent.gameObject.GetComponent<Image>().enabled = true;
            }
            else
            {
                slots[i].SetText("");
            }
        }
    }


}
