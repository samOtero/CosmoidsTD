using System;
using UnityEngine;

[Serializable]
public class BehaviorHolder {

    public Behavior behaviour;
    public VariableHolder variables; //used to store variables for specific behaviours

    /// <summary>
    /// Initialize our behaviour
    /// </summary>
    public void Init(unit myUnit)
    {
        behaviour.Init(myUnit, this);
    }

    public bool Run(unit myUnit)
    {
        return behaviour.Run(myUnit, this);
    }

    /// <summary>
    /// Handle a particular message that is sent to this behavior
    /// </summary>
    /// <param name="whichMsg"></param>
    /// <param name="whichUnit"></param>
    public void GetMessage(msg whichMsg, unit whichUnit)
    {
        behaviour.GetMessage(whichMsg, whichUnit, this);
    }
}

[Serializable]
public class VariableHolder
{
    public int step;
    public Vector2 velocity;
    public pathSpot path;
    public float range;
    public bool overrideDefaults;

}

