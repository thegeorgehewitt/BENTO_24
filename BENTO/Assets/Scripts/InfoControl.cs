using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoControl : MonoBehaviour
{
    bool infoVisible = false;

    public void ShowInfo()
    {
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
                transform.GetChild(i).gameObject.SetActive(true);
            }
            infoVisible = true;
        }
    }
}
