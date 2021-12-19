using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager musicManager;

    private void Awake()
    {
        if (musicManager == null) musicManager = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);   
    }
}
