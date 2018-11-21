using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic list for units
/// </summary>
[CreateAssetMenu(menuName = "Collections/Unit")]
public class UnitRuntimeCollection : RuntimeCollection<unit>
{

    //Reset out unit list, since we don't need to persist
    private void OnEnable()
    {
        Items = new List<unit>();
    }
}
