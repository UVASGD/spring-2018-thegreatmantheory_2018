using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue dialogue;
    public bool triggerOnStart = false;

    void Start() {
        //if (triggerOnStart) {
        //    TriggerDialogue();
        //}
    }

    void LateUpdate() {
        if (triggerOnStart) {
            TriggerDialogue();
            triggerOnStart = false;
        }
    }

    public void TriggerDialogue() {
        Debug.Log("Starting dialogue");
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
