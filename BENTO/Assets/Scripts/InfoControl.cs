using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoControl : MonoBehaviour
{
    private bool infoVisible = false;
    private bool buttonPressed = false;

    private void Awake()
    {
        TouchInput.OnTouch += HideInfo;
    }

    private void OnDestroy()
    {
        TouchInput.OnTouch -= HideInfo;
    }

    public void ToggleInfo()
    {
        Debug.Log("Yellooooo");
        buttonPressed = true;

        if (infoVisible)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Debug.Log("turning off " + transform.GetChild(i).gameObject);
                transform.GetChild(i).gameObject.SetActive(false);
            }
            infoVisible = false;
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Debug.Log("turning on" + transform.GetChild(i).gameObject);
                transform.GetChild(i).gameObject.SetActive(true);
            }
            infoVisible = true;
        }
    }

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
