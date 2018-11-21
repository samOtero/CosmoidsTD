using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelWave : MonoBehaviour {

    public List<WaveGroup> WaveGroups;

    public int currentWaveNum;
    public int totalWaves;
    public int currentStep;
    public int waveCounter;
    public int waveInitialCooldown;
    public bool forceStartWave;
    public int groupSpawnCounter;
    public bool finished;

    public WaveGroup currentWaveGroup;

	// Use this for initialization
	void Start () {

        waveCounter = waveInitialCooldown;
	}
	
	// Update is called once per frame
	void Update () {

        if (finished)
            return;

        if (waveCounter > 0 && forceStartWave == false)
        {
            waveCounter--;
            return;
        }

        forceStartWave = false;

        var waveGroup = GetNextGroup();

        //We have a next wave
        if (waveGroup != null)
        {
            //Starting the next wave group
            if (groupSpawnCounter == 0)
            {
                groupSpawnCounter = waveGroup.numToSpawn;
            }

            waveCounter = waveGroup.timeBetweenSpawns;
            groupSpawnCounter--;

            //Spawn unit
            var newPos = new Vector3(11.75f, 4.03f, 0f);
            var unit = Instantiate(waveGroup.unitPrefab.gameObject);
            unit.transform.localPosition = newPos;

            //We finished this group
            if (groupSpawnCounter == 0)
            {
                currentStep++;
            }
        }
        else
        {
            currentStep = 0;
            currentWaveNum++;
            if (currentWaveNum > totalWaves)
                finished = true;
        }
	}

    private WaveGroup GetNextGroup()
    {
        WaveGroup nextGroup = WaveGroups.Where(m => m.waveNum == currentWaveNum && m.stepList.Contains(currentStep)).FirstOrDefault();
        return nextGroup;
    }
}
