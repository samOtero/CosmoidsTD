using UnityEngine;

/// <summary>
/// Behaviour for Hero units to tag and melee approach an enemy in range
/// </summary>
[CreateAssetMenu(menuName = "Behaviour/Tag Enemies")]
public class EnemyTaggingBhv : Behavior {

    public override void Init(unit myUnit, BehaviorHolder myHolder)
    {
        //Do nothing
    }

    public override bool Run(unit myUnit, BehaviorHolder myHolder)
    {
        var stopBehaviours = false;

        switch (myUnit.myStatus)
        {
            case unitStatus.headingBackToMarker:
            case unitStatus.idle:
                stopBehaviours = CheckForTarget(myUnit);
                break;
            case unitStatus.headingToTarget:
                stopBehaviours = true;
                HeadToTarget(myUnit);
                break;
        }
        return stopBehaviours;
    }

    private bool CheckForTarget(unit myUnit)
    {

        var foundTarget = false;
        //Get target list based on unit default targetting
        myUnit.target = myUnit.myFunctionality.GetEnemyTarget(myUnit);

        //Head to target, if we have one
        if (myUnit.target != null)
        {
            foundTarget = true;
            myUnit.myFunctionality.setAnimation(unitAnim.walking, myUnit); //Set animation to walking
            myUnit.myStatus = unitStatus.headingToTarget;
            myUnit.target.myFunctionality.setAnimation(unitAnim.idle, myUnit.target); //target should stop walking animation
            myUnit.target.myStatus = unitStatus.taggedForMelee;
        }

        return foundTarget;
    }

    private void HeadToTarget(unit myUnit)
    {
        var target = myUnit.target;
        if (target == null)
        {
            myUnit.myStatus = unitStatus.headingBackToMarker;
            return;
        }
        var targetPosOffSet = new Vector3(-1f, 0f, 0f);
        if (myUnit.myFunctionality.GoToTarget(target.gameObject.transform.position + targetPosOffSet, myUnit))
        {
            myUnit.myFunctionality.setAnimation(unitAnim.meleeFighting, myUnit); //Set animation
            myUnit.myStatus = unitStatus.startFighting;

            //If unit is waiting to start fighting then set it's status as so
            if (target.myStatus == unitStatus.taggedForMelee)
            {
                target.target = myUnit; //Set this unit as the target for my target
                target.myStatus = unitStatus.startFighting;
                target.myFunctionality.setAnimation(unitAnim.meleeFighting, target); //Set animation
            }
        }
    }
}
