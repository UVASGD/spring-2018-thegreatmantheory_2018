using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TRASH THIS SHIT
public class BotMovement : MonoBehaviour {

    public Transform target;

    public SquadScript squad;

    public virtual bool CheckTarget() {
        return (target);
    }
}
