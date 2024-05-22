using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sets position of a world space item based on a UI guide and offset value
public class PositionControl : MonoBehaviour
{
    // UI gameobject to base position of this gameobject
    [SerializeField] private GameObject GuideUI;

    // determine if should be offset form position
    [SerializeField] private UIOffsetType offsetType;

    void Start()
    {
        // positioon depending on offset and UI guide position
        if (offsetType == UIOffsetType.HalfHori)
        { 
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(GuideUI.transform.position).x - transform.localScale.x/2, Camera.main.ScreenToWorldPoint(GuideUI.transform.position).y, 0);
        }
        if (offsetType == UIOffsetType.HalfVert)
        {
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(GuideUI.transform.position).x, Camera.main.ScreenToWorldPoint(GuideUI.transform.position).y - transform.localScale.y / 2, 0);
        }
        else if (offsetType == UIOffsetType.None)
        {
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(GuideUI.transform.position).x, Camera.main.ScreenToWorldPoint(GuideUI.transform.position).y, 0);
        }
    }
}

public enum UIOffsetType
{
    HalfHori,
    HalfVert,
    None
}
