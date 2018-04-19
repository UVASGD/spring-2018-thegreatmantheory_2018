using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasmScript : MonoBehaviour {

    private Vector3 targetScale = new Vector3(0.1f, 0.1f, 1);

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Body")) {
            collider.GetComponent<Body>().Damage(10000);
        }

        if (collider.CompareTag("DeadBody")) {
            StartCoroutine(Fall(collider));
        }
    }

    private IEnumerator Fall(Collider2D collider) {
        while (collider) {
            collider.transform.localScale = Vector3.Lerp(collider.transform.localScale, targetScale, 0.05f);
            yield return null;
        }
    }
}
