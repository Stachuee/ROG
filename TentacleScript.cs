using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TentacleScript : MonoBehaviour, EnemyInterface
{
    Animator myAnimator;
    SpriteRenderer myRenderer;
    [SerializeField]
    int maxHp;
    int hp;

    [SerializeField]
    GameObject bug;

    [SerializeField]
    float spawnStartDelay;
    [SerializeField]
    float spawnDelay;
    [SerializeField]
    Transform enemyTransform;
    [SerializeField]
    Transform firstTarget;

    Animator myanimator;

    private void Awake()
    {
        myAnimator = gameObject.GetComponent<Animator>();
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        myanimator = transform.GetComponent<Animator>();
    }

    public void OnEnable()
    {
        myAnimator.SetTrigger("Appear");
        StartCoroutine("StartSpawning");
        hp = maxHp;
    }

    private void OnDisable()
    {
        StopAllCoroutines(); //Stop all coroutines to avoid then stacing
    }

    public float Hit(int damage)
    {
        Damage(damage);
        return damage;
    }

    public Vector2 getPosition()
    {
        return transform.position;
    }

    public GameObject getEnemy()
    {
        return gameObject;
    }

    public void SetTarget(Transform target)
    {

    }

    IEnumerator StartSpawning()
    {
        yield return new WaitForSeconds(spawnStartDelay); //spawn bugs after x seconds
        while(true)
        {
            EnemyInterface temp = Instantiate(bug, transform.position, Quaternion.identity, enemyTransform).GetComponent<EnemyInterface>();
            temp.SetTarget(firstTarget);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void Damage(int ammount)
    {
        hp -= ammount;
        myRenderer.color = Color.red;
        CancelInvoke("NormalColor");
        Invoke("NormalColor", 0.2f);
        if (hp <= 0)
        {
            Invoke("DeactivateMe", 0.2f);
            myanimator.SetTrigger("Death");
        }
    }

    void DeactivateMe()
    {
        gameObject.SetActive(false);
    }

    void NormalColor()
    {
        myRenderer.color = Color.white;
    }

    public int DealtDamage()
    {
        return 0;
    }

    public void Knockback(Vector2 strength)
    {
        
    }
}
