using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodScript : MonoBehaviour, InteractableInterface
{
    [SerializeField]
    bool podOpen = false;

    [SerializeField]
    Sprite opened;

    public void Damage(int damage)
    {

    }

    public int Drain(int damage)
    {
        return 0;
    }

    public int PowerUp(int ownedCharges) // if its open and player use it, end game
    {
        if (podOpen) GameManager.gameManager.EndGame(true);
        return 0;
    }

    public void OpenPod() // if opened, change sprite
    {
        podOpen = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = opened;
    }

}
