using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public static SpawnerController spawnerController;

    public static List<EnemyInterface> enemiesPresent;


    [SerializeField]
    EyeScript eye;
    [SerializeField]
    AudioSource kraken;

    [SerializeField]
    TentacleScript[] tentacles;

    [SerializeField]
    int chargeToSpawnKraken;
    [SerializeField]
    Vector2 timeBetweenSpawns;
    bool krakenActive;
    bool krakenEntranceDone;
    float nextTentacleSpawn;

    [SerializeField]
    GameObject walkerPrefab;
    [SerializeField]
    GameObject droidPrefab;
    [SerializeField]
    GameObject tankPrefab;
    [SerializeField]
    GameObject banshePrefab;

    [SerializeField]
    ParticleSystem rubble;

    [SerializeField]
    Transform[] anchor;

    [SerializeField]
    Transform enemyParrent;

    [SerializeField]
    Transform[] firstTarget;

    [SerializeField]
    WaveSO[] waves;

    WaveSO currentWave;

    float currentDelay;

    int spawnedWalkers;
    int spawnedDroids;
    int spawnedTanks;
    int spawnedBanshes;


    int enemiesToSpawn;

    bool cooldown = false;
    BombScript bomb;


    private void Awake()
    {
        if (spawnerController == null) spawnerController = this;
        else Destroy(gameObject);

        enemiesPresent = new List<EnemyInterface>(); //reset all spawned enemies list
    }

    private void Start()
    {
        bomb = BombScript.bombScript;
    }

    public void DestroyEnemy(EnemyInterface enemy) //remove enemy from list
    {
        enemiesPresent.Remove(enemy);
    }

    private void Update()
    {
        if(!cooldown && currentDelay <= 0)
        {
            SpawnNextEnemy(); // spawn next wave
            currentDelay = currentWave.spawnDelay;
        }
        currentDelay -= Time.deltaTime;

        if(krakenActive && nextTentacleSpawn <= 0) //spawn next tentacle
        {
            TentacleScript tentacle = tentacles[Random.Range(0, tentacles.Length)];
            if(!tentacle.isActiveAndEnabled)
            {
                tentacle.gameObject.SetActive(true);
            }
            nextTentacleSpawn = Random.Range(timeBetweenSpawns.x, timeBetweenSpawns.y);
        }
        nextTentacleSpawn -= Time.deltaTime;
    }


    void GetWave()
    {
        List<WaveSO> avalibleWaves = new List<WaveSO>();

        int charge = bomb.GetCharge();

        if (charge >= chargeToSpawnKraken && !krakenEntranceDone) // awaken kraken if not already spawned
        {
            InvokeRepeating("AwakenKraken", 0, Random.Range(60, 120));
            krakenEntranceDone = true;
        }

        foreach (WaveSO wave in waves) //check which wave is appropriate
        {
            if (wave.minCharge <= charge && wave.maxCharge > charge)
            {
                avalibleWaves.Add(wave);
            }
        }
        currentWave = avalibleWaves[Random.Range(0, avalibleWaves.Count)]; // get random wave from selected

        timeBetweenSpawns = currentWave.tentacleSpawnDelay;
        spawnedWalkers = 0; 
        spawnedDroids = 0;
        spawnedTanks = 0;
        spawnedBanshes = 0;
        enemiesToSpawn = currentWave.walkerCount + currentWave.bansheCount + currentWave.droidCount + currentWave.tankCount; //set all values
        currentDelay = currentWave.spawnDelay;
        cooldown = false;
    }

    void AwakenKraken() // shake camera, play particles and activate eye
    {
        CameraEffects.cameraEffects.StartCameraShake(3);
        krakenActive = true;
        eye.Activate();
        rubble.Play();
        kraken.Play();
    }

    void SpawnNextEnemy()
    {
        if (cooldown) return;
        if (currentWave == null) GetWave(); //If none left to spawn, get new wave 
        else if (enemiesToSpawn == 0)
        {
            Invoke("GetWave", currentWave.nextWaveDelay);
            cooldown = true;
            return;
        }

        bool spawned = false;
        while(!spawned)
        {
            int rand = Random.Range(0, 4), door = Random.Range(0, anchor.Length); //get random starting position and random mob
            GameObject temp = gameObject; // >:(
            switch (rand)
            {
                case 0:
                    if (spawnedWalkers < currentWave.walkerCount) // if no more mob left to spawn get next random one
                    {
                        temp = Instantiate(walkerPrefab, anchor[door].position, Quaternion.identity, enemyParrent);
                        spawnedWalkers++;
                        spawned = true;
                    }
                    break;
                case 1:
                    if (spawnedDroids < currentWave.droidCount)
                    {
                        temp = Instantiate(droidPrefab, anchor[door].position, Quaternion.identity, enemyParrent);
                        spawnedDroids++;
                        spawned = true;
                    }
                    break;
                case 2:
                    if (spawnedTanks < currentWave.tankCount)
                    {
                        temp = Instantiate(tankPrefab, anchor[door].position, Quaternion.identity, enemyParrent);
                        spawnedTanks++;
                        spawned = true;
                    }
                    break;
                case 3:
                    if (spawnedBanshes < currentWave.bansheCount)
                    {
                        temp = Instantiate(banshePrefab, anchor[door].position, Quaternion.identity, enemyParrent);
                        spawnedBanshes++;
                        spawned = true;
                    }
                    break;
            }

            if (spawned)
            {
                EnemyInterface tempInterface = temp.GetComponent<EnemyInterface>(); // set mob first target and add it to the list
                tempInterface.SetTarget(firstTarget[door]);
                enemiesPresent.Add(tempInterface);
                enemiesToSpawn--;
                break;
            }
        }
        
    }
}
