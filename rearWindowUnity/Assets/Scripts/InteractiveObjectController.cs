﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Phrase
{
    public string actorName;
    public string text;
    public Color textColor = Color.white;
    public Camera camera;
}
[System.Serializable]
public class Lists
{
    public List<Phrase> phrases = new List<Phrase>();
}


public class InteractiveObjectController : MonoBehaviour
{
    public GameObject feedback;
    public Animator anim;

    public List<Lists> dialogues = new List<Lists>();
    string textToDisplay;
    int currentDialogue = 0;
    int currentLine = 0;

    AudioSource _audio;
    TextController tc;
    GameManager gm;

    bool interacting;
    public float interactingDelayMax = 0.25f;
    float interactingDelay = 0f;
    public TeleportController teleport;

    Camera currentCamera;

    ActionOnDialogue actionOnDialogue;

    private void Start()
    {
        gm = GameManager.instance;
        tc = gm.tc;
        gm.interactiveObjectControllers.Add(this);
        ToggleFeedback(false);
        _audio = GetComponent<AudioSource>();

        actionOnDialogue = GetComponent<ActionOnDialogue>();

        if (dialogues.Count > 0)
        {
            foreach(Lists l in dialogues)
            {
                foreach(Phrase phrases in l.phrases)
                {
                    if (phrases.camera != null)
                    {
                        phrases.camera.enabled = false;
                        phrases.camera.gameObject.GetComponent<AudioListener>().enabled = false;
                    }
                }
            }
        }
    }

    public void ToggleFeedback(bool active)
    {
        feedback.SetActive(active);
    }

    public void Interact()
    {
        if (!interacting)
        {
            if (dialogues.Count > 0)
                StartDialogue();
            else if (teleport)
                teleport.Teleport();
        }
        else
        {
            if (interactingDelay <= 0)
            {
                Phrase newPhrase = dialogues[currentDialogue].phrases[currentLine];
                if (textToDisplay == newPhrase.actorName + ":\n" + newPhrase.text)
                {
                    // skip line
                    if (actionOnDialogue)
                        actionOnDialogue.LineIsOver(currentDialogue, currentLine);

                    if (currentLine < dialogues[currentDialogue].phrases.Count - 1)
                    {
                        currentLine += 1;
                        if (dialogues[currentDialogue].phrases[currentLine].camera)
                        {
                            currentCamera.enabled = false;
                            currentCamera.GetComponent<AudioListener>().enabled = false;
                            currentCamera = dialogues[currentDialogue].phrases[currentLine].camera;
                            //gm.activeCameraZone.SetInactive();
                            currentCamera.enabled = true;
                            currentCamera.GetComponent<AudioListener>().enabled = true;
                        }
                        StartCoroutine(AnimateText(dialogues[currentDialogue].phrases[currentLine].text));
                    }
                    else
                    {
                        FinishDialogue();
                    }
                }
                else
                {
                    FinishLine();
                }
            }
        }

        if (interactingDelay <= 0)
            StartCoroutine(InteractingDelay());
    }

    void StartDialogue()
    {
        interacting = true;
        ToggleFeedback(false);

        if (_audio)
            _audio.Play();
        if (anim)
            anim.SetBool("Action", true);

        if (dialogues.Count > 0)
        {
            StartCoroutine(AnimateText(dialogues[currentDialogue].phrases[currentLine].text));
            if (dialogues[currentDialogue].phrases[0].camera)
            {
                gm.activeCameraZone.SetInactive();
                currentCamera = dialogues[currentDialogue].phrases[0].camera;
                currentCamera.enabled = true;
                currentCamera.GetComponent<AudioListener>().enabled = true;
            }
        }
    }

    void FinishLine()
    {
        Phrase newPhrase = dialogues[currentDialogue].phrases[currentLine];
        textToDisplay = newPhrase.text;
        textToDisplay = newPhrase.actorName + ":\n" + textToDisplay;
        tc.SetText(textToDisplay, newPhrase.textColor);
    }

    void FinishDialogue()
    {
        if (currentDialogue +1 < dialogues.Count)
        {
            currentDialogue += 1;
        }

        tc.SetText(null, Color.black);
        interacting = false;
        ToggleFeedback(true);

        if (_audio)
            _audio.Stop();
        if (currentCamera != null)
        {
            gm.activeCameraZone.SetActive();
            currentCamera.enabled = false;
            currentCamera.GetComponent<AudioListener>().enabled = false;
            currentCamera = null;
        }
        if (anim)
            anim.SetBool("Action", false);

        currentLine = 0;
        StartCoroutine(gm.pc.CanMove());
    }

    IEnumerator InteractingDelay()
    {
        interactingDelay = interactingDelayMax;
        while (interactingDelay > 0)
        {
            interactingDelay -= Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator AnimateText(string strComplete)
    {
        int i = 0;
        textToDisplay = "";
        Phrase newPhrase = dialogues[currentDialogue].phrases[currentLine];
        strComplete = newPhrase.actorName + ":\n" + strComplete;
        while (i < strComplete.Length)
        {
            if (textToDisplay == strComplete)
            {
                yield break;
            }
            textToDisplay += strComplete[i++];
            tc.SetText(textToDisplay, newPhrase.textColor);
            yield return new WaitForSeconds(0.05f);
        }
    }
}