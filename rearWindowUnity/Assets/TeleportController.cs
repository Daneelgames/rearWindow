﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public string sceneName;
    public string spawner;

    public void Teleport()
    {
        StartCoroutine(GameManager.instance.Teleport(sceneName, spawner));
    }
}