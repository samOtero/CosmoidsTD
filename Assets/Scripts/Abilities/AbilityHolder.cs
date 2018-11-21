using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityHolder {

    public int currentCooldown;
    public int phaseCooldown;
    public int phase;
    public Ability ability;

    private void OnEnable()
    {
        currentCooldown = ability.GlobalCooldown;
    }
    public void RunCooldown()
    {
        if (currentCooldown > 0)
            currentCooldown--;
    }

    public void Run(unit myUnit)
    {
        if (currentCooldown <= 0)
            if (ability != null && ability.Trigger(myUnit, this) == true)
                ResetCooldown();
    }

    public virtual void ResetCooldown()
    {
        currentCooldown = ability.baseCooldown; //Add other modifiers here later on
    }

    /// <summary>
    /// Makes sure abilities waits at least as much as the global cooldown
    /// </summary>
    public void TriggerGlobalCooldown()
    {
        if (currentCooldown < ability.GlobalCooldown)
            currentCooldown = ability.GlobalCooldown;
    }


}
