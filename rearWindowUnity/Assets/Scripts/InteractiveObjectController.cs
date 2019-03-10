using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObjectController : MonoBehaviour
{
    public GameObject feedback;
    public Camera cam;
    public Animator anim;

    AudioSource _audio;
    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
        gm.interactiveObjectControllers.Add(this);
        ToggleFeedback(false);
        _audio = GetComponent<AudioSource>();

        if (cam)
            cam.enabled = false;
    }

    public void ToggleFeedback(bool active)
    {
        feedback.SetActive(active);
    }

    public void Interact()
    {
        ToggleFeedback(false);

        if (_audio)
            _audio.Play();
        if (cam)
        {
            gm.activeCameraZone.enabled = false;
            cam.enabled = true;
        }
        if (anim)
            anim.SetBool("Action", true);
    }
}