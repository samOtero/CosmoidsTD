using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject {

    public int GlobalCooldown = 60;
    public int baseCooldown;
    public int power;

    public abstract bool Trigger(unit abilityOwner, AbilityHolder myHolder);
}
