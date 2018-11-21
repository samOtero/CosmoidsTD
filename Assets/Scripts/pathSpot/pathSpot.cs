using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class pathSpot : MonoBehaviour {

    /// <summary>
    /// Position of path in the current path
    /// </summary>
    public int Position;

    /// <summary>
    /// Path Id to differentiate with other paths
    /// </summary>
    public string Id;

    public PathRuntimeCollection pathList;

    private void OnEnable()
    {
        pathList.Add(this);

        //sort our path to be in the right order
        pathList.Items = pathList.Items.OrderBy(m => m.Position).ToList();
    }

    private void OnDisable()
    {
        pathList.Remove(this);
    }
}
