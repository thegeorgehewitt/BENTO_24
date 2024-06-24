using System;
using System.Collections;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    // determines speed of movement for slotting
    [SerializeField] private float moveTime = 0.4f;

    // indicate if this is an ingredient (1), prepped food (2) or bento box(3)
    [SerializeField] private int itemType;
    // indicate what type of ingredient/prepped food this is 
    [SerializeField] private int itemSubtype;

    // save starting position
    [SerializeField] protected Transform spawnPoint;

    // sprite to be assigned to draggable
    [SerializeField] private Sprite spriteToApply;

    [SerializeField] private string errorMessage;

    // set appropriate visual and placement error message on spawn
    protected virtual void Start()
    {
        UpdateVisual();

        switch(itemType)
        {
            case 1:
                errorMessage = "Place onto the chopping board";
                break;
            case 2:
                errorMessage = "Place onto the counter";
                break;
            case 3:
                errorMessage = "Place onto the customer";
                break;
            default:
                errorMessage = string.Empty;
                break;
        }
    }


    // check if item can be places
    public virtual void CheckLocation()
    {
        bool wrongType = false;

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
                        // make child of slot object017uj
                        transform.SetParent(nextFreeSlot, true);

                        // move objects to the slot's position
                        StartMoveTo(nextFreeSlot);

                        // terminate function
                        return;
                    }
                }
                else
                {
                    wrongType = true;
                }
            }
        }

        // advice player of proper placement
        if (wrongType)
        {
            Vector3 location = new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.x);
            ErrorMessage.Instance.ShowText(errorMessage, Camera.main.WorldToScreenPoint(location));
        }

        // move object back to it's start position
        StartMoveTo(null);

    }

    public void CheckLocationUp()
    {
        // if has parent (is slotted)
        if (transform.parent)
        {
            //get slot script on parent
            Slot slot = transform.parent.GetComponent<Slot>();

            if (slot)
            {
                // set marker to show slot is empty
                slot.SetItem(0);

                // remove parentage
                transform.parent = null;
            }
        }
    }

    IEnumerator MoveTo(Transform endPosition)
    {
        // default end position to spawn point if a position hasn't been passed in
        if (endPosition == null)
        {
            endPosition = spawnPoint;
        }

        // to be used for pop-up UI
        if ((endPosition.position - transform.position).magnitude < 1f)
        {
            // activate UI
        }

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
            transform.position = Vector3.Lerp(atStart, endPosition.position, Mathf.SmoothStep(0.0f, 1.0f, progress));

            yield return null;
        }

        // set position to end position (remove overshoot from lerp)
        transform.position = endPosition.position;

        AfterMoveTo();

        yield return null;
    }

    public void SetTypes(int type, int subtype, Sprite sprite)
    {
        // set variable to represent the type of object (ingredient/food/bento) and subtype (specific item)
        itemType = type;
        itemSubtype = subtype;
        spriteToApply = sprite;
    }

    // accessible function to intiate movement coroutine
    public void StartMoveTo(Transform endPosition)
    {
        StartCoroutine(MoveTo(endPosition));
    }

    // update visuals to match the item type/subtype
    public virtual void UpdateVisual()
    {
        if (itemType == 1 || itemType == 2)
        {
            // set sprite for draggable object
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteToApply;
        }
    }

    // function to retrieve item type
    public int GetItemType()
    {
        return itemType;
    }

    // for adding additional functionality in inheriting classes
    protected virtual void AfterMoveTo()
    {

    }

    // funciton to snap back to start position
    public void ResetPosition()
    {
        transform.position = spawnPoint.position;
    }

    // update spawn point variable
    public void SetSpawnPoint(Transform spawn)
    {
        spawnPoint = spawn;
    }
}
