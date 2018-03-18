using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

    List<Transform> enemies = new List<Transform>();
    Body thisBody;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        Collider2D collider = collision.collider;
        if (collider.CompareTag("Body") || !enemies.Contains(collider.transform)) {
            if (collider.GetComponent<Body>().team != thisBody.team) {
                enemies.Add(collider.transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        enemies.Remove(collision.transform);
        //If it's a friendly, tell em to regroup
    }

    public Transform FindEnemy() {
        if (enemies.Count == 0)
            return null;

        for (int i = 0; i < enemies.Count; i++)
            if (!enemies[i])
                enemies.RemoveAt(i);

        return enemies[Random.Range(0, enemies.Count)];
    }
}
