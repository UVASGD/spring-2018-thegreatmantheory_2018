using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue dialogue;
    public bool triggerOnStart = false;

    void Start() {
        if (triggerOnStart) {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue() {
        Debug.Log("Starting dialogue");
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
