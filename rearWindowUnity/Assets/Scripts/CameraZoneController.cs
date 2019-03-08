using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoneController : MonoBehaviour
{
    public Camera cam;
    GameManager gm;

    private void Start()
    {
        cam = transform.parent.gameObject.GetComponent<Camera>();
        gm = GameManager.instance;
        gm.cameraZoneControllers.Add(this);
    }

    public void SetActive()
    {
        cam.enabled = true;
        foreach (CameraZoneController camZone in gm.cameraZoneControllers)
        {
            if (camZone != this)
                camZone.cam.enabled = false;
        }
    }
}