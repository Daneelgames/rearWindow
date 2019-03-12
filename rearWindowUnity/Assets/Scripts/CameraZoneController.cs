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

        transform.SetParent(null);
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
                camZone.SetInactive();
            }
        }
        foreach(GameObject o in gm.objectsTurnToCamera)
        {
            o.transform.LookAt(cam.transform);
        }
    }

    public void SetInactive()
    {
        cam.enabled = false;
        camAudioListener.enabled = false;
    }
}