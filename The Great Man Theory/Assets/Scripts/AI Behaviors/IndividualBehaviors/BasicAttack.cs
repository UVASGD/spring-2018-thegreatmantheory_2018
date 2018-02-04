using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TRASH THIS SHIT
public class BasicAttack : BotMovement {

    //public Transform target; 
    //public SquadScript squad; BotMovement initializes these

    private Rigidbody2D rb;
    public FollowPointer pointer; //SET IN EDITOR

    bool attacking = true;

    public int maxWalkingSpeed = 20;
    public int walkSpeed = 1; //Multiply moveForce by this when walking SET IN EDITOR
    public int attackSpeed = 1; //Multiply moveForce by this when attacking, 100 for pike, 50 for longsword, 20 for shortsword SET IN EDITOR
    private int walkDistance = 10;

    private Vector2 attackSpot = new Vector2(0, 0); //This is the random position the rigidbody will ZOOM towards
    private Vector2 power = new Vector2(0, 0); //This is the vector applied to pointer

    public float maxTimer;
    private float moveTimer;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        moveTimer = maxTimer;
        if (target) {
            attackSpot = target.position;
        }
        squad = transform.parent.parent.GetComponent<SquadScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (target) { Attack(); }
        else if (squad){ squad.FindTarget(this); }
	}

    public void Attack() {
        if (CheckTarget() && attacking) {
            float distance = (Vector2.Distance(transform.position, attackSpot));
            bool walking = (distance > walkDistance);
            moveTimer -= Time.deltaTime;

            if ((walking && rb.velocity.magnitude < maxWalkingSpeed) || moveTimer <= 0) {
                Move();
            }

            if (moveTimer <= 0) {
                CheckMove(walking, distance);
                moveTimer = maxTimer;
            }
        }
        else { attacking = false; }
    }

    public override bool CheckTarget() {
        if (!target.gameObject.activeSelf) { return squad.FindTarget(this); };
        return true;
    }

    void CheckMove(bool walking, float distance) {
        float multiplier = 1;
        float randoMultiplier = 1;
        if (walking) {
            multiplier = walkSpeed;
            attackSpot = target.position;
            maxTimer = 2;
        }
        else {
            multiplier = attackSpeed;
            randoMultiplier = (walkDistance - distance);
            attackSpot = (Vector2)target.position + (Random.insideUnitCircle * randoMultiplier);
            maxTimer = (distance/2) * 0.1f;
            //Mathf.Clamp(maxTimer, 1, 5);
        }

        power = (attackSpot - (Vector2)transform.position).normalized;
        power *= multiplier * randoMultiplier;

    }

    void Move() {
        pointer.TargetPos = power;
        pointer.Forces();
    }

}
