using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Follow Path")]
public class FollowPathBhv : Behavior
{

    public PathRuntimeCollection pathList;
    public UnitGameEvent reachedExitEvent;
    public int StartPosition;

    public override void Init(unit myUnit, BehaviorHolder myHolder)
    {
        //If our Holder allows overrides then use ScriptObjects default
        if (myHolder.variables.overrideDefaults == false)
            myHolder.variables.step = StartPosition;
    }

    public override bool Run(unit myUnit, BehaviorHolder myHolder)
    {
        var targetPath = myHolder.variables.path;
        //If we don't have a target path then don't do anything
        if (targetPath == null || myUnit.myStatus == unitStatus.idle)
        {
            var currentPathPosition = myHolder.variables.step;
            setTarget(pathList.Items[currentPathPosition], myUnit, myHolder);
            return true; //We are getting our next path, so don't run other behaviours, usually this behaviour should be last
        }

        //If we aren't walking to path then don't do the walk
        if (myUnit.myStatus != unitStatus.walkingPath)
            return false; //We aren't walking at this point then we don't have a target

        var moveVelocity = myHolder.variables.velocity;
        var transform = myUnit.gameObject.transform;
        transform.Translate(moveVelocity * Time.deltaTime);
        var distanceToTarget = Vector2.Distance(transform.position, targetPath.transform.position);
        if (distanceToTarget < myUnit.currentMovementSpeed * Time.deltaTime)
            GotToPoint(targetPath, myUnit, myHolder);

        return true; //We are walking so don't do any other behaviours, usually this behaviour should be last
    }

    private void GotToPoint(pathSpot whichSpot, unit myUnit, BehaviorHolder holder)
    {
        var targetPath = holder.variables.path;

        if (whichSpot == targetPath)
        {
            var currentPathPosition = holder.variables.step;

            if (myUnit.turnedAround == false)
            {
                currentPathPosition++;
                //If we got to the last point in the path
                if (pathList.Items.Count == currentPathPosition)
                {
                    //For turning around
                    currentPathPosition -= 2;
                    myUnit.turnedAround = true;
                }
            }
            else
            {
                currentPathPosition--;
                if (currentPathPosition < 0)
                {
                    currentPathPosition = 1;
                    myUnit.turnedAround = false;

                    

                    //Send message that unit reached the end of the path
                    reachedExitEvent.Raise(myUnit);

                    //Destroy item if we are holding one
                    if (myUnit.heldItem)
                    {
                        Destroy(myUnit.heldItem.gameObject);
                        myUnit.heldItem = null;
                    }
                }
            }

            //Save our variable
            holder.variables.step = currentPathPosition;

            setTarget(pathList.Items[currentPathPosition], myUnit, holder);
        }
    }

    /// <summary>
    /// Set target velocity for unit to follow
    /// </summary>
    /// <param name="newPosition"></param>
    private void setTarget(pathSpot newPath, unit myUnit, BehaviorHolder myHolder)
    {
        //Get new velocity from target
        var heading = newPath.transform.position - myUnit.gameObject.transform.position;
        var direction = heading / heading.magnitude;
        var moveVelocity = direction * myUnit.currentMovementSpeed;

        //Update unit's facing direction
        myUnit.myFunctionality.changeSpriteDirectionBasedOnVelocity(moveVelocity, myUnit);

        //Set unit status as walking
        myUnit.myStatus = unitStatus.walkingPath;
        myUnit.myFunctionality.setAnimation(unitAnim.walking, myUnit); //Set animation

        //Set variables
        myHolder.variables.velocity = moveVelocity;
        myHolder.variables.path = newPath;
    }

    /// <summary>
    /// Handle a particular message that is sent to this behavior
    /// </summary>
    /// <param name="whichMsg"></param>
    /// <param name="whichUnit"></param>
    public override void GetMessage(msg whichMsg, unit whichUnit, BehaviorHolder holder)
    {
        if (whichMsg == msg.grabbedItem)
        {
            //Try to turn around
            if (whichUnit.turnedAround == false)
            {
                whichUnit.turnedAround = true;
                holder.variables.step--;
                setTarget(pathList.Items[holder.variables.step], whichUnit, holder);

            }
        }
    }

}
