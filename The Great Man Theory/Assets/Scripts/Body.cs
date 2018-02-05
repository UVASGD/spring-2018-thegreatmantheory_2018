using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour {

    public enum TeamColor { GoodGuys, BadGuys }

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

    public TeamColor teamColor;
    public Color bodyColor = Color.black;
    public Color lowerArmColor;
    public Color upperArmColor;

    public Transform deadBody;
    public Transform effectGen;

    public Rigidbody2D rb;
    public float health = 100f;
    public float punctureResist = 8;
    public float threshold = 2;
    //public float thresholdMultiplier = 2;
    public bool greatHittable = true;

    public GameObject weapon;

    public float height; //TODO make this a thing for elevation, and maybe use it in collisions? 
                         //make sure to set weapon's height equal to body height

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        SetColors();
        ApplyColors();
        CheckHealth();
    }

    public void Hit(float force, Vector2 hitPoint, bool puncturing = false, bool playerHit = false) {
        //Debug.Log(force);
        if (force > threshold) {
            Debug.Log(force);
            Vector2 spankForce = (hitPoint - (Vector2)transform.position).normalized; //Yes, we are calling it this.
            if (force > (threshold * 4) && playerHit && greatHittable) {
                //Particle effect, push away from hitPoint big, play cheer sound, shake camera
                if (!puncturing)
                    rb.AddRelativeForce(spankForce * 500, ForceMode2D.Impulse);
                Debug.Log("GREAT HIT!");
            }
            else if (force > (threshold * 3)) {
                //Particle effect, play sound large, push away from hitPoint medium
                if (!puncturing)
                    rb.AddRelativeForce(spankForce * 100, ForceMode2D.Impulse);
                Debug.Log("Large hit");
            }
            else if (force > (threshold * 2)) {
                //Particle effect, play sound medium, push away from hitPoint small
                if (!puncturing)
                    rb.AddRelativeForce(spankForce * 20, ForceMode2D.Impulse);
                Debug.Log("Medium hit");
            }
            else {
                //play sound quiet
                Debug.Log("Small hit");
            }

            Damage(force);
        }

    }

    public void Damage(float force) {
        health -= force;
        CheckHealth();
    }

    void CheckHealth() {
        if (health <= 0) {
            if (deadBody) {
                Transform dead = Instantiate(deadBody, transform.position, Quaternion.Euler(0, 0, (Vector2.SignedAngle(Vector2.up, rb.velocity))));
                dead.GetComponent<SpriteRenderer>().color = bodyColor;
            }

            if (weapon) {
                weapon.transform.parent = null;
                Rigidbody2D weaponRB = weapon.GetComponent<Rigidbody2D>();
                weaponRB.velocity = new Vector2(0, 0);
                weaponRB.angularVelocity = 0;
                weapon.layer = LayerMask.NameToLayer("Ground");
            }
            Destroy(transform.parent.gameObject);
        }
    }


    //COLORING AND SPRITING THE DUDE

    void SetColors() {
        switch (teamColor) {
            case (TeamColor.GoodGuys):
                bodyColor = Color.blue;
                upperArmColor = Color.red;
                lowerArmColor = Color.blue;
                break;
            case (TeamColor.BadGuys):
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
