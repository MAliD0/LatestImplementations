using DS.Graph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSTrigger : MonoBehaviour
{
    [SerializeField] private DialogueGraph dialogue;
    [SerializeField] private string text;

    public void TriggerDialogue()
    {
        if(dialogue == null)
        {
            DSManager.instance.StartDialogue(text);
            return;
        }
        DSManager.instance.StartDialogue(dialogue);
    }
}
