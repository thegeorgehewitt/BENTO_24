using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabControl : MonoBehaviour
{
    bool tabOpen = true;

    // move UI off-screen or on-screen to open/close
    public void ToggleTab()
    {
        if(tabOpen)
        {
            tabOpen = false;
            transform.Find("OpenPanel").position = new Vector3(transform.Find("OpenPanel").position.x - 500, transform.Find("OpenPanel").position.y, transform.Find("OpenPanel").position.z);
            transform.Find("PanelBackground").position = new Vector3(transform.Find("PanelBackground").position.x - 500, transform.Find("PanelBackground").position.y, transform.Find("PanelBackground").position.z);
        }
        else
        { 
            tabOpen = true;
            transform.Find("OpenPanel").position = new Vector3(transform.Find("OpenPanel").position.x + 500, transform.Find("OpenPanel").position.y, transform.Find("OpenPanel").position.z);
            transform.Find("PanelBackground").position = new Vector3(transform.Find("PanelBackground").position.x + 500, transform.Find("PanelBackground").position.y, transform.Find("PanelBackground").position.z);

        }
    }
}