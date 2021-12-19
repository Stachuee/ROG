using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunBase
{
    protected Transform barrel;
    protected GameObject prefab;
    protected PlayerControllerScript player;
    protected float fireDelay;
    protected float nextShot;
    protected GunBase(GameObject prefab, PlayerControllerScript player) // constructor
    {
        this.prefab = prefab;
        this.player = player;
    }
    public abstract bool Shoot();
}


public class Pistol : GunBase
{
    public Pistol(GameObject prefab, PlayerControllerScript player) : base(prefab, player)
    {
        fireDelay = 0.5f; 
    }

    override public bool Shoot()
    {
        if(nextShot <= Time.time)
        {
            player.InstantiateBullet(prefab, 0); // Call player to create bullet with set recoil
            nextShot = Time.time + fireDelay;
            return true;
        }
        return false;
    }

}


public class SMG : GunBase
{
    public SMG(GameObject prefab, PlayerControllerScript player) : base(prefab, player)
    {
        fireDelay = 0.15f;
    }

    override public bool Shoot()
    {
        if (nextShot <= Time.time)
        {
            player.InstantiateBullet(prefab, 0);// Call player to create bullet with set recoil
            nextShot = Time.time + fireDelay;
            return true;
        }
        return false;
    }

}

public class AR : GunBase
{
    public AR(GameObject prefab, PlayerControllerScript player) : base(prefab, player)
    {
        fireDelay = 0.25f;
    }

    override public bool Shoot()
    {
        if (nextShot <= Time.time)
        {
            player.InstantiateBullet(prefab, 0);// Call player to create bullet with set recoil
            nextShot = Time.time + fireDelay;
            return true;
        }
        return false;
    }

}

public class Shotgun : GunBase
{
    public Shotgun(GameObject prefab, PlayerControllerScript player) : base(prefab, player)
    {
        fireDelay = 0.5f;
    }

    override public bool Shoot()
    {
        if (nextShot <= Time.time)
        {
            for(int i = 0; i < 8; i++)player.InstantiateBullet(prefab, Random.Range(-5,5));// Call player to create bullet with set recoil
            nextShot = Time.time + fireDelay;
            return true;
        }
        return false;
    }

}

public class Sniper : GunBase
{
    public Sniper(GameObject prefab, PlayerControllerScript player) : base(prefab, player)
    {
        fireDelay = 0.7f;
    }

    override public bool Shoot()
    {
        if (nextShot <= Time.time)
        {
            player.InstantiateBullet(prefab, 0);// Call player to create bullet with set recoil
            nextShot = Time.time + fireDelay;
            return true;
        }
        return false;
    }

}

public class RPG : GunBase
{
    public RPG(GameObject prefab, PlayerControllerScript player) : base(prefab, player)
    {
        fireDelay = 1.2f;
    }

    override public bool Shoot()
    {
        if (nextShot <= Time.time)
        {
            player.InstantiateBullet(prefab, 0);// Call player to create bullet with set recoil
            nextShot = Time.time + fireDelay;
            return true;
        }
        return false;
    }

}


public class RGP : GunBase
{
    public RGP(GameObject prefab, PlayerControllerScript player) : base(prefab, player)
    {
        fireDelay = 0.6f;
    }

    override public bool Shoot()
    {
        if (nextShot <= Time.time)
        {
            player.InstantiateBullet(prefab, 0);// Call player to create bullet with set recoil
            nextShot = Time.time + fireDelay;
            return true;
        }
        return false;
    }
}