using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public List<TextMeshProUGUI> textObjects;

    public void SetText(string newText)
    {
        foreach(TextMeshProUGUI text in textObjects)
        {
            text.text = newText;
        }
    }
}