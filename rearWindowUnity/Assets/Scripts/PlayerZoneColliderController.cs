using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZoneColliderController : MonoBehaviour
{
    Rigidbody rb;
    PlayerController pc;
    GameManager gm;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        gm = GameManager.instance;
        pc = gm.pc;

        transform.SetParent(null);
    }

    private void Update()
    {
        transform.position = pc.transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            float distance = 100;
            CameraZoneController activeCamera = null;
            foreach(CameraZoneController camZone in gm.cameraZoneControllers)
            {
                if (camZone.gameObject.name == other.gameObject.name)
                {
                    float newDistance = Vector3.Distance(transform.position, camZone.gameObject.transform.position);
                    if (newDistance < distance)
                    {
                        distance = newDistance;
                        activeCamera = camZone;
                    }
                }
            }
            if (activeCamera)
                activeCamera.SetActive();
        }
    }
}