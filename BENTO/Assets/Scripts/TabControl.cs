using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabControl : MonoBehaviour
{
    bool tabOpen = true;
    private Transform panelTransform;
    private Vector3 startPosition;

    private void Start()
    {
        panelTransform = transform.Find("PanelBackground");
        startPosition = panelTransform.position;
    }

    // move UI off-screen or on-screen to open/close
    public void ToggleTab()
    {
        if(tabOpen)
        {
            tabOpen = false;
            panelTransform.position = new Vector3(0f, panelTransform.position.y, panelTransform.position.z);
        }
        else
        { 
            tabOpen = true;
            transform.Find("PanelBackground").position = startPosition;

        }
    }
}