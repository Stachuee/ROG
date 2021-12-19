using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterPadScript : MonoBehaviour, InteractableInterface
{
    TeleporterScript parrent;

    [SerializeField]
    int direction;

    [SerializeField]
    Transform nextTarget;
    [SerializeField]
    bool canBeUsedByEnemies;

    List<GameObject> ignore = new List<GameObject>();

    bool active;

    private void Start()
    {
        parrent = transform.GetComponentInParent<TeleporterScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ignore.Contains(collision.gameObject)) {
            if (collision.tag == "Enemy" && canBeUsedByEnemies) // check if teleport can be used by enemy, and if so teleport it
            {
                collision.gameObject.GetComponent<EnemyInterface>().SetTarget(nextTarget);
                parrent.Teleport(direction, collision.gameObject, collision.transform.position - transform.position);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ignore.Remove(collision.gameObject);
    }

    public void AddToIgnore(GameObject target)
    {
        ignore.Add(target);
    }

    public int PowerUp(int ownedCharges) // teleport player
    {
        parrent.Teleport(direction, PlayerControllerScript.playerController.gameObject, PlayerControllerScript.playerController.transform.position - transform.position);
        return 0;
    }

    public void Damage(int damage)
    {

    }

    public int Drain(int damage)
    {
        return 0;
    }
}
