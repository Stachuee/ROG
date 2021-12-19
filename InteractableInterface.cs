using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InteractableInterface 
{
    int PowerUp(int ownedCharges);
    void Damage(int damage);
    int Drain(int damage);
}
