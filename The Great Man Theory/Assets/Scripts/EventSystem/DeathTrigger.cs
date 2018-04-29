using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : EventTrigger {

	void OnDestroy() {
        if  (GameManager.state != GameState.SceneTransition)
        strEvent.Invoke("");
    }
}
