using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unit Behaviours base class
/// </summary>
public abstract class Behavior : ScriptableObject {

    public abstract void Init(unit myUnit, BehaviorHolder myHolder);
    public abstract bool Run(unit myUnit, BehaviorHolder myHolder);

    /// <summary>
    /// Handle a particular message that is sent to this behavior
    /// </summary>
    /// <param name="whichMsg"></param>
    /// <param name="whichUnit"></param>
    public virtual void GetMessage(msg whichMsg, unit whichUnit, BehaviorHolder holder)
    {
        //do nothing
    }

}
