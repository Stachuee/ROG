using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidScript : MonoBehaviour, EnemyInterface
{
    [SerializeField]
    int maxHp;
    [SerializeField]
    int hp;
    [SerializeField]
    float speed;
    [SerializeField]
    int damage;
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
            myBody.velocity = new Vector2(target.transform.position.x - transform.position.x, 0).normalized * speed * Time.fixedDeltaTime;
        }
    }

    private void Update()
    {
        if (transform.position.x > target.transform.position.x) myRenderer.flipX = false;
        else myRenderer.flipX = true;
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
        CancelInvoke("NormalColor");
        Invoke("NormalColor", 0.2f);
        if (hp <= 0)
        {
            SpawnerController.spawnerController.DestroyEnemy(this);
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
        return damage;
    }

    public void Knockback(Vector2 strength)
    {
        knockbackDuration = 0.25f;
        myBody.AddForce(strength * 0.5f);
    }
}
