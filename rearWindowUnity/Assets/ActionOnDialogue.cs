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
    }

    public List<Action> actions = new List<Action>();

    public void DialogIsOver(int index)
    {
        foreach(Action a in actions)
        {
            if (a.dialogue == index)
            {
                a.objectToDisable.SetActive(false);
                a.objectToEnable.SetActive(true);
                return;
            }
        }
    }
}