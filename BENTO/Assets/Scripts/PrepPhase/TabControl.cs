using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabControl : MonoBehaviour
{
    bool tabOpen = false;
    [SerializeField] private RectTransform panelRect;
    private bool isMoving = false;

    // move UI off-screen or on-screen to open/close
    public void ToggleTab()
    {
        SoundManager.instance?.PlaySFX("RecipeBook");

        if (isMoving)
        {
            return;
        }

        isMoving = true;
        if(tabOpen)
        {
            tabOpen = false;
            panelRect.localPosition = new Vector3( panelRect.localPosition.x - panelRect.rect.width/2f, panelRect.localPosition.y, panelRect.localPosition.z);
        }
        else
        { 
            tabOpen = true;
            panelRect.localPosition = new Vector3(panelRect.localPosition.x + panelRect.rect.width / 2f, panelRect.localPosition.y, panelRect.localPosition.z);
        }
        isMoving = false;
    }
}