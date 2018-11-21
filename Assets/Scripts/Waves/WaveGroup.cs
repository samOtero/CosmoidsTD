using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveGroup {

    public int waveNum;
    public List<int> stepList;
    public unit unitPrefab;
    public float timeBetweenSpawns;
    public float timeAfterGroup;
    public int numToSpawn;
}
