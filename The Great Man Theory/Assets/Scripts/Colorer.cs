using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorer : MonoBehaviour {

    [HideInInspector]
    public SpriteRenderer headRender;
    [HideInInspector]
    public SpriteRenderer bodyRender;
    [HideInInspector]
    public SpriteRenderer rUpperArm;
    [HideInInspector]
    public SpriteRenderer rLowerArm;
    [HideInInspector]
    public SpriteRenderer lUpperArm;
    [HideInInspector]
    public SpriteRenderer lLowerArm;

    public Sprite[] bodySprites;
    public Sprite[] headSprites;

    public Team team;
    public UnitType unitType;
    public Color bodyColor = Color.black;
    public Color lowerArmColor;
    public Color upperArmColor;

    // Use this for initialization
    void Start () {
        SetColors();
        ApplyColors();
    }

    void SetColors() {
        switch (team) {
            case (Team.GoodGuys):
                bodyColor = Color.blue;
                upperArmColor = Color.red;
                lowerArmColor = Color.blue;
                break;
            case (Team.BadGuys):
                bodyColor = Color.black;
                upperArmColor = Color.red;
                lowerArmColor = Color.black;
                break;
            default:
                bodyColor = Color.white;
                upperArmColor = Color.white;
                lowerArmColor = Color.black;
                break;
        }
    }

    void ApplyColors() {
        if (bodySprites.Length > 0 && bodyRender)
            bodyRender.sprite = bodySprites[Random.Range(0, bodySprites.Length)];
        if (headSprites.Length > 0 && headRender)
            headRender.sprite = headSprites[Random.Range(0, headSprites.Length)];

        ColorPart(bodyRender, bodyColor);
        ColorPart(rUpperArm, upperArmColor);
        ColorPart(rLowerArm, lowerArmColor);
        ColorPart(lUpperArm, upperArmColor);
        ColorPart(lLowerArm, lowerArmColor);
    }

    void ColorPart(SpriteRenderer part, Color color) {
        if (part)
            part.color = color;
    }
}
