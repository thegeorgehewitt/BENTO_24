using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoControl : MonoBehaviour
{
    // track if visible and if button has been pressed
    private bool infoVisible = false;
    private bool buttonPressed = false;

    private void Awake()
    {
        // subscribe to event for player touch input
        TouchInput.OnTouch += HideInfo;
    }

    private void OnDestroy()
    {
        // unsubscribe to event for player touch input
        TouchInput.OnTouch -= HideInfo;
    }

    // switch between displaying pup-up UI and not
    public void ToggleInfo()
    {
        buttonPressed = true;

        if (infoVisible)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            infoVisible = false;
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).GetComponent<Image>().sprite != null)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            infoVisible = true;
        }
    }

    // turn off pop-up
    private void HideInfo()
    {
        if(!buttonPressed)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            infoVisible = false;
        }
        else
        {
            buttonPressed = false;
        }
    }    
}
