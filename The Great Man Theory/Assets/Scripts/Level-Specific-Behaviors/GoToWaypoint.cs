using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWaypoint : MonoBehaviour {

    public Transform waypoint;
    public Squad squad;

    void Start() {
        squad = GetComponent<Squad>();
        squad.MoveCommand(waypoint.position);
    }
}
