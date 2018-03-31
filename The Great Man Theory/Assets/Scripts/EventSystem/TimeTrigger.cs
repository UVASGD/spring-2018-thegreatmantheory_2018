using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrigger : EventTrigger {

    public float time;
	
	// Update is called once per frame
	void Update () {
        if (time > 0f)
            time -= Time.deltaTime;
        else if (!hasHappened || !onceOnly) {
            strEvent.Invoke("");
            hasHappened = true;
        }
	}
}
