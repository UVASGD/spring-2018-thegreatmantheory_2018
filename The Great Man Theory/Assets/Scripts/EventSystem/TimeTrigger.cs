using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrigger : EventTrigger {

    public float time;
    public bool active = true;
	
	// Update is called once per frame
	void Update () {
        if (active && time > 0f)
            time -= Time.deltaTime;
        else if (active && !(hasHappened && onceOnly)) {
            strEvent.Invoke("");
            hasHappened = true;
        }
	}
}
