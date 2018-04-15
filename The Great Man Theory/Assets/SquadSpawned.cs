using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadSpawned : MonoBehaviour {

    public SquadSpawner spawner;

	void OnDestroy() {
        spawner.SquadDeath();
    }
}
