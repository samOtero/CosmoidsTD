using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Range/Basic")]
public class RangeAbility : Ability {

    private const int AttackNotInitiated = 0;
    private const int AttackCasting = 1;
    private const int AttackRelease = 2;
    private const int AttackFinish = 3;
    public float range;
    public int castingTotalTime;

    private void Reset(AbilityHolder myHolder)
    {
        myHolder.phase = AttackNotInitiated;
    }

    public override bool Trigger(unit abilityOwner, AbilityHolder myHolder)
    {
        var target = abilityOwner.target;

        //If we are dead or moving to a new marker position don't try to do this ability and reset it
        if (abilityOwner.myStatus == unitStatus.dead || abilityOwner.myStatus == unitStatus.movingToNewMarkerSpot)
        {
            //Check if we died or interrupted the attack after it was released
            var abilityNeedsReset = false;
            if (myHolder.phase == AttackFinish)
                abilityNeedsReset = true;

            Reset(myHolder);
            return abilityNeedsReset;
        }


        //Nothing going on with attack
        if (myHolder.phase == AttackNotInitiated)
        {
            if (abilityOwner.myStatus == unitStatus.idle || abilityOwner.myStatus == unitStatus.headingBackToMarker || abilityOwner.myStatus == unitStatus.walkingPath)
            {
                //Get new target if we are idle and this attack is just starting
                var targetList = abilityOwner.myProfile.enemyList;

                target = abilityOwner.myFunctionality.GetTargetInRange(abilityOwner, range, targetList.Items);

                //Nothing in range so do nothing
                if (target == null)
                    return false;

                abilityOwner.target = target;
                myHolder.phase = AttackCasting;
                abilityOwner.myStatus = unitStatus.rangeAbility;
                myHolder.phaseCooldown = 0;
                return false; //Don't reset cooldown since attack is not completed yet 
            }
            else if (abilityOwner.myStatus == unitStatus.fighting)
            {
                //If we lost our target or it's dead
                if (target == null || target.myStatus == unitStatus.dead)
                {
                    abilityOwner.target = null;
                    myHolder.phase = AttackNotInitiated;
                    return false;
                }

                myHolder.phase = AttackCasting;
                //myUnit.myStatus = unitStatus.rangeAbility; //Want to stay in the fighting status
                myHolder.phaseCooldown = 0;
                return false; //Don't reset cooldown since attack is not completed yet 
            }
        }
        else if (myHolder.phase == AttackCasting) //while casting
        {
            //If we lost our target or it's dead
            if (target == null || target.myStatus == unitStatus.dead)
            {
                abilityOwner.target = null;
                myHolder.phase = AttackNotInitiated;
                abilityOwner.myStatus = unitStatus.idle;
                return false;
            }

            abilityOwner.myFunctionality.setAnimation(unitAnim.casting, abilityOwner); //Set animation to casting
            abilityOwner.myFunctionality.changeSpriteDirectionBasedOnTargetPosition(abilityOwner.target.transform.position, abilityOwner); //face our target
            myHolder.phaseCooldown++;
            if (myHolder.phaseCooldown >= castingTotalTime)
                myHolder.phase = AttackRelease;

            return false;
        }
        else if (myHolder.phase == AttackRelease)
        {
            //If we lost our target or it's dead
            if (target == null || target.myStatus == unitStatus.dead)
            {
                abilityOwner.target = null;
                myHolder.phase = AttackNotInitiated;
                abilityOwner.myStatus = unitStatus.idle;
                return false;
            }
            abilityOwner.myFunctionality.setAnimation(unitAnim.castingRelease, abilityOwner); //Set animation to casting release

            //Create and send projectile
            var projectile = Object.Instantiate(Resources.Load("gfx_projectile")) as GameObject;
            var deltaY = new Vector3(0f, 1f, 0f);
            var projectileSpeed = 7f;
            projectile.transform.localPosition = abilityOwner.transform.localPosition + deltaY;
            var projectileScript = projectile.AddComponent(typeof(proj_basic)) as proj_basic;
            projectileScript.init(abilityOwner, power, target.transform.localPosition + deltaY, projectileSpeed, target);

            myHolder.phase = AttackFinish;
            return false;
        }
        else if (myHolder.phase == AttackFinish)
        {
            myHolder.phase = AttackNotInitiated;

            //If not in melee combat then go back to idle
            if (abilityOwner.myStatus == unitStatus.rangeAbility)
                abilityOwner.myStatus = unitStatus.idle;
            return true;
        }

        return false;
    }
}
