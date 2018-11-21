using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Melee/Basic")]
public class MeleeAbility : Ability
{

    /// <summary>
    /// Do ability returns true if ability was completed, false if not, relies on myUnit's target for anything targetted, individual attacks may opt for their own targets
    /// </summary>
    public override bool Trigger(unit myUnit, AbilityHolder myHolder)
    {
        var target = myUnit.target;

        //If we are dead don't try to do this ability
        if (myUnit.myStatus == unitStatus.dead)
            return false;

        //If no target, or target is not an enemy, or my unit is not fighting then don't do attack
        if (target == null || myUnit.myProfile.isEnemy == target.myProfile.isEnemy || myUnit.myStatus != unitStatus.fighting)
            return false;

        //Do damage to target
        target.myFunctionality.damage(power, myUnit, target);

        return true;
    }
}
