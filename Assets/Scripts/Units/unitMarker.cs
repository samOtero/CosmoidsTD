using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitMarker : MonoBehaviour {

    public Transform unitPos;
	// Use this for initialization
	void Start () {
        Reset();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Update a marker's unit position
    public void Reset()
    {
        unitPos = transform.GetChild(0);
    }
}
