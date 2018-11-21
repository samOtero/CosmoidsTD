using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Statuses a unit can have to describe their current state
/// </summary>
public enum unitStatus
{
    idle,
    movingToNewMarkerSpot,
    headingToTarget,
    startFighting,
    fighting,
    rangeAbility,
    dead,
    headingBackToMarker,
    taggedForMelee,
    walkingPath,
}

/// <summary>
/// Animation values for Animator
/// </summary>
public class unitAnim
{
    public static int idle = 0;
    public static int walking = 1;
    public static int meleeFighting = 2;
    public static int death = 3;
    public static int casting = 4;
    public static int castingRelease = 5;
}
