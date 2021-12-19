using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TurretScript : MonoBehaviour, InteractableInterface
{
    [SerializeField]
    float range;
    [SerializeField]
    int damage;
    [SerializeField]
    float fireRate;
    float toFire;

    [SerializeField]
    Animator animator;

    [SerializeField]
    Transform head;
    SpriteRenderer headRenderer;
    [SerializeField]
    AudioSource shoot;
    [SerializeField]
    AudioSource powerUp;


    [SerializeField]
    Sprite[] energyBars;
    [SerializeField]
    SpriteRenderer barRenderer;

    [SerializeField]
    Light2D barrelLight;

    [SerializeField]
    float durationRemain;

    [SerializeField]
    float durationDrainPerHit;

    [SerializeField]
    int batteriesPerCharge;

    [SerializeField]
    float maxDuration;

    [SerializeField]
    LayerMask toIgnore;

    GameObject target;

    private void Start()
    {
        headRenderer = head.GetComponent<SpriteRenderer>();
        InvokeRepeating("ScanForClosestEnemy", UnityEngine.Random.Range(0, 0.1f), 0.1f); // scan for closest enemy every x seconds
    }

    private void Update()
    {
        barRenderer.sprite = energyBars[Mathf.CeilToInt(durationRemain / maxDuration * (energyBars.Length - 1))]; // set energy bar indicator
        if (durationRemain < 0) return; //if out of power, then return
        if(toFire <= 0 && Fire()) // if ready to fire, fire
        {
            toFire = 1/fireRate;
        }

        if(target != null) // look at enemy
        {
            Vector3 diff = (target.transform.position - head.transform.position).normalized;
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            if (rot_z < 90 && rot_z > -90) headRenderer.flipY = true;
            else headRenderer.flipY = false;
            head.rotation = Quaternion.Euler(0f, 0f, rot_z - 180);
        }

        toFire -= Time.deltaTime;
        durationRemain -= Time.deltaTime;
    }

    private bool Fire()
    {
        if (target != null) // show muzzleflash and play sound
        {
            animator.SetTrigger("Fire");
            barrelLight.enabled = true;
            Invoke("HideLight", 0.3f);
            target.GetComponent<EnemyInterface>().Hit(damage);
            shoot.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            shoot.Play();
            return true;
        }
        return false;
    }

    void HideLight()
    {
        barrelLight.enabled = false;
    }

    public void Damage(int damage)
    {
        if (durationRemain > 0) durationRemain -= durationDrainPerHit * damage;
    }

    public int Drain(int damage)
    {
        if(durationRemain > 0)
        {
            durationRemain -= durationDrainPerHit * damage;
            return 1;
        }
        return 0;
    }

    public int PowerUp(int ownedCharges)
    {
        if(ownedCharges >= batteriesPerCharge)
        {
            durationRemain = maxDuration;
            powerUp.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            powerUp.Play();
            return batteriesPerCharge;
        }
        return 0;
    }


    void ScanForClosestEnemy() // get closest enemy taht it can see from list in spawner
    {
        EnemyInterface temp;
        GameObject enemyObject = gameObject;
        float distanceToClosest = Mathf.Infinity;

        foreach(EnemyInterface enemy in SpawnerController.enemiesPresent)
        {
            float distance = Vector2.Distance(enemy.getPosition(), transform.position);
            if (distance <= range && distance < distanceToClosest)
            {
                RaycastHit2D hit = Physics2D.Linecast(transform.position, enemy.getEnemy().transform.position, ~toIgnore);
                if (hit.transform == enemy.getEnemy().transform)
                {
                    temp = enemy;
                    distanceToClosest = distance;
                    enemyObject = temp.getEnemy();
                }
            }
        }
        if (distanceToClosest != Mathf.Infinity) {
            target = enemyObject;
        }
        else target = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
