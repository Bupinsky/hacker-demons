using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;

    public Image actorImage;
    public Text actorName;
    public Text messageText;
    public RectTransform backgroundBox;

    Message[] currentMessages;
    Actor[] currentActors;
    int activeMessage = 0;
    public static bool isActive = false;
    //prevents accidental text skipping
    public static bool isReady = false;

    //toggle renderer to make message box invisible/visible
    private CanvasRenderer rend;

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;

        // activate
        isActive = true;
        rend.SetAlpha(1);
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);


        DisplayMessage();
    }

    void DisplayMessage()
    {
        isReady = false;
        Message messageToDisplay = currentMessages[activeMessage];
        //messageText.text = messageToDisplay.message;
        Say(messageToDisplay.message);

        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
    }

    public void NextMessage()
    {
        isReady = false;
        activeMessage++;
        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        } else
        {

            //deactivate
            isActive = false;
            rend.SetAlpha(0);
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);


            isReady = false;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rend = GetComponent<CanvasRenderer>();

        //deactivate
        isActive = false;
        rend.SetAlpha(0);
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isReady == true)
        {
            NextMessage();
        }
    }

    public void Say(string speech)
    {
        StopSpeaking();

        speaking = StartCoroutine(Speaking(speech, false));
    }

    public void SayAdd(string speech)
    {
        StopSpeaking();
        messageText.text = targetSpeech;
        speaking = StartCoroutine(Speaking(speech, true));
    }

    public void StopSpeaking()
    {
        if (isSpeaking)
        {
            StopCoroutine(speaking);
        }
        speaking = null;
    }

    public bool isSpeaking {get{ return speaking != null;}}
    [HideInInspector] public bool isWaitingForUserInput = false;

    Coroutine speaking = null;

    string targetSpeech = "";
    IEnumerator Speaking(string speech, bool additive)
    {
        targetSpeech = speech;

        if (!additive)
            messageText.text = "";
        else
            targetSpeech = messageText.text + targetSpeech;

        while(messageText.text != targetSpeech)
        {
            messageText.text += targetSpeech[messageText.text.Length];
            //sentence enders will cause a longer pause before the next character is revealed
            if (messageText.text.Length > 0
                && targetSpeech[messageText.text.Length-1] == '!'
                || targetSpeech[messageText.text.Length-1] == '.'
                || targetSpeech[messageText.text.Length-1] == '?'
                || targetSpeech[messageText.text.Length-1] == ',')
            {
                yield return new WaitForSeconds(0.06f);
            }
            yield return new WaitForSeconds(0.02f);
            //yield return new WaitForEndOfFrame();
        }

        //text finished
        isReady = true;
        isWaitingForUserInput = true;
        while (isWaitingForUserInput)
            yield return new WaitForEndOfFrame();

        StopSpeaking();
    }
}
