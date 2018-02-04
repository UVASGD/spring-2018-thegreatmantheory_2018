using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TRASH THIS SHIT
public class RandomWiggles : BotMovement {

    public FollowPointer pointer;
    GameManager gm;

    // Use this for initialization
    void Start () {
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update () {
        Vector2 randPos = RandPos();
        Vector2 result = pointer.TargetPos + randPos;

        if (result.magnitude > gm.wigglemax) {
            result = pointer.TargetPos.normalized * gm.wigglemax;
        }

        pointer.TargetPos = result;
        pointer.Forces();
	}

    Vector2 RandPos() {
        Vector2 direct = Random.insideUnitCircle;
        float random = Random.Range(0, gm.wigglemax/2);
        return direct * random;
    }
}
