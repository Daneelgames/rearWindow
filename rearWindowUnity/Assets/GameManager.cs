using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector]
    public PlayerController pc;
    [HideInInspector]
    public List<CameraZoneController> cameraZoneControllers;
    [HideInInspector]
    public TextController tc;

    public List<InteractiveObjectController> interactiveObjectControllers;
    public CameraZoneController activeCameraZone;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}