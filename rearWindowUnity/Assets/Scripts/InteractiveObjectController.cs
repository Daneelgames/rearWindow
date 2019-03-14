using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lists
{
    public List<string> phrases = new List<string>();
}

public class InteractiveObjectController : MonoBehaviour
{
    public GameObject feedback;
    public Camera cam;
    AudioListener camAudioListener;
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

    ActionOnDialogue actionOnDialogue;

    private void Start()
    {
        gm = GameManager.instance;
        tc = gm.tc;
        gm.interactiveObjectControllers.Add(this);
        ToggleFeedback(false);
        _audio = GetComponent<AudioSource>();

        actionOnDialogue = GetComponent<ActionOnDialogue>();

        if (cam)
        {
            cam.enabled = false;
            camAudioListener = cam.gameObject.GetComponent<AudioListener>();
            camAudioListener.enabled = false;
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
                if (textToDisplay == dialogues[currentDialogue].phrases[currentLine])
                {
                    // skip line
                    if (currentLine < dialogues[currentDialogue].phrases.Count - 1)
                    {
                        currentLine += 1;
                        StartCoroutine(AnimateText(dialogues[currentDialogue].phrases[currentLine]));
                    }
                    else
                    {
                        print("here");
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
        if (cam)
        {
            gm.activeCameraZone.SetInactive();
            cam.enabled = true;
            camAudioListener.enabled = true;
        }
        if (anim)
            anim.SetBool("Action", true);

        if (dialogues.Count > 0)
        {
            StartCoroutine(AnimateText(dialogues[currentDialogue].phrases[currentLine]));
        }
    }

    void FinishLine()
    {
        textToDisplay = dialogues[currentDialogue].phrases[currentLine];
        tc.SetText(textToDisplay);
    }

    void FinishDialogue()
    {
        if (currentDialogue +1 < dialogues.Count)
        {
            actionOnDialogue.DialogIsOver(currentDialogue);
            currentDialogue += 1;
        }

        tc.SetText(null);
        interacting = false;
        ToggleFeedback(true);

        if (_audio)
            _audio.Stop();
        if (cam)
        {
            gm.activeCameraZone.SetActive();
            cam.enabled = false;
            camAudioListener.enabled = false;
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
        while (i < strComplete.Length)
        {
            if (textToDisplay == strComplete)
            {
                yield break;
            }
            textToDisplay += strComplete[i++];
            tc.SetText(textToDisplay);
            yield return new WaitForSeconds(0.05f);
        }
    }
}