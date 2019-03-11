using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoneController : MonoBehaviour
{
    [HideInInspector]
    public Camera cam;
    AudioListener camAudioListener;
    GameManager gm;

    private void Start()
    {
        cam = transform.parent.gameObject.GetComponent<Camera>();
        camAudioListener = transform.parent.gameObject.GetComponent<AudioListener>();
        gm = GameManager.instance;
        gm.cameraZoneControllers.Add(this);

        SetActive();
    }

    public void SetActive()
    {
        cam.enabled = true;
        camAudioListener.enabled = true;
        gm.activeCameraZone = this;
        foreach (CameraZoneController camZone in gm.cameraZoneControllers)
        {
            if (camZone != this)
            {
                camZone.cam.enabled = false;
                camAudioListener.enabled = false;
            }
        }
    }
}