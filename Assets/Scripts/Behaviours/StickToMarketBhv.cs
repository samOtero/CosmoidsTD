using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Stick To Marker")]
public class StickToMarketBhv : Behavior {

    public override void Init(unit myUnit, BehaviorHolder myHolder)
    {
        //Set unit status to go close to marker as default
        myUnit.myStatus = unitStatus.movingToNewMarkerSpot;
        myUnit.myFunctionality.setAnimation(unitAnim.walking, myUnit); //Set animation
    }

    public override bool Run(unit myUnit, BehaviorHolder myHolder)
    {
        var stopBehaviours = false;

        //If moving to a new marker spot or just heading back to marker after a fight
        if (myUnit.myStatus == unitStatus.movingToNewMarkerSpot || myUnit.myStatus == unitStatus.headingBackToMarker)
        {
            GoToMarker(myUnit);

            //If moving to a new marker then ignore all other actions
            if (myUnit.myStatus == unitStatus.movingToNewMarkerSpot)
                stopBehaviours = true;
        }

        return stopBehaviours;
    }

    /// <summary>
    /// Go to the assigned position for this unit's marker
    /// </summary>
    private void GoToMarker(unit myUnit)
    {
        if (myUnit.marker.unitPos == null)
            return;

        var gotToMarker = myUnit.myFunctionality.GoToTarget(myUnit.marker.unitPos.position, myUnit);

        if (gotToMarker == true)
        {
            myUnit.myFunctionality.setAnimation(unitAnim.idle, myUnit); //Set animation
            myUnit.myStatus = unitStatus.idle;
        }
    }
}
