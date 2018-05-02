using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadSpawner : MonoBehaviour {

    public Squad prefab;

    public int maxSquads = 1;
    List<Squad> activeSquads = new List<Squad>();
    public List<Squad> ActiveSquads { get { return activeSquads; } }
    // int activeSquads = 0;

    bool wasNew = false;
    Squad newestSquad = null;
    public Squad NewestSquad { get { return newestSquad; } }

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
        if (activeSquads.Count < maxSquads && GameManager.state == GameState.Gameplay)
            waiting -= Time.deltaTime;
        if (waiting < 0f)
            MakeSquad();
        if (newestSquad && wasNew) {
            newestSquad = null;
            wasNew = false;
        }
        else if (newestSquad && !wasNew)
            wasNew = true;
    }

    public void SquadDeath(Squad ded) {
        // activeSquads--;
        activeSquads.Remove(ded);
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

        // activeSquads++;
        activeSquads.Add(newSquad);
        newestSquad = newSquad;
        waiting = waitTime;
    }
}
