using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WalkerScript : MonoBehaviour, EnemyInterface
{
    [SerializeField]
    int maxHp;
    [SerializeField]
    int hp;
    [SerializeField]
    float speed;
    [SerializeField]
    int damge;
    [SerializeField]
    float knockbackDuration;

    [SerializeField]
    Transform target;
    Rigidbody2D myBody;
    SpriteRenderer myRenderer;

    Animator myanimator;
    private void Start()
    {
        myBody = transform.GetComponent<Rigidbody2D>();
        myRenderer = transform.GetComponent<SpriteRenderer>();
        myanimator = transform.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (knockbackDuration > 0) return;
        if (target != null)
        {
            myBody.velocity = new Vector2(target.transform.position.x - transform.position.x, 0).normalized * speed * Time.fixedDeltaTime + new Vector2(0, myBody.velocity.y); //walk towards target
        }
    }

    private void Update()
    {
        if (transform.position.x > target.transform.position.x) myRenderer.flipX = true;
        else myRenderer.flipX = false; // check which way to flip sprite
        knockbackDuration -= Time.deltaTime;
    }


    public GameObject getEnemy()
    {
        return gameObject;
    }

    public Vector2 getPosition()
    {
        return transform.position;
    }

    public float Hit(int damage)
    {
        TakeDamage(damage);
        return damage;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    void TakeDamage(int ammount)
    {
        hp -= ammount;
        myRenderer.color = Color.red;
        CancelInvoke("NormalColor"); // Change color to red and after 0.2f change it back
        Invoke("NormalColor", 0.2f);
        if (hp <= 0)
        {
            SpawnerController.spawnerController.DestroyEnemy(this); // Call Spawner that im destoryed, play animation and destroy me after 0.2f
            Invoke("DestroyMe", 0.2f);
            myanimator.SetTrigger("Death");
            speed = 0;
        }
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }

    void NormalColor()
    {
        myRenderer.color = Color.white;
    }

    public int DealtDamage()
    {
        TakeDamage(hp);
        return damge;
    }

    public void Knockback(Vector2 strength)
    {
        knockbackDuration = 0.5f; // Knockback strength and duration
        myBody.AddForce(strength);
    }
}
