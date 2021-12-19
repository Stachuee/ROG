using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BansheScript : MonoBehaviour, EnemyInterface
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
    float drainRange;
    [SerializeField]
    float drainCooldown;
    float drainCooldownRemian;
    [SerializeField]
    float knockbackDuration;


    [SerializeField]
    Transform target;
    Rigidbody2D myBody;
    SpriteRenderer myRenderer;

    PlayerControllerScript player;

    Animator myanimator;

    List<InteractableInterface> interactableInRange = new List<InteractableInterface>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="Interactable")
        {
            interactableInRange.Add(collision.GetComponent<InteractableInterface>()); // Add object to drain target list 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            interactableInRange.Remove(collision.GetComponent<InteractableInterface>()); // Remove object from drain target list 
        }
    }

    private void Start()
    {
        player = PlayerControllerScript.playerController;
        myBody = transform.GetComponent<Rigidbody2D>();
        myRenderer = transform.GetComponent<SpriteRenderer>();
        myanimator = transform.GetComponent<Animator>();
        InvokeRepeating("DrainPower", 0, 3); // Call drain function every 3 seconds
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
        if(Vector2.Distance(player.transform.position, transform.position) <= drainRange && drainCooldownRemian <= 0) //if player in range remove all battery charges
        {
            player.LooseAllCharges();
            drainCooldownRemian = drainCooldown;
        }
        if (transform.position.x > target.transform.position.x) myRenderer.flipX = false;
        else myRenderer.flipX = true;

        drainCooldownRemian -= Time.deltaTime;
        knockbackDuration -= Time.deltaTime;
    }


    void DrainPower()
    {
        foreach(InteractableInterface inter in interactableInRange)
        {
            if (inter.Drain(1) > 0) // drain all devices, and restore hp for all drained
            {
                myRenderer.color = Color.green;
                CancelInvoke("NormalColor");
                Invoke("NormalColor", 0.2f);
                hp += Mathf.CeilToInt(maxHp * 0.05f);
                hp = Mathf.Clamp(hp, 0, maxHp);
            }
        }
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, drainRange);
    }

    public void Knockback(Vector2 strength)
    {
        knockbackDuration = 0.1f; // set knockback strenght and duration
        myBody.AddForce(strength * 0.2f);
    }
}
