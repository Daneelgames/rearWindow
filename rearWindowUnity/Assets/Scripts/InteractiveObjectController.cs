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
    string currentLine;

    AudioSource _audio;
    TextController tc;
    GameManager gm;

    bool interacting;
    float interactingDelay = 0f;

    private void Start()
    {
        gm = GameManager.instance;
        tc = gm.tc;
        gm.interactiveObjectControllers.Add(this);
        ToggleFeedback(false);
        _audio = GetComponent<AudioSource>();

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
        if (interactingDelay <= 0)
            StartCoroutine(InteractingDelay());
        if (!interacting)
        {
            interacting = true;
            ToggleFeedback(false);

            if (_audio)
                _audio.Play();
            if (cam)
            {
                gm.activeCameraZone.enabled = false;
                cam.enabled = true;
                camAudioListener.enabled = true;
            }
            if (anim)
                anim.SetBool("Action", true);

            if (dialogues.Count > 0)
            {
                StartCoroutine(AnimateText(dialogues[0].phrases[0]));
            }
        }
        else
        {
            if (interactingDelay <= 0)
            {
                if (textToDisplay == currentLine)
                {
                    // skip line
                }
                else
                {
                    // 
                }
            }
        }
    }

    IEnumerator InteractingDelay()
    {
        interactingDelay = 0.5f;
        while (interactingDelay > 0)
        {
            print(interactingDelay);

            interactingDelay -= Time.deltaTime;
            yield return null;
        }
    }


    IEnumerator AnimateText(string strComplete)
    {
        int i = 0;
        textToDisplay = "";
        currentLine = strComplete;
        while (i < strComplete.Length)
        {
            textToDisplay += strComplete[i++];
            tc.SetText(textToDisplay);
            yield return new WaitForSeconds(0.05f);
        }
    }
}