using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic unit functionality
/// </summary>
[CreateAssetMenu(menuName = "Functionality/Basic/Unit")]
public class unitFunc : ScriptableObject {


    public void Init(unit whichUnit)
    {
        var profile = whichUnit.myProfile;
        whichUnit.life = profile.totalLife;
        whichUnit.currentMovementSpeed = profile.baseMovementSpeed;

        profile.friendlyList.Add(whichUnit);

        //Set unit's animator
        whichUnit.animator = whichUnit.GetComponent<Animator>();

        var abilities = whichUnit.abilities;
        //Init our selected ability
        if (abilities != null && abilities.Count > 0)
            whichUnit.selectedAbility = abilities[whichUnit.selectedAbilityIndex];

        //Init our behaviours
        var behaviors = whichUnit.behaviors;
        if (behaviors != null)
            foreach (var bhv in behaviors)
                bhv.Init(whichUnit);
    }

    public void End(unit whichUnit)
    {
        whichUnit.myProfile.friendlyList.Remove(whichUnit);
    }

    public void Run(unit whichUnit)
    {
        var behaviors = whichUnit.behaviors;
        if (behaviors != null)
        {
            foreach (var bhv in behaviors)
            {
                var stopRunning = bhv.Run(whichUnit);
                if (stopRunning)
                    break;
            }
        }

        var abilities = whichUnit.abilities;
        if (abilities != null)
        {
            //Run cooldown for all abilities
            foreach (var abl in abilities)
                abl.RunCooldown();

            //Run trigger for selected ability
            whichUnit.selectedAbility.Run(whichUnit);
        }
    }

    #region Targetting Functions


    /// <summary>
    /// Get a target unit that is in range of another
    /// </summary>
    /// <param name="targets"></param>
    /// <param name="whichUnit"></param>
    /// <returns></returns>
    public unit GetTargetInRange(unit whichUnit, float range, List<unit> targets)
    {
        unit unitInRange = null;

        //By default our unit position will be where they stand
        Vector3 unitPos = whichUnit.transform.position;

        //If the unit has a marker then use the marker as the range position
        if (whichUnit.marker != null)
            unitPos = whichUnit.marker.unitPos.position;

        foreach (var current in targets)
        {
            //don't select dead targets
            if (current.myStatus == unitStatus.dead)
                continue;

            var currDistance = Vector2.Distance(current.gameObject.transform.position, unitPos);
            if (currDistance <= range)
            {
                unitInRange = current;
                break;
            }
        }

        return unitInRange;
    }

    /// <summary>
    /// Get a suitable enemy target for the unit
    /// </summary>
    /// <returns></returns>
    public unit GetEnemyTarget(unit whichUnit)
    {
        var startPos = GetAnchorLocation(whichUnit);

        return whichUnit.defaultTargetting.GetTarget(startPos, whichUnit.myProfile.taggingRange.Value);
    }

    /// <summary>
    /// Get the unit's anchor point for checking range
    /// </summary>
    /// <returns></returns>
    public Vector3 GetAnchorLocation(unit whichUnit)
    {
        var startPos = whichUnit.transform.position;
        if (whichUnit.marker != null)
            startPos = whichUnit.marker.unitPos.position;

        return startPos;
    }

    #endregion

    #region Animation Functions

    /// <summary>
    /// Set the animation for our unit
    /// </summary>
    /// <param name="newAnimation"></param>
    public void setAnimation(int newAnimation, unit whichUnit)
    {
        if (whichUnit.animator != null)
            whichUnit.animator.SetInteger("animationStatus", newAnimation);
    }

    /// <summary>
    /// Figure out which direction a unit should face based on current velocity
    /// </summary>
    /// <param name="velocity"></param>
    public void changeSpriteDirectionBasedOnVelocity(Vector3 velocity, unit whichUnit)
    {
        var faceLeft = false;
        if (velocity.x < 0)
            faceLeft = true;

        if (faceLeft == whichUnit.facingLeft || velocity.x == 0)
            return;

        whichUnit.facingLeft = faceLeft;

        //Change local scale for unit
        var gfxContainer = whichUnit.transform.Find("gfxContainer");
        var newScale = gfxContainer.transform.localScale;
        newScale.x = Mathf.Abs(newScale.x);

        if (whichUnit.facingLeft)
            newScale.x *= -1;

        gfxContainer.transform.localScale = newScale;

    }

    /// <summary>
    /// Figure out which direction a unit should face to face a target location
    /// </summary>
    /// <param name="target"></param>
    public void changeSpriteDirectionBasedOnTargetPosition(Vector3 target, unit whichUnit)
    {
        var velocityToTarget = GetVelocityToTarget(target, whichUnit);
        changeSpriteDirectionBasedOnVelocity(velocityToTarget, whichUnit);
    }

    #endregion

    #region Life Functions

    /// <summary>
    /// Give damage to a unit. Returns true if dead, false if still has life.
    /// </summary>
    /// <param name="howMuch"></param>
    /// <returns></returns>
    public bool damage(int howMuch, unit fromWho, unit whichUnit)
    {
        var dead = false;
        whichUnit.life -= howMuch;

        if (whichUnit.life <= 0)
        {
            whichUnit.life = 0;
            dead = true;
            setAnimation(unitAnim.death, whichUnit); //Set animation
            whichUnit.myStatus = unitStatus.dead;
        }

        if (whichUnit.lifeBarFunc != null)
            whichUnit.lifeBarFunc.updateLifeBar(whichUnit);

        return dead;
    }

    /// <summary>
    /// Called after the death animation finishes
    /// </summary>
    public void deathAnimationOver(unit whichUnit)
    {
        //Raise our death event, if we have one
        if (whichUnit.deathEvent != null)
            whichUnit.deathEvent.Raise(whichUnit);

        //If we are an enemy then remove from level, may do some unit pooling in the future
        if (whichUnit.myProfile.isEnemy)
            Destroy(whichUnit.gameObject);

    }

    #endregion

    #region Movement Functions

    /// <summary>
    /// Moves the unit to a target location, returns if it got there or not
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool GoToTarget(Vector3 target, unit whichUnit)
    {
        var gotToTarget = false;

        var velToTarget = GetVelocityToTarget(target, whichUnit);

        //Update sprite x scale depending on current direction
        changeSpriteDirectionBasedOnVelocity(velToTarget, whichUnit);

        whichUnit.transform.Translate(velToTarget * Time.deltaTime);

        var distanceToTarget = Vector2.Distance(whichUnit.transform.position, target);
        if (distanceToTarget < whichUnit.currentMovementSpeed * Time.deltaTime)
            gotToTarget = true;

        return gotToTarget;
    }

    /// <summary>
    /// Updates the direction of the target
    /// </summary>
    /// <param name="target"></param>
    private Vector3 GetVelocityToTarget(Vector3 target, unit whichUnit)
    {
        var heading = target - whichUnit.transform.position;
        var directionToTarget = heading / heading.magnitude;
        var velToTarget = directionToTarget * whichUnit.currentMovementSpeed;

        return velToTarget;
    }

    #endregion

    #region Behavior Message

    public void SendBhvMessage(msg whichMsg, unit whichUnit)
    {
        foreach(var bhvHolder in whichUnit.behaviors)
        {
            bhvHolder.GetMessage(whichMsg, whichUnit);
        }
    }

    #endregion
}
