using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Language { English, French, German, Italian, Spanish, Swedish, Czech, Turkish };

[System.Serializable]
public class Sentence {

    public Speaker speaker;

    [TextArea(3, 10)]
    public string text;

    public StrEvent strEvent = null;
    // public Event eventTrigger = null;
    // public DialogueEvent eventName = DialogueEvent.None;

}
