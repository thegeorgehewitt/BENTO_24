using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabControl : MonoBehaviour
{
    bool tabOpen = true;

    public void ToggleTab()
    {
        if(tabOpen)
        {
            tabOpen = false;
            transform.Find("OpenPanel").position = new Vector3(transform.Find("OpenPanel").position.x - 500, transform.Find("OpenPanel").position.y, transform.Find("OpenPanel").position.z);
        }
        else
        { 
            tabOpen = true;
            transform.Find("OpenPanel").position = new Vector3(transform.Find("OpenPanel").position.x + 500, transform.Find("OpenPanel").position.y, transform.Find("OpenPanel").position.z);
        }


        transform.Find("PanelBackground").gameObject.SetActive(tabOpen);

    }
}
