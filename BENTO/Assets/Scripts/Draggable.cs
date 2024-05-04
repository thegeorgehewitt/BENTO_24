using System;
using System.Collections;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    // determines speed of movement for slotting
    [SerializeField] private float moveTime = 0.4f;

    // indicate if this is an ingredient (1), prepped food (2) or bento box(3)
    [SerializeField] private int itemType;
    // indicate what type of ingredient/prepped food this is, rice/steamed rice (1), 
    [SerializeField] private int itemSubtype;

    // save starting position
    [SerializeField] protected Transform spawnPoint;

    [SerializeField] private Sprite spriteToApply;

    protected virtual void Start()
    {
        UpdateVisual();
    }

    public virtual void CheckLocation()
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
                        // make child of slot object017uj
                        transform.SetParent(nextFreeSlot, true);

                        // move objects to the slot's position
                        StartMoveTo(nextFreeSlot);

                        // terminate function
                        return;
                    }
                }
            }
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

    // update visuals to match the item type and subtype
    public virtual void UpdateVisual()
    {
        // based on item type, change colour
        switch (itemType)
        {
            case 1:
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteToApply;
                break;
            case 2:
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = spriteToApply;
                break;
            case 3:
                return;
            default:
                return;
        }
        // change text to display subtype
        //transform.GetChild(0).GetComponent<TextMesh>().text = itemSubtype.ToString();
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
