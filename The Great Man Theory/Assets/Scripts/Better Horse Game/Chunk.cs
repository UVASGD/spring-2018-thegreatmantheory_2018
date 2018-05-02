using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {

	public GameObject grasstile;

	public List<GameObject> trees;

	public GameObject Horse;

	public Chunk() {
		trees = new List<GameObject> ();
	}

	public void Unload() {
		GameObject.Destroy (grasstile);
		for (int i = trees.Count - 1; i >= 0; i--) {
			GameObject.Destroy (trees [i]);
		}
		if (Horse && !Horse.GetComponent<FreeHorseAI> ().activated) {
			GameObject.Destroy (Horse);
		}
	}
}
