using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOnDialogue : MonoBehaviour
{
    [System.Serializable]
    public class Action
    {
        public GameObject objectToEnable;
        public GameObject objectToDisable;
        public int dialogue;
        public int phrase;
    }

    public List<Action> actions = new List<Action>();

    public void LineIsOver(int dialogueIndex, int phraseIndex)
    {
        foreach(Action a in actions)
        {
            if (a.dialogue == dialogueIndex)
            {
                if (a.phrase == phraseIndex)
                {
                    if(a.objectToDisable)
                        a.objectToDisable.SetActive(false);
                    if (a.objectToEnable)
                        a.objectToEnable.SetActive(true);
                    return;
                }
            }
        }
    }
}