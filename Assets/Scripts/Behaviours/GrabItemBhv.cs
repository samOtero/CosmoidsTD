using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Grab Item")]
public class GrabItemBhv : Behavior {

    public float grabRange;
    public ItemRuntimeCollection itemList;

    public override void Init(unit myUnit, BehaviorHolder myHolder)
    {
        //do nothing
    }

    public override bool Run(unit myUnit, BehaviorHolder myHolder)
    {
        //Can't grab if you are dead
        if (myUnit.myStatus == unitStatus.dead)
        {
            //If we are holding something, then let's drop it
            if (myUnit.heldItem != null)
            {
                //Deattach from unit
                myUnit.heldItem.transform.parent = null;
                myUnit.heldItem.holder = null;
                myUnit.heldItem = null;
            }
            return false;
        }


        //If already holding something don't check to grab something
        if (myUnit.heldItem != null)
            return false;

        //Get list of items
        foreach (var itm in itemList.Items)
        {
            //If item is held then don't try to grab it
            if (itm.holder != null)
                continue;

            //Check if item is close enough
            var distanceToTarget = Vector2.Distance(myUnit.transform.position, itm.gameObject.transform.position);
            if (distanceToTarget <= grabRange)
            {
                myUnit.heldItem = itm;
                itm.holder = myUnit;
                itm.transform.SetParent(myUnit.transform, true);

                //Send message that we turned around
                myUnit.myFunctionality.SendBhvMessage(msg.grabbedItem, myUnit);
                
                break;
            }

        }

        return false;
    }
}
