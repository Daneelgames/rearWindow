using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    GameManager gm;

    public List<TextMeshProUGUI> textObjects;

    private void Start()
    {
        gm = GameManager.instance;
        gm.tc = this;
    }

    public void SetText(string newText)
    {
        foreach(TextMeshProUGUI text in textObjects)
        {
            text.text = newText;
        }
    }
}