using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

    DefaultTree maintree;

    List<Transform> enemies = new List<Transform>();
    public BasicBot carrier;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Body") || !enemies.Contains(collider.transform)) {
            if (collider.GetComponent<Body>().team != carrier.body.team) {
                enemies.Add(collider.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        enemies.Remove(collider.transform);
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

    public void SetTree(DefaultTree tree) {
        maintree = tree;
    }
}
