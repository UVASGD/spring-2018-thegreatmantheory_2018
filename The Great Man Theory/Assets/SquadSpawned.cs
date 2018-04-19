using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadSpawned : MonoBehaviour {

    public SquadSpawner spawner;
    Squad squad;

    void Start() {
        squad = GetComponent<Squad>();
    }

	void OnDestroy() {
        spawner.SquadDeath(squad);
    }
}
