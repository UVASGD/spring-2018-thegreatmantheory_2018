using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadSpawner : MonoBehaviour {

    public Squad prefab;
    List<Squad> activeSquads = new List<Squad>();

    public int maxSquads = 1;
    public float waitTime = 0f;
    float waiting = 0f;

    BoxCollider2D spawnArea;

	// Use this for initialization
	void Start () {
        spawnArea = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (activeSquads.Count < maxSquads)
            waiting -= Time.deltaTime;
        if (waiting <= 0f)
            MakeSquad();
	}

    void MakeSquad() {
        return;
    }
}
