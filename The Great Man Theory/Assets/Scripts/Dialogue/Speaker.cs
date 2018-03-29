using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Speaker", menuName = "Speaker")]
public class Speaker : ScriptableObject {

    public new string name;

    public Language language;

    [Range(-3,3)]
    public float voicePitch = 0f;

    public Sprite portrait;
}
