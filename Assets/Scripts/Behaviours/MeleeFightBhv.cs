using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Hero Melee Fight")]
public class MeleeFightBhv : Behavior {

    public override void Init(unit myUnit, BehaviorHolder myHolder)
    {
        //Do nothing
    }

    public override bool Run(unit myUnit, BehaviorHolder myHolder)
    {
        var stopBehaviours = false;

        switch (myUnit.myStatus)
        {
            case unitStatus.startFighting:

                stopBehaviours = StartFight(myUnit);
                break;
            case unitStatus.fighting:
                stopBehaviours = Fight(myUnit);
                break;
        }
        return stopBehaviours;
    }

    /// <summary>
    /// Do this right before starting a fight
    /// </summary>
    /// <returns></returns>
    private bool StartFight(unit myUnit)
    {
        if (myUnit.selectedAbility != null)
            myUnit.selectedAbility.TriggerGlobalCooldown(); //Trigger a global cooldown on the current ability before starting to fight to not have attacks go off instantly

        myUnit.myStatus = unitStatus.fighting;
        myUnit.myFunctionality.changeSpriteDirectionBasedOnTargetPosition(myUnit.target.transform.position, myUnit); //face our target

        return true;
    }

    /// <summary>
    /// Run this while in the Fighting status
    /// </summary>
    /// <returns></returns>
    private bool Fight(unit myUnit)
    {
        var target = myUnit.target;
        if (target == null)
        {
            FightDone(myUnit);
            return true;
        }

        //Check if the enemy died
        var enemyDied = target.myStatus == unitStatus.dead;
        var enemyDisengaged = target.myStatus == unitStatus.movingToNewMarkerSpot;

        if (enemyDied || enemyDisengaged)
        {
            //When fighting is done
            FightDone(myUnit);
        }

        return true;
    }

    /// <summary>
    /// When the target is no longer available
    /// </summary>
    private void FightDone(unit myUnit)
    {
        //Remove our target
        myUnit.target = null;

        if (myUnit.marker != null)
        {
            myUnit.myFunctionality.setAnimation(unitAnim.walking, myUnit); //Set animation
            myUnit.myStatus = unitStatus.headingBackToMarker;
        }
        else
        {
            myUnit.myFunctionality.setAnimation(unitAnim.idle, myUnit); //Set animation
            myUnit.myStatus = unitStatus.idle;
        }

    }
}
