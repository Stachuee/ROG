using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{

    public static CameraEffects cameraEffects;
    [SerializeField]
    float strength;
    [SerializeField]
    float timeBetweenShakes;

    [SerializeField]
    float maxOffset;
    Vector3 startingPosition;

    Camera cam;

    private void Awake()
    {
        if (cameraEffects == null) cameraEffects = this;
        else Destroy(gameObject);
        cam = Camera.main;
        startingPosition = transform.position;
    }


    public void StartCameraShake(float endingTime)
    {
        InvokeRepeating("CamShake", 0, timeBetweenShakes); // start shaking and set ending time
        Invoke("StopCamShake", endingTime);
    }

    void CamShake() // set new camera position
    {
        Vector3 pos = cam.transform.position;

        float offsetX = Random.value * strength * 2 - strength;
        float offsetY = Random.value * strength * 2 - strength;

        pos.x += offsetX;
        pos.y += offsetY;

        cam.transform.position = new Vector3(Mathf.Clamp(pos.x, -maxOffset + startingPosition.x, maxOffset + startingPosition.x), Mathf.Clamp(pos.y, -maxOffset + startingPosition.y, maxOffset + startingPosition.y), -10);
    }

    void StopCamShake()
    {
        CancelInvoke("CamShake");
        cam.transform.position = startingPosition; // reste cam position
    }

}
