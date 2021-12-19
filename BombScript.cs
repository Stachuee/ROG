using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class BombScript : MonoBehaviour, InteractableInterface
{

    [SerializeField]
    int charge;
    [SerializeField]
    int maxCharge;

    [SerializeField]
    float selfChargeDelay;

    [SerializeField]
    PodScript pod;

    bool isReadyToExplode;

    [SerializeField]
    Image bar1;
    [SerializeField]
    Image bar2;

    [SerializeField]
    AudioSource hit;
    [SerializeField]
    AudioSource powerUp;
    [SerializeField]
    AudioSource siren;

    Animator myAnimator;
    SpriteRenderer myRenderer;

    public static BombScript bombScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Damage(collision.GetComponent<EnemyInterface>().DealtDamage()); // When hit by enemy damage self and destroy enemy
        }
    }

    private void Awake()
    {
        if (bombScript == null) bombScript = this;
        else Destroy(gameObject);

        myAnimator = transform.GetComponent<Animator>();
        hit = transform.GetComponent<AudioSource>();
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating("AddCharge", selfChargeDelay, selfChargeDelay); // self cahrge every x seconds
    }

    void ChargeChange()
    {
        bar1.fillAmount = (float)charge / (float)maxCharge; // set visual indicator of charge
        bar2.fillAmount = (float)charge / (float)maxCharge;
    }

    public void AddCharge()
    {
        if(charge < maxCharge)
        {
            charge++; // if charge is les then max, increase it
        }

        if (charge >= maxCharge && !isReadyToExplode) {
            isReadyToExplode = true; // if charge at max, set bomb to invulnerable, play sound, and open pod 
            pod.OpenPod();
            siren.Play();
            myAnimator.SetTrigger("ReadyToExplode");
            CameraEffects.cameraEffects.StartCameraShake(3);
        }
        ChargeChange();
    }

    public int GetCharge()
    {
        return charge;
    }

    public int PowerUp(int ownedCharges) // get charge and play sound
    {
        if(ownedCharges >= 1)
        {
            AddCharge();
            powerUp.pitch = 0.8f + ((float)charge / (float)maxCharge) * 0.6f;
            powerUp.Play();
            return 1;
        }
        return 0;
    }

    public void Damage(int damage) // play sound and change color for a second
    {
        if (isReadyToExplode) return;
        charge -= damage;
        hit.Play();
        myRenderer.color = Color.red;
        CancelInvoke("NormalColor");
        Invoke("NormalColor", 0.2f);
        if (charge < 0)
        {
            GameManager.gameManager.EndGame(false);
            return;
        }
        ChargeChange();
    }


    void NormalColor()
    {
        myRenderer.color = Color.white;
    }

    public int Drain(int damage)
    {
        throw new System.NotImplementedException();
    }
}
