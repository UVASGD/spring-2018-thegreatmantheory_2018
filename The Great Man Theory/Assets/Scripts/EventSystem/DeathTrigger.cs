using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : EventTrigger {

    public bool dewit = true;

	void OnDestroy() {
        if  (dewit && GameManager.state != GameState.SceneTransition)
            strEvent.Invoke("");
    }
}
