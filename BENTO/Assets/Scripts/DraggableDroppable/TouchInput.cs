using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    // create Instance for the script
    public static TouchInput instance;

    // used to store the world position that alines with the players current touching position
    private Vector2 touchPosition;

    // used to track if dragging is happening
    private bool isDragging;
    // stores the current/most recently dragged object
    private Draggable lastDragged;

    // create event for new touch
    public static event Action OnTouch;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        // check if currently dragging, only one touch input is present and this touch has ended
        if (isDragging && Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            // stop the drag
            EndDrag();

            // end fixed update early
            return;
        }

        // if there are any touches occuring
        if (Input.touchCount > 0)
        {
            // update value of touchPosition with the current touch input position
            touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            // if there is currently an item being dragged
            if(isDragging)
            {
                // continue to drag the object
                Drag();
            }
            else
            {
                // cast for objects where the player is touching
                RaycastHit2D[] hits = Physics2D.RaycastAll(touchPosition, Vector2.zero);

                // check if anything is subscribed, then invoke
                OnTouch?.Invoke();
                
                foreach (RaycastHit2D hit in hits)
                {
                    // if an item is found
                    if (hit.collider != null)
                    {
                        // save this item's draggable script to the draggable variable
                        Draggable draggable = hit.collider.gameObject.GetComponent<Draggable>();
                        // if a script was found
                        if (draggable != null)
                        {
                            if (draggable.GetItemType() < 3)
                            {   
                                // update last dragged to this value
                                lastDragged = draggable;
                                // intiate the drag
                                StartDrag();
                                return;
                            }
                            else
                            {
                                // update last dragged to this value
                                lastDragged = draggable;
                                // intiate the drag
                                StartDrag();
                            }
                        }
                    }
                }
            }
            
        }
    }

    void StartDrag()
    {
        // update layer and isDragging bool
        UpdateDragStatus(true);

        // save ref to draggable script on dragged item
        Draggable draggable = lastDragged.GetComponent<Draggable>();

        if (draggable != null)
        {
            // call function to reset slot the item is being removed from
            draggable.CheckLocationUp();
        }
    }

    void Drag()
    {
        // move the dragged item to the new touch position
        lastDragged.transform.position = new Vector2(touchPosition.x, touchPosition.y);
    }

    void EndDrag()
    {
        // update layer and isDragging bool
        UpdateDragStatus(false);

        // save ref to draggable script on dragged item
        Draggable draggable = lastDragged.GetComponent<Draggable>();

        if (draggable != null)
        {
            // check if item can be dropped
            draggable.CheckLocation();
        }
    }

    public Draggable LastDragged()
    {
        return lastDragged;
    }

    void UpdateDragStatus(bool dragActive)
    {
        // update bool according to input
        isDragging = dragActive;

        // move dragged object onto the appropriate layer
        if (isDragging)
        {
            lastDragged.gameObject.layer = Layer.Dragging;
        }
        else
        {
            lastDragged.gameObject.layer = Layer.Default;
        }
    }
}
