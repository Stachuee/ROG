using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueBox // Message struct
    {
        public string message;
        public string speaker;
        public Sprite speakreSprite;
    }
    int number = -1; //index of message

    [SerializeField]
    DialogueBox[] dialogues; // All messages

    [SerializeField]
    Text textBox;
    [SerializeField]
    Text nameBox;
    [SerializeField]
    Image speaker;

    AudioSource characterWritten;

    bool AllTextShown = true; // Is active message done yet

    private void Awake()
    {
        characterWritten = transform.GetComponent<AudioSource>();
    }

    private void Start()
    {
        if(dialogues.Length > 0)
        {
            ShowDialogue(); // show first dialogue
        }
    }


    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            ShowDialogue(); // After pressing fire show next message
        }
    }

    void ShowDialogue()
    {
        if (!AllTextShown) // If still writing message stop, show whole message and set flag
        {
            textBox.text = dialogues[number].message;
            StopAllCoroutines();
            AllTextShown = true;
            characterWritten.Stop();
            return;
        }
        number++;
        if (number >= dialogues.Length) {
            SceneManager.LoadScene(1); // If done with messages, then load next scene
            return;
        }
        StopAllCoroutines();
        message = "";
        StartCoroutine("ShowLetter");
        nameBox.text = dialogues[number].speaker;
        speaker.sprite = dialogues[number].speakreSprite; //setup all basic info about speaker
    }

    string message;
    IEnumerator ShowLetter()
    {
        AllTextShown = false;
        characterWritten.Play(); // play looping sound 
        for (int i = 0; i < dialogues[number].message.Length; i++)
        {
            message += dialogues[number].message[i]; // add enxt letter to string and show
            textBox.text = message;
            yield return new WaitForSeconds(0.02f);
        }
        AllTextShown = true;
        characterWritten.Stop(); // stop looping sound
    }

}
