using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour {

    public Squad squad;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E)) {
            squad.TargetCommand(gameObject, timeLeft: 5, priority: 1);
        }
	}
}
