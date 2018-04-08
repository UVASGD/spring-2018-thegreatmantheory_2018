using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Language { English, French, German, Italian, Spanish, Swedish, Czech, Turkish };

[System.Serializable]
public class Sentence {

    public Speaker speaker;

    [TextArea(3, 10)]
    public string text;

    /// <summary>
    /// If true, then the dialogue will move on to the next sentence
    /// without prompting.
    /// </summary>
    public bool runOn = false;

    /// <summary>
    /// This indicates the amout of time waited before the next sentence
    /// is displayed automatically. Only activates if 'runOn' is true.
    /// </summary>
    public float runWait = 0f;

    public StrEvent strEvent = null;

    // public Event eventTrigger = null;
    // public DialogueEvent eventName = DialogueEvent.None;

}
