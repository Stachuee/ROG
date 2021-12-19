using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class WaveSO : ScriptableObject
{
    public int minCharge;
    public int maxCharge;

    public float spawnDelay;
    public float nextWaveDelay;

    public int walkerCount;
    public int tankCount;
    public int droidCount;
    public int bansheCount;

    public Vector2 tentacleSpawnDelay;
}
