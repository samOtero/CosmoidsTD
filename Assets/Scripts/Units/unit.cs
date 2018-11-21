using System.Collections.Generic;
using UnityEngine;

public class unit : MonoBehaviour {

    #region unit Properties

    /// <summary>
    /// Holds base data for this type of unit, can be shared with multiple units, should be read only
    /// </summary>
    public unitProfile myProfile;

    /// <summary>
    /// Hold basic functionality that units will use, keeping it in a scriptable object 
    /// </summary>
    public unitFunc myFunctionality;

    #endregion

    #region Behaviour Properties

    /// <summary>
    /// List of behaviours for a unit, should be sorted by priority
    /// </summary>
    public List<BehaviorHolder> behaviors;

    /// <summary>
    /// Current status of unit, used for behaviours and abilities
    /// </summary>
    public unitStatus myStatus;

    #endregion

    #region Life Point Properties

    /// <summary>
    /// Current life of unit
    /// </summary>
    public float life;

    /// <summary>
    /// Saved percent of current life and total life
    /// </summary>
    public float percLife = 1f;

    public GameObject lifeBarGfx;
    public Canvas lifeBarContainer;
    public unitLifeBarFunc lifeBarFunc;
    public UnitGameEvent deathEvent;

    #endregion

    #region Movement Properties
    /// <summary>
    /// Current Movement speed for this unit, used for some behaviours, can be modified by buffs/debuffs etc
    /// </summary>
    public float currentMovementSpeed;

    public bool turnedAround;
    #endregion

    #region Ability Properties

    public unit target;
    public AbilityHolder selectedAbility;
    public List<AbilityHolder> abilities;
    public int selectedAbilityIndex = 0;

    #endregion

    #region Marker properties

    /// <summary>
    /// Marker assigned to a Hero unit
    /// </summary>
    public unitMarker marker;

    #endregion

    #region Item properties

    public item heldItem;

    #endregion

    #region Animation properties

    public Animator animator;
    public int currentAnimation = 0;
    public bool facingLeft = false;

    #endregion

    #region Targetting Properties

    public UnitTargetting defaultTargetting;

    #endregion

    private void Start()
    {
        myFunctionality.Init(this);

        //Add our lifebar graphic
        if (lifeBarFunc != null)
            lifeBarFunc.Init(this);

    }

    private void OnDisable()
    {
        myFunctionality.End(this);
    }

    // Update is called once per frame
    void Update()
    {
        //Temporary move marker on mouse up if we are selected hero
        if (marker != null && Input.GetMouseButtonUp(0) && levelManager.current.selectedHero == this)
        {
            var newPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            newPosition.z = 0f;
            marker.transform.position = newPosition;

            //If we were heading to a target then release that target from us
            if (myStatus == unitStatus.headingToTarget && target != null && target.myStatus == unitStatus.taggedForMelee)
            {
                target.myFunctionality.setAnimation(unitAnim.idle, target); //target should stop walking animation
                target.myStatus = unitStatus.idle;
                target = null;
            }

            //Go to new marker position if we aren't dead
            if(myStatus != unitStatus.dead)
            {
                myStatus = unitStatus.movingToNewMarkerSpot;
                myFunctionality.setAnimation(unitAnim.walking, this); //Set animation
            }
            
        }

        myFunctionality.Run(this);
        
    }

    #region Animation Event Functions

    /// <summary>
    /// Called when unit's death animation is done playing
    /// </summary>
    public void deathAnimationOver()
    {
        myFunctionality.deathAnimationOver(this);
    }

    /// <summary>
    /// Called by unit animation event
    /// </summary>
    /// <param name="which"></param>
    public void setAnimation(int which)
    {
        myFunctionality.setAnimation(which, this);
    }

    #endregion

    #region Death Functions


    #endregion

}
