using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    int damage;
    [SerializeField]
    int penetration;
    [SerializeField]
    bool exploding;
    [SerializeField]
    float explosionRadius;
    bool exploded;
    [SerializeField]
    float knockbackStrength;
    [SerializeField]
    float maxDistance;
    float distance;

    [SerializeField]
    LayerMask notToHit;

    [SerializeField]
    AudioSource shoot;

    [SerializeField]
    AudioSource explodeSound;

    [SerializeField]
    Animator animator; 

    Vector2 prevPos;

    List<GameObject> ignore = new List<GameObject>(); // list af all object to ignore if penetration >1


    private void Start()
    {
        prevPos = transform.position;
        shoot.pitch = Random.Range(-0.1f, 0.1f) + shoot.pitch; // after spawning play gunshot noise
        shoot.Play();
    }

    void Update()
    {
        if (exploded) return; // if is explosive and playing explode animation
        transform.position += transform.right * speed * Time.deltaTime; // move bullet


       RaycastHit2D hit = Physics2D.Linecast(prevPos, transform.position, ~notToHit); // get all object in trajectory

        if(hit)
        {
            EnemyInterface hitInterface = hit.transform.GetComponent<EnemyInterface>(); 

            if (hitInterface != null && !ignore.Contains(hit.transform.gameObject))// if hit something first time and it has a EnemyInterface damage it and if explosive then explode
            {
                hitInterface.Hit(damage);
                if (exploding) Explode();
                ignore.Add(hit.transform.gameObject); // add to ignored list
                penetration--; // decrease penetration
            }
            if (penetration <= 0 && !exploding) Destroy(gameObject); // if not explosive and penetration is 0 then destory
            if (exploding) Explode(); //if its explosive, then explode
        }
        distance += Mathf.Abs(transform.position.x - prevPos.x); // add to traveled distance
        if (distance > maxDistance) // if at max range destroy if not explosive, or explode
        {
            if (exploding) Explode();
            else Destroy(gameObject);
        }
        prevPos = transform.position;
    }



    void Explode()
    {
        exploded = true;
        explodeSound.Play(); //play animation and sound
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, ~notToHit);
        animator.SetTrigger("Explode");
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length - 0.6f); //destory after animation
        speed = 0;
        foreach (Collider2D hit in hits)
        {
            EnemyInterface hitInterface = hit.transform.GetComponent<EnemyInterface>(); //cast circle, damage all enemies inside and knockback them
            if (hitInterface != null)
            {
                hitInterface.Hit(damage);
                hitInterface.Knockback((hitInterface.getPosition() - (Vector2)transform.position).normalized * knockbackStrength);
            }
        }

    }
}
