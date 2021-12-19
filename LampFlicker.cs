using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LampFlicker : MonoBehaviour
{

    Light2D myLight;

    void Start()
    {
        myLight = GetComponent<Light2D>();
        StartCoroutine("Flicker");
    }

    IEnumerator Flicker()
    {
        while (true) // flicker every 1 to 20 seconds
        {
            yield return new WaitForSeconds(Random.Range(1, 20));
            myLight.intensity = Random.Range(0.5f, 0.9f);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            myLight.intensity = 0.82f;
        }
    }
    public void StopFlicker() // turn it off
    {
        myLight.intensity = 0;
        StopAllCoroutines();
    }

    public void StartFlicker() // turn it on
    {
        StopAllCoroutines();
        myLight.intensity = 0.82f;
        StartCoroutine("Flicker");
    }

}
