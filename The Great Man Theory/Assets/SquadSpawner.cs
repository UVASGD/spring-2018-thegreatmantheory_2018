using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadSpawner : MonoBehaviour {

    public Squad prefab;

    public int maxSquads = 1;
    int activeSquads = 0;

    public float waitTime = 0f;
    float waiting = 0f;

    public SquadType squadType = SquadType.Advance;

    BoxCollider2D spawnArea;

	// Use this for initialization
	void Start () {
        spawnArea = GetComponent<BoxCollider2D>();
        waiting = waitTime;
	}
	
	// Update is called once per frame
	void Update () {
        if (activeSquads < maxSquads && GameManager.state == GameState.Gameplay)
            waiting -= Time.deltaTime;
        if (waiting <= 0f)
            MakeSquad();
	}

    public void SquadDeath() {
        activeSquads--;
    }

    void MakeSquad() {
        Vector3 spawnPosition;
        if (spawnArea) {
            Vector3 min = spawnArea.bounds.min;
            Vector3 max = spawnArea.bounds.max;

            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);

            spawnPosition = new Vector3(x, y, transform.position.z);
        }
        else
            spawnPosition = transform.position;

        Squad newSquad = Instantiate(prefab, spawnPosition, transform.rotation);

        newSquad.squadType = squadType;
        newSquad.SetDefaultBehavior(squadType);

        SquadSpawned spawned = newSquad.GetComponent<SquadSpawned>();
        if (spawned)
            spawned.spawner = this;

        activeSquads++;
        waiting = waitTime;
    }
}
