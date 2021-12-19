using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static CanvasController canvasController;

    [SerializeField]
    Image batteryFill;
    [SerializeField]
    Text timer;

    [SerializeField]
    GameObject winEnd;
    [SerializeField]
    GameObject loseEnd;

    [SerializeField]
    Text bestTime;
    [SerializeField]
    Text currentTime;
    [SerializeField]
    Sprite[] gunIcons;
    [SerializeField]
    Image gunIconImage;


    private PlayerControllerScript playerController;
    private GameManager gm;

    private void Awake()
    {
        if (canvasController == null) canvasController = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playerController = PlayerControllerScript.playerController;
        gm = GameManager.gameManager;
    }

    void Update()
    {
        gunIconImage.sprite = gunIcons[playerController.Charges]; // show icon of currently held gun
        batteryFill.fillAmount = ((float)playerController.Charges / (float)playerController.maxCharges) * 0.8f; // show battery charge
        timer.text = GetStringFromTime(gm.timer); // update timer
    }

    public void ShowEnding(bool win, float time) //if game ends, check if won and compare times
    {
        if(win)
        {
            float bestRecordedTime = PlayerPrefs.GetFloat("BestTime");
            if(time < bestRecordedTime)
            {
                currentTime.text = GetStringFromTime(time);
                bestTime.text = currentTime.text;
            }
            else
            {
                currentTime.text = GetStringFromTime(time);
                bestTime.text = GetStringFromTime(bestRecordedTime);
            }
            winEnd.SetActive(true);
        }
        else
        {
            loseEnd.SetActive(true);
        }
    }

    public string GetStringFromTime(float time) // get string from timer float
    {
        float minutes = Mathf.Floor(time / 60);
        float seconds = Mathf.Floor(time % 60);
        float miliSeconds = Mathf.Floor((time - (seconds + minutes * 60)) * 100);
        return (minutes < 10 ? "0" + minutes.ToString() : minutes.ToString()) + ":" + (seconds < 10 ? "0" + seconds.ToString() : seconds.ToString()) + ":" + (miliSeconds < 10 ? "0" + miliSeconds.ToString() : miliSeconds.ToString());
    }

}
