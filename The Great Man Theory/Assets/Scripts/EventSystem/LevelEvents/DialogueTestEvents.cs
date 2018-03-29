using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTestEvents : EventManager {

    public GameObject lighto;

	public IEnumerator CreateLighto() {
        Debug.Log("Fiat Lux!");
        lighto.SetActive(true);
        yield return null;
    }
}
