using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    // saves intial position
    public Vector3 startPosition;
    // determines speed of movement for slotting
    [SerializeField] private float moveTime = 0.4f;

    // indicate if this is an ingredient (1), prepped food (2) or bento box(3)
    [SerializeField] private int itemType;
    // indicate what type of ingredient/prepped food this is, rice/steamed rice (1), 
    [SerializeField] private int itemSubtype;

    private void Start()
    {
        // save position at start
        startPosition = transform.position;
        UpdateVisual();
    }

    public void CheckLocation()
    {
        // cast for objects at item's location
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);

        // loop through every raycast hit
        foreach (RaycastHit2D hit in hits)
        {
            // attempt to save hit object's Slot script
            Droppable droppable = hit.transform.GetComponent<Droppable>();

            // if script found
            if (droppable != null)
            {
                if(droppable.CompareType(itemType))
                {
                    Transform nextFreeSlot = droppable.GetFreeSlot(itemSubtype);
                    // if free slot available
                    if (nextFreeSlot != null)
                    {
                        transform.SetParent(nextFreeSlot, true);

                        // move objects to the slot's position
                        StartMoveTo(nextFreeSlot.position);

                        // terminate function
                        return;
                    }
                }
            }
        }

        // move object back to it's start position
        StartMoveTo(startPosition);
    }

    public void CheckLocationUp()
    {
        Debug.Log("CheckLocationUp");
        // cast for objects at item's location
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);

        // loop through every raycast hit
        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log("hit object: " + hit.transform.name);
            if (hit.transform == transform.parent)
            {
                // attempt to save hit object's Slot script
                Slot slot = hit.transform.GetComponent<Slot>();

                // if script found
                if (slot)
                {
                    transform.parent = null;
                    // set marker to show slot is empty
                    slot.SetItem(0);
                }
            }
        }
    }

    IEnumerator MoveTo(Vector3 endPosition)
    {
        // used to monitor progress through lerp
        float progress = 0.0f;
        // used at starting position for lerp
        Vector3 atStart = transform.position;

        // continue until lerp complete
        while (progress <= 1.0f)
        {
            // update lerp process
            progress += Time.deltaTime / moveTime;

            // update object position
            transform.position = Vector3.Lerp(atStart, endPosition, Mathf.SmoothStep(0.0f, 1.0f, progress));

            yield return null;
        }

        // set position to end position (remove overshoot from lerp)
        transform.position = endPosition;

        yield return null;
    }

    public void SetTypes(int type, int subtype)
    {
        // set variable to represent the type of object (ingredient/food/bento) and subtype (specific item)
        itemType = type;
        itemSubtype = subtype;
    }

    // accessible function to intiate movement coroutine
    public void StartMoveTo(Vector3 endPosition)
    {
        StartCoroutine(MoveTo(endPosition));
    }

    // update visuals to match the item type and subtype
    public void UpdateVisual()
    {
        // based on item type, change colour
        switch (itemType)
        {
            case 1:
                GetComponent<SpriteRenderer>().color = new Color(0.4980392f, 0.4196079f, 0.682353f, 1);
                break;
            case 2:
                GetComponent<SpriteRenderer>().color = new Color(0.682353f, 0.5067859f, 0.4196078f, 1);
                break;
            case 3:
                return;
            default:
                return;
        }
        // change text to display subtype
        transform.GetChild(0).GetComponent<TextMesh>().text = itemSubtype.ToString();
    }

    public int GetItemType()
    {
        return itemType;
    }
}
