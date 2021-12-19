using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyInterface
{
    float Hit(int damage); // damage enemy
    Vector2 getPosition(); // get its position
    GameObject getEnemy(); // get its object
    void Knockback(Vector2 strength); // knockback it

    int DealtDamage(); // Get its damage to bomb

    void SetTarget(Transform target); // set its next target to follow
}
