using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterScript : MonoBehaviour
{
    [SerializeField]
    TeleporterPadScript padOne;

    [SerializeField]
    TeleporterPadScript padTwo;

    public void Teleport(int direction, GameObject target, Vector2 targetOffset)
    {
        if (direction == 0) // check which way to teleport and add object to ignore od destination teleport
        {
            target.transform.position = (Vector2)padTwo.transform.position + targetOffset;
            padTwo.AddToIgnore(target);
        }
        else if(direction == 1)
        {
            target.transform.position = (Vector2)padOne.transform.position + targetOffset;
            padOne.AddToIgnore(target);
        }
    }
}
