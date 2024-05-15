using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeableText : MonoBehaviour
{
    // used to determine the type of information to be displayed here e.g. stat name, stat
    //[SerializeField] private int displayType = 0;
    [SerializeField] private DisplayType displayType;

    public DisplayType GetDisplayType()
    {
        return displayType;
    }
}