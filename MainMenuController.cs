using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    [SerializeField]
    Text bestTime;
    [SerializeField]
    GameObject timeLabel;

    private void Start()
    {
        float time = PlayerPrefs.GetFloat("BestTime"); // get and show best time
        if (time != 0)
        {
            float minutes = Mathf.Floor(time / 60);
            float seconds = Mathf.Floor(time % 60);
            float miliSeconds = Mathf.Floor((time - (seconds + minutes * 60)) * 100);
            bestTime.text = (minutes < 10 ? "0" + minutes.ToString() : minutes.ToString()) + ":" + (seconds < 10 ? "0" + seconds.ToString() : seconds.ToString()) + ":" + (miliSeconds < 10 ? "0" + miliSeconds.ToString() : miliSeconds.ToString());
        }
        else
        {
            bestTime.text = "--:--:--";
        }
    }


    public void Play()
    {
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
