using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Collections/Path")]
public class PathRuntimeCollection : RuntimeCollection<pathSpot> {

    //Reset out unit list, since we don't need to persist
    private void OnEnable()
    {
        Items = new List<pathSpot>();
    }
}
