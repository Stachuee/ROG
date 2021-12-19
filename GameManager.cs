using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public float timer;

    CanvasController canvas;

    private void Awake()
    {
        if (gameManager == null) gameManager = this;
        else Destroy(gameManager);
    }

    private void Start()
    {
        canvas = CanvasController.canvasController;
        Time.timeScale = 1; //resterat time scale
    }


    private void Update()
    {
        timer += Time.deltaTime; // update clock
    }


    public void EndGame(bool win)
    {
        if(win)
        {
            float bestTime = PlayerPrefs.GetFloat("BestTime");
            if ((bestTime != 0 && bestTime > timer) || bestTime == 0) PlayerPrefs.SetFloat("BestTime", timer);// update bext time
        }
        canvas.ShowEnding(win, timer); // show end screen
        Time.timeScale = 0; // stop gameplay loop
    }



    public void Reload()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}

