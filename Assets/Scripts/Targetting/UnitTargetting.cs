using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for different targetting modes for units
/// </summary>
public abstract class UnitTargetting : ScriptableObject
{

    public UnitRuntimeCollection targetList;

    public abstract unit GetTarget(Vector3 startPos, float range);
}
