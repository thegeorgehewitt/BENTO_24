using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sets position of a world space item based on a UI guide and offset value
public class PositionControl : MonoBehaviour
{
    [SerializeField] private GameObject GuideUI;

    [SerializeField] private UIOffsetType offsetType;

    void Start()
    {
        if (offsetType == UIOffsetType.Half)
        { 
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(GuideUI.transform.position).x - transform.localScale.x/2, Camera.main.ScreenToWorldPoint(GuideUI.transform.position).y, 0);
        }
        else if (offsetType == UIOffsetType.None)
        {
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(GuideUI.transform.position).x, Camera.main.ScreenToWorldPoint(GuideUI.transform.position).y, 0);
        }
    }
}

public enum UIOffsetType
{
    Half,
    None
}
