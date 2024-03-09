using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    // describe what is held, 0 = empty
    [SerializeField] private int itemHeld = 0;

    public bool IsFree()
    {
        // return true if no item held
        return (itemHeld == 0);
    }

    public int GetItem()
    {
        // return held item num
        return itemHeld;
    }

    public void SetItem(int itemType)
    {
        // update variable with current held item/empty status
        itemHeld = itemType;
    }
}
