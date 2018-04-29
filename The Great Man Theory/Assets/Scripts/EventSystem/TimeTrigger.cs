using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrigger : EventTrigger {

    public float time;
    public bool active = true;
	
	// Update is called once per frame
	protected override void Update () {
        // base.Update();
        if (active && time > 0f)
            time -= Time.deltaTime;
        else if (active && !(triggered && onceOnly)) {
            strEvent.Invoke("");
            triggered = true;
        }
	}
}
