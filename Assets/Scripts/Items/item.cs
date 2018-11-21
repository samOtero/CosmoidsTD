using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour {

    public unit holder;
    public itemID id;
    public ItemRuntimeCollection list;

    private void OnEnable()
    {
        list.Add(this);
    }

    private void OnDisable()
    {
        list.Remove(this);
    }
}

/// <summary>
/// List of item types
/// </summary>
public enum itemID
{
    meteor
}
