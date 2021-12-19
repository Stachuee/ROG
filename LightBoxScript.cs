using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightBoxScript : MonoBehaviour, InteractableInterface
{

    public static LightBoxScript lightBoxScript;

    [SerializeField]
    LampFlicker[] allLights;

    [SerializeField]
    float durationRemain;

    [SerializeField]
    float durationDrainPerHit;

    [SerializeField]
    int batteriesPerCharge;

    [SerializeField]
    float maxDuration;


    [SerializeField]
    Sprite[] energyBars;
    [SerializeField]
    SpriteRenderer barRenderer;

    [SerializeField]
    AudioSource powerUp;
    [SerializeField]
    AudioSource powerDown;

    bool lightsOn = true;

    private void Awake()
    {
        if (lightBoxScript == null) lightBoxScript = this;
        else Destroy(gameObject);
    }

    public void Damage(int damage)
    {
        if (durationRemain > 0) durationRemain -= durationDrainPerHit * damage;
    }

    public int Drain(int damage)
    {
        if (durationRemain > 0)
        {
            durationRemain -= durationDrainPerHit * damage;
            return 1;
        }
        return 0;
    }

    public int PowerUp(int ownedCharges)
    {
        if (ownedCharges >= batteriesPerCharge) // power up box
        {
            durationRemain = maxDuration;
            powerUp.pitch = Random.Range(0.9f, 1.1f);
            powerUp.Play();
            return batteriesPerCharge;
        }
        return 0;
    }

    private void Update()
    {
        barRenderer.sprite = energyBars[Mathf.CeilToInt(Mathf.Clamp(durationRemain, 0, maxDuration) / maxDuration * (energyBars.Length - 1))];
        if (durationRemain <= 0 && lightsOn) // after running out of power, stop flickering
        {
            foreach(LampFlicker light in allLights)
            {
                light.StopFlicker();
                powerDown.pitch = Random.Range(0.9f, 1.1f);
                powerDown.Play();
            }
            lightsOn = false;
        }
        else if(durationRemain > 0 && !lightsOn) // after powering on light start flickering randomly 
        {
            foreach (LampFlicker light in allLights)
            {
                light.StartFlicker();
            }
            lightsOn = true;
        }
        durationRemain -= Time.deltaTime;
    }

}
