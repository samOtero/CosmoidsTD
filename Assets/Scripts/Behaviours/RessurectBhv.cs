using UnityEngine;

[CreateAssetMenu(menuName = "Behaviour/Ressurect")]
public class RessurectBhv : Behavior {

    public int BaseRezTime;

    public override void Init(unit myUnit, BehaviorHolder myHolder)
    {
        resetTimer(myHolder);
    }

    public override bool Run(unit myUnit, BehaviorHolder myHolder)
    {
        var stopBehaviours = false;

        if (myUnit.myStatus == unitStatus.dead)
        {
            stopBehaviours = true;
            myHolder.variables.step--;
            if (myHolder.variables.step <= 0)
            {
                //Reset timer for next time
                resetTimer(myHolder);

                //Reset life
                myUnit.life = myUnit.myProfile.totalLife;

                if (myUnit.lifeBarFunc != null)
                    myUnit.lifeBarFunc.updateLifeBar(myUnit);

                //Set appropriate status
                if (myUnit.marker != null)
                {
                    //If we have a marker then reset your positioning
                    myUnit.myFunctionality.setAnimation(unitAnim.walking, myUnit); //Set animation
                    myUnit.myStatus = unitStatus.movingToNewMarkerSpot;
                }
                else
                {
                    myUnit.myFunctionality.setAnimation(unitAnim.idle, myUnit); //Set animation
                    myUnit.myStatus = unitStatus.idle;
                }
            }
        }
        return stopBehaviours;
    }

    /// <summary>
    /// Reset the ressurection timer
    /// </summary>
    private void resetTimer(BehaviorHolder myHolder)
    {
        myHolder.variables.step = BaseRezTime;
    }
}
