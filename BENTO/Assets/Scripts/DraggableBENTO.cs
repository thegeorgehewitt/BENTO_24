using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
                    ratingSystem.RateBento(BENTODroppable);

                    //Debug.Log("rating: " + ratingSystem.RateBento(BENTODroppable));
                }
            }
        }
    }

    // additional code to be run after a move to have been completed
    protected override void AfterMoveTo()
    {
        // if has parent - is placed on customer
        if ( transform.parent != null )
        {
            // turn off renderer
            GetComponent<Renderer>().enabled = false;

            // get draggable scripts on the food items held
            Draggable[] draggables = GetComponentsInChildren<Draggable>();
            foreach (Draggable draggable in draggables)
            {
                //turn off renderer
                draggable.transform.GetComponent<Renderer>().enabled = false;
                //reset BENTO slot
                draggable.CheckLocationUp();
                // snap food to spawn position
                draggable.ResetPosition();
                // turn on renderer
                draggable.transform.GetComponent<Renderer>().enabled = true;
            }

            // save reference to slots on object
            Transform[] slots = transform.GetComponent<Droppable>().GetSlots();

            // reset customer slot
            CheckLocationUp();
            // snap BENTO back to spawn locatiom
            transform.position = spawnPoint.position;

            // turn on renderer
            GetComponent<Renderer>().enabled = true;
        }
        
    }

}
