using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScript : MonoBehaviour
{
    [SerializeField]
    bool active;
    bool linger;

    [SerializeField]
    float speed;

    [SerializeField]
    Transform[] anchors;

    [SerializeField]
    Transform pupil;

    [SerializeField]
    float maxFollow;

    Transform player;
    Transform target;

    private void Start()
    {
        GetDestination();
        player = PlayerControllerScript.playerController.transform;
    }

    private void Update()
    {
        if(active)
        {
            if(linger) // eye linger for a moment after reaching its destination
            {
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)target.position + new Vector2(0, Mathf.Sin(Time.time)), speed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, target.position) < 0.2f) {
                linger = true;
                Invoke("GetDestination", Random.Range(3,6));
            }
            else transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            pupil.transform.position = (player.position - transform.position).normalized * maxFollow + transform.position; // pupil follows player

        }
    }


    void GetDestination() // choose random next position fo follow
    {
        target = anchors[Random.Range(0, anchors.Length)];
        linger = false;
    }

    public void Activate()
    {
        active = true;
    }
}
