using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableBENTO : Draggable
{
    protected override void Start()
    {
        // save position at start
        startPosition = transform.position;
    }

    // override function as not needed and could cause errors
    public override void UpdateVisual() { }


}
