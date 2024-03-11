using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DraggableBENTO : Draggable
{
    protected override void Start() { }

    // override function as not needed and could cause errors
    public override void UpdateVisual() { }

    // override Check Location function to add BENTO rating
    public override void CheckLocation()
    {
        // call base function
        base.CheckLocation();

        // check if slotted in customer
        if (transform.parent != null )
        {
            // if required scripts found
            RatingSystem ratingSystem = transform.parent.parent.GetComponent<RatingSystem>();
            if( ratingSystem != null )
            {
                Droppable BENTODroppable = transform.GetComponent<Droppable>();
                if ( BENTODroppable != null )
                {
                    // run rating script
                    Debug.Log("rating: " + ratingSystem.RateBento(BENTODroppable));
                }
            }
        }
    }

    protected override void AfterMoveTo()
    {
        GetComponent<Renderer>().enabled = false;
        CheckLocationUp();
        transform.position = spawnPoint.position;
        GetComponent<Renderer>().enabled = true;

        // save reference to slots on object
        Transform[] slots = transform.GetComponent<Droppable>().GetSlots();

        foreach (Transform slot in slots)
        {
            // look for objects in the slot
            RaycastHit2D[] hits = Physics2D.RaycastAll(slot.transform.position, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                // attempt to get ref to draggable script on found object
                Draggable script = hit.transform.GetComponent<Draggable>();
                // if found
                if (script != null)
                {
                    // call function to reset slot the item is being removed from
                    script.CheckLocationUp();
                    // move object back to their starting position
                    script.StartMoveTo(null);
                }
            }
        }
    }

}
