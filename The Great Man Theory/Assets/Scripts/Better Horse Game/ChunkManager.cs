using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChunkManager : MonoBehaviour {

	public List<GameObject> grassTile;

	public float tilex, tiley;

	private Vector3 previousPosition;

	public Transform playerTransform;

	public int averageNumberOfTrees;

	private Chunk N, NE, E, SE, S, SW, W, NW, C;

	public GameObject tree;

	public GameObject horse;

	public Text credits;

	public List<string> accreditations;

	private int index = -1;

	private float timer;

	public float creditsdelay;

	// Use this for initialization
	void Start () {
		if (playerTransform == null) {
			Destroy (this);
		}
		previousPosition = playerTransform.position;

		NW = GenChunk (-1,1);
		N = GenChunk (0,1);
		NE = GenChunk (1,1);
		W = GenChunk (-1, 0);
		C = GenChunk (0,0);
		E = GenChunk (1,0);
		SW = GenChunk (-1,-1);
		S = GenChunk (0,-1);
		SE = GenChunk (1,-1);
		 
		index = -1;

		timer = 0;


		credits.text = "Credits";
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;

		int previousChunkx = Mathf.FloorToInt(previousPosition.x / tilex);
		int previousChunky = Mathf.FloorToInt(previousPosition.y / tiley);

		int currentChunkx = Mathf.FloorToInt(playerTransform.position.x / tilex);
		int currentChunky = Mathf.FloorToInt(playerTransform.position.y / tiley);

		previousPosition = playerTransform.position;

		if (previousChunkx == currentChunkx && previousChunky == currentChunky) { // C --> C
			return;
		}

		if (previousChunkx + 1 == currentChunkx && previousChunky + 1 == currentChunky) { // C --> NE
			W.Unload ();
			SW.Unload ();
			S.Unload ();
			NW.Unload ();
			SE.Unload ();
			W = N;
			SW = C;
			S = E;
			C = NE;
			NW = GenChunk (currentChunkx - 1, currentChunky + 1);
			N = GenChunk (currentChunkx, currentChunky + 1);
			NE = GenChunk (currentChunkx + 1, currentChunky + 1);
			E = GenChunk (currentChunkx + 1, currentChunky);
			SE = GenChunk (currentChunkx + 1, currentChunky - 1);
		}

		if (previousChunkx == currentChunkx && previousChunky + 1 == currentChunky) { // C --> N
			SW.Unload ();
			S.Unload ();
			SE.Unload ();
			SW = W;
			S = C;
			SE = E;
			W = NW;
			C = N;
			E = NE;
			NW = GenChunk (currentChunkx - 1, currentChunky + 1);
			N = GenChunk (currentChunkx, currentChunky + 1);
			NE = GenChunk (currentChunkx + 1, currentChunky + 1);
		}

		if (previousChunkx - 1 == currentChunkx && previousChunky + 1 == currentChunky) { // C --> NW
			S.Unload ();
			SE.Unload ();
			E.Unload ();
			NE.Unload ();
			SW.Unload ();
			E = N;
			SE = C;
			S = W;
			C = NW;
			NW = GenChunk (currentChunkx - 1, currentChunky + 1);
			N = GenChunk (currentChunkx, currentChunky + 1);
			NE = GenChunk (currentChunkx + 1, currentChunky + 1);
			W = GenChunk (currentChunkx - 1, currentChunky);
			SW = GenChunk (currentChunkx - 1, currentChunky - 1);
		}

		if (previousChunkx + 1 == currentChunkx && previousChunky == currentChunky) { // C --> E
			NW.Unload ();
			W.Unload ();
			SW.Unload ();
			NW = N;
			W = C;
			SW = S;
			N = NE;
			C = E;
			S = SE;
			NE = GenChunk (currentChunkx + 1, currentChunky + 1);
			E = GenChunk (currentChunkx + 1, currentChunky);
			SE = GenChunk (currentChunkx + 1, currentChunky - 1);
		}

		if (previousChunkx - 1 == currentChunkx && previousChunky == currentChunky) { // C --> W
			NE.Unload();
			E.Unload ();
			SE.Unload ();
			NE = N;
			E = C;
			SE = S;
			N = NW;
			C = W;
			S = SW;
			NW = GenChunk (currentChunkx - 1, currentChunky + 1);
			W = GenChunk (currentChunkx - 1, currentChunky);
			SW = GenChunk (currentChunkx - 1, currentChunky - 1);
		}

		if (previousChunkx + 1 == currentChunkx && previousChunky - 1 == currentChunky) { // C --> SE
			W.Unload();
			NW.Unload ();
			N.Unload ();
			NE.Unload ();
			SW.Unload ();
			NW = C;
			N = E;
			C = SE;
			W = S;
			NE = GenChunk (currentChunkx + 1, currentChunky + 1);
			E = GenChunk (currentChunkx + 1, currentChunky);
			SE = GenChunk (currentChunkx + 1, currentChunky - 1);
			S = GenChunk (currentChunkx, currentChunky - 1);
			SW = GenChunk (currentChunkx - 1, currentChunky - 1);
		}

		if (previousChunkx == currentChunkx && previousChunky - 1 == currentChunky) { // C --> S
			NW.Unload();
			N.Unload ();
			NE.Unload ();
			NW = W;
			N = C;
			NE = E;
			W = SW;
			C = S;
			E = SE;
			SW = GenChunk (currentChunkx - 1, currentChunky - 1);
			S = GenChunk (currentChunkx, currentChunky - 1);
			SE = GenChunk (currentChunkx + 1, currentChunky - 1);
		}

		if (previousChunkx - 1 == currentChunkx && previousChunky - 1 == currentChunky) { // C --> SW
			NW.Unload();
			N.Unload ();
			NE.Unload ();
			E.Unload ();
			SE.Unload ();
			N = W;
			NE = C;
			E = S;
			C = SW;
			W = GenChunk (currentChunkx - 1, currentChunky);
			SW = GenChunk (currentChunkx - 1, currentChunky - 1);
			S = GenChunk (currentChunkx, currentChunky - 1);
			NW = GenChunk (currentChunkx - 1, currentChunky + 1);
			SE = GenChunk (currentChunkx + 1, currentChunky - 1);
		}

		if (C.Horse) {
			C.Horse.GetComponent<FreeHorseAI> ().Activate (playerTransform);
			ShowNextCredits ();
		}
	}

	Chunk GenChunk(int x, int y) {
		Chunk boy = new Chunk ();
		GameObject newgrass = Instantiate(grassTile[Random.Range(0,grassTile.Count)]) as GameObject;
		newgrass.transform.position = new Vector2 (x * tilex + (0.5f * tilex), y * tiley + (0.5f * tiley));
		boy.grasstile = newgrass;
		int numtrees = 0;
		for (int i = 0; i < 4; i++) {
			numtrees += Random.Range (0, averageNumberOfTrees / 4 + 1);
		}
		for (int i = 0; i < numtrees; i++) {
			GameObject newtree = Instantiate (tree) as GameObject;
			newtree.transform.position = new Vector2 (x * tilex + Random.Range (0, tilex), y * tiley + Random.Range (0, tiley));
			newtree.transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, Random.Range (0, 360)));
			boy.trees.Add (newtree);
		}
		if (Random.value < 0.5f) {
			GameObject newHorse = Instantiate (horse) as GameObject;
			newHorse.transform.position = new Vector2 (x * tilex + Random.Range (0, tilex), y * tiley + Random.Range (0, tiley));
			newHorse.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, Random.Range (0, 360)));
			boy.Horse = newHorse;
		}

		return boy;
	}

	void ShowNextCredits() {
		if (timer > 0) {
			return;
		}
		index++;
		if (index < accreditations.Count) {
			credits.text = accreditations [index];
		} else {
			credits.text = "Fin.";
		}
		timer = creditsdelay;
	}
}
