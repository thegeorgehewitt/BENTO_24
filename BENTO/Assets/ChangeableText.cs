using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeableText : MonoBehaviour
{
    [SerializeField] private int displayType = 0;

    public int GetDisplayType()
    {
        return displayType;
    }
}
