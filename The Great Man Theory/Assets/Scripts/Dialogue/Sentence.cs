using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Language { English, French, German, Italian, Spanish, Swedish, Turkish };

[System.Serializable]
public class Sentence {

    public Language language = Language.English;
    public string speaker;

    [TextArea(3, 10)]
    public string text;

}
