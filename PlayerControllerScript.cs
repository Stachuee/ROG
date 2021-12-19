using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerControllerScript : MonoBehaviour
{
    //singleton
    public static PlayerControllerScript playerController;


    //movment variables
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float jumpForce;
    bool isFloating;
    bool isFacingRight;

    //battery variables
    public int Charges {
        get
        {
            return currentCharges;
        }
        set
        {
            currentCharges = value;
            switch(currentCharges) //get gun based on charge level
            {
                case 0:
                    gun = new Pistol(PistolBulletPrefab, this);
                    break;
                case 1:
                    gun = new SMG(SMGBulletPrefab, this);
                    break;
                case 2:
                    gun = new AR(ARBulletPrefab, this);
                    break;
                case 3:
                    gun = new Shotgun(ShotgunBulletPrefab, this);
                    break;
                case 4:
                    gun = new Sniper(SniperBulletPrefab, this);
                    break;
                case 5:
                    gun = new RPG(RPGBulletPrefab, this);
                    break;
                case 6:
                    gun = new RGP(RGPBulletPrefab, this);
                    break;
            }
        }
    }
    private int currentCharges;
    public int maxCharges;

    //use variables
    [SerializeField]
    float useRadius;

    //gun
    GunBase gun;
    [SerializeField]
    GameObject PistolBulletPrefab;
    [SerializeField]
    GameObject SMGBulletPrefab;
    [SerializeField]
    GameObject ARBulletPrefab;
    [SerializeField]
    GameObject ShotgunBulletPrefab;
    [SerializeField]
    GameObject SniperBulletPrefab;
    [SerializeField]
    GameObject RPGBulletPrefab;
    [SerializeField]
    GameObject RGPBulletPrefab;


    //references
    Rigidbody2D myBody;
    SpriteRenderer myRenderer;
    Animator myAnimator;
    [SerializeField]
    Transform floorCheck;
    [SerializeField]
    GameObject barrel;
    [SerializeField]
    Transform flashlight;
    [SerializeField]
    GameObject muzzleFlesh;

    //layers masks
    [SerializeField]
    LayerMask ground;

    //additional variables
    [SerializeField]
    Vector2 groundCheckSize;



    private void Awake()
    {
        if (playerController == null) playerController = this;
        else Destroy(gameObject);

        myBody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        myBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Not sure about performance

        gun = new Pistol(PistolBulletPrefab, this); // start with pistol
    }


    void Update()
    {
        #region Inputs
        if (Input.GetButtonDown("Jump") && !isFloating)
        {
            myBody.AddForce(Vector2.up * jumpForce); // add force to jump
        }

        if(Input.GetButton("Fire1"))
        {
            Fire();
        }

        if(Input.GetButtonDown("Fire2"))
        {
            Action();
        }

        #endregion

        if (isFacingRight)
        {
            myRenderer.flipX = true;
            barrel.transform.localPosition = new Vector3(1f, barrel.transform.localPosition.y, 0);
            flashlight.rotation = Quaternion.Euler(0, 0, -90); // flip sprite and barrel if needed
        }
        else
        {
            myRenderer.flipX = false;
            barrel.transform.localPosition = new Vector3(-1f, barrel.transform.localPosition.y, 0);
            flashlight.rotation = Quaternion.Euler(0, 0, 90);
        }
        

        myAnimator.SetFloat("SpeedX", Math.Abs(myBody.velocity.x)); // set animator values
        myAnimator.SetFloat("SpeedY", Math.Abs(myBody.velocity.y));
        CheciIfFlying();
    }


    private void FixedUpdate()
    {
        MovePlayer();
    }

    void CheciIfFlying()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(floorCheck.position, groundCheckSize, 0, ground); //check if standing on a platform

        if (hits.Length != 0) isFloating = false;
        else isFloating = true;
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal > 0) isFacingRight = true;
        else if(horizontal < 0) isFacingRight = false;
        myBody.velocity = new Vector2(horizontal * maxSpeed * Time.fixedDeltaTime, myBody.velocity.y); // move player on x axis based on horizontal input axis
    }


    void Fire()
    {
        if(gun.Shoot()) // if can shoot, show muzzle flash, and hide it after 0.1f
        {
            muzzleFlesh.SetActive(true);
            Invoke("HideMuzzleFlesh", 0.1f);
        }
    }

    void HideMuzzleFlesh()
    {
        muzzleFlesh.SetActive(false);
    }

    public void InstantiateBullet(GameObject prefab, float offset) // Instantiate bullet facing forward with set recoil 
    {
        Instantiate(prefab, barrel.transform.position, isFacingRight ? Quaternion.Euler(0, 0, 0 + offset) : Quaternion.Euler(0, 0, 180 + offset));
    }

    private void Action() // check if any interactable objects near and activate them
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, useRadius);

        foreach(Collider2D hit in hits)
        {
            if(hit.transform.tag == "Interactable")
            {
                Charges -= hit.GetComponent<InteractableInterface>().PowerUp(Charges);
            }
        }
    }


    public void GainCharge() // if not at full charges, then gain one
    {
        if (Charges < maxCharges) Charges++;
    }

    public void LooseAllCharges()
    {
        Charges = 0;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(floorCheck.transform.position, groundCheckSize);
    }
}
