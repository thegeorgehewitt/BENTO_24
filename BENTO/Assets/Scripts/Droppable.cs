using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droppable : MonoBehaviour
{
    // indicate if this is for an ingredient (1), prepped food (2) or bento box(3)
    [SerializeField] private int slotType;
    [SerializeField] private CookingSystem cookingSystem;

    // function to return the transform of the next available slot, if there is one
    public Transform GetFreeSlot(int itemSubtype)
    {
        // repeat for each child of the object (slots)
        for (int i = 0; i < transform.childCount; i++)
        {
            // attempt to save ref of the slot script attackted to the child
            Slot slot = transform.GetChild(i).GetComponent<Slot>();

            // if script found
            if (slot != null && slot.gameObject.activeSelf)
            {
                // check if free
                if(slot.IsFree())
                {
                    // set to false, indicating it is in use
                    slot.SetItem(itemSubtype);

                    // return the transform of the slot
                    return transform.GetChild(i);
                }
            }
        }

        return null;
    }
    
    public bool CompareType(int itemType)
    {
        // return true if item and slot type match
        return itemType == slotType;
    }

    public int[] GetContents()
    {
        int[] contents = new int[transform.childCount];

        // repeat for each child of the object (slots)
        for (int i = 0; i < transform.childCount; i++)
        {
            // attempt to save ref to slot script attackted to the child
            Slot slot = transform.GetChild(i).GetComponent<Slot>();

            // if script found
            if (slot != null)
            {
                // record item at index
                contents[i] = slot.GetItem();
            }
        }

        return contents;
    }

    public Transform[] GetSlots()
    {
        Transform[] slots = new Transform[transform.childCount];

        // repeat for each child of the object (slots)
        for (int i = 0; i < transform.childCount; i++)
        {
            // save reference to transforms of children
            slots[i] = transform.GetChild(i);
        }

        return slots;
    }

    public void PrepSlots(int numOfSlots)
    {
        // repeat for each child of the object (slots)
        for (int i = 0; i < transform.childCount && i < numOfSlots; i++)
        {
            // attempt to save ref of the slot script attackted to the child
            Slot slot = transform.GetChild(i).GetComponent<Slot>();

            // if script found
            if (slot != null)
            {
                slot.gameObject.SetActive(true);
            }
        }

        return;
    }
}
