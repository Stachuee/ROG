using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(AudioSource))]
public class ElectricalBoxScript : MonoBehaviour
{
    [SerializeField]
    float cooldown;
    float cooldownRemian;

    [SerializeField]
    float range;

    PlayerControllerScript player;
    CircleCollider2D myCollider;

    bool inRange;

    AudioSource powerSound;

    private void OnTriggerEnter2D(Collider2D collision) // if player in range set flag
    {
        if (collision.gameObject.tag == "Player")
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)// if player not in range reset flag
    {
        if (collision.gameObject.tag == "Player")
        {
            inRange = false;
        }
    }

    private void Awake()
    {
        myCollider = GetComponent<CircleCollider2D>();
        powerSound = GetComponent<AudioSource>();
    }

    private void Start()
    {
        player = PlayerControllerScript.playerController;
        myCollider.radius = range;
    }

    private void Update()
    {
        if (inRange && cooldownRemian <= 0 && player.Charges != player.maxCharges) // if player in range and not fully charged, charge one par, play sound and set cooldown
        {
            player.GainCharge();
            powerSound.pitch = 0.9f + ((float)player.Charges / (float)player.maxCharges) * 0.5f;
            powerSound.Play();
            cooldownRemian = cooldown;
        }
        cooldownRemian -= Time.deltaTime;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
