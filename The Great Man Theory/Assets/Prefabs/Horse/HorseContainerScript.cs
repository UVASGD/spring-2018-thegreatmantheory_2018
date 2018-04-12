using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseContainerScript : MonoBehaviour {

    public Transform Horse;
    public Transform Rider;

    private void Awake() {
        Horse.parent = (transform.parent) ? transform.parent : null;
        Rider.parent = (transform.parent) ? transform.parent : null;
        Destroy(gameObject);
    }
}
