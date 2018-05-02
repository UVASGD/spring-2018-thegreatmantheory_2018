using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeYouOnTheOtherSide : MonoBehaviour {

    public Transform player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // if (player && transform.position.y - player.position.y > 10)
        if (player)
            transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
	}
}
