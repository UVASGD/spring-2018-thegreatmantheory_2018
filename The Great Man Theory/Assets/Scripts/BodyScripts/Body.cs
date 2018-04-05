using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team { GoodGuys, BadGuys }

public enum UnitType { Pike, Sword, Longsword, Arquebus, HorseSword, HorseArquebus }

public class Body : MonoBehaviour {

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

    public bool horse = false;

    public Sprite[] bodySprites;
    public Sprite[] headSprites;

    public Team team;
    public UnitType unitType;
    public Color bodyColor = Color.black;
    public Color lowerArmColor;
    public Color upperArmColor;

    public Transform deadBody;
    public Transform effectGen;
	public ParticleSystem fx;
	public ParticleSystem.MainModule fxMain;

    public Rigidbody2D rb;
    public float maxHealth = 100f;
    float health;
    public float punctureResist = 8;
    public float threshold = 2;
    //public float thresholdMultiplier = 2;
    public bool greatHittable = true;

    public Weapon weapon;

    public float height; //TODO make this a thing for elevation, and maybe use it in collisions? 
                         //make sure to set weapon's height equal to body height

    bool alreadyDead = false;

    public BodySoundbox soundbox;

    bool paused = false;
    public bool cutsceneOverride = false;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;

        if (!weapon)
            weapon = transform.parent.GetComponentInChildren<Weapon>();

        if (!fx)
		    fx = effectGen.GetComponent<ParticleSystem> ();
		fxMain = fx.main;

        if (!soundbox)
            soundbox = GetComponentInChildren<BodySoundbox>();

        if (!horse) {
            SetColors();
            ApplyColors();
        }
        CheckHealth();
    }

    void Update() {
        if (!paused && GameManager.state != GameState.Gameplay && !cutsceneOverride) {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            paused = true;
        }
        else if (paused && GameManager.state == GameState.Gameplay || cutsceneOverride) {
            rb.constraints = RigidbodyConstraints2D.None;
            paused = false;
        }
        
    }

    public void Hit(float force, Vector2 hitPoint, bool puncturing = false, bool playerHit = false) {
        //Debug.Log(force);
        if (force > threshold) {
            //Debug.Log(force);
            Vector2 spankForce = (hitPoint - (Vector2)transform.position).normalized; //Yes, we are calling it this.
            if (force > (threshold * 4) && playerHit && greatHittable) {
                //Particle effect, push away from hitPoint big, play cheer sound, shake camera
                if (!puncturing)
                    rb.AddForce(spankForce * 50, ForceMode2D.Impulse);
				if (!fx.isPlaying)
					fxMain.duration = 1.5f;
				fx.Play ();
                // Debug.Log("GREAT HIT!");
            }
            else if (force > (threshold * 3)) {
                //Particle effect, play sound large, push away from hitPoint medium
                if (!puncturing)
                    rb.AddForce(spankForce * 20, ForceMode2D.Impulse);
				if (!fx.isPlaying)
					fxMain.duration = 1f;
				fx.Play ();
                // Debug.Log("Large hit");
            }
            else if (force > (threshold * 2)) {
                //Particle effect, play sound medium, push away from hitPoint small
                if (!puncturing)
                    rb.AddForce(spankForce * 10, ForceMode2D.Impulse);
				
				if (!fx.isPlaying)
					fxMain.duration = 0.75f;
				fx.Play ();
                // Debug.Log("Medium hit");
            }
            else {
                //play sound quiet
				if (!fx.isPlaying)
					fxMain.duration = 0.2f;
				fx.Play ();
                // Debug.Log("Small hit");
            }

            Damage(force);
            HitSound(force);
        }

    }

    public void Damage(float force) {
        health -= force;
        CheckHealth();
    }

    void CheckHealth() {
        if (health <= 0) {
            if (deadBody && !alreadyDead) {
                alreadyDead = true;
                Transform dead = Instantiate(deadBody, transform.position, Quaternion.Euler(0, 0, (Vector2.SignedAngle(Vector2.up, rb.velocity))));
                dead.GetComponent<SpriteRenderer>().color = bodyColor;
                dead.GetComponent<Rigidbody2D>().AddForce(rb.velocity*5, ForceMode2D.Impulse);
            }

            /*if (weapon) { //TODO just get Weapon script and call Drop();
                weapon.transform.parent = null;
                Rigidbody2D weaponRB = weapon.GetComponent<Rigidbody2D>();
                weaponRB.velocity = new Vector2(0, 0);
                weaponRB.angularVelocity = 0;
                weapon.layer = LayerMask.NameToLayer("Ground");
            }*/
            DeadSound();
            Destroy(transform.parent.gameObject);
        }
    }

    public bool Wounded() {
        return (health < (maxHealth * 0.1f));
    }

    //COLORING AND SPRITING THE DUDE

    public void SetColors() {
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

    public void ApplyColors() {
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

    void HitSound(float force) {
        float volume = 50f / force;
        soundbox.Hit(volume);
    }

    void DeadSound() {
        soundbox.Death();
    }
}
