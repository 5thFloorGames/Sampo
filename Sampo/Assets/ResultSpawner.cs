using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSpawner : MonoBehaviour {

	public GameObject[] results;
	public SymbolStatus[] symbols;

	public void Spawn(Result result){
		symbols [(int)result].MarkDone ();
		results [(int)result].SetActive(true);
	}

	public void Reset(){
		foreach (GameObject g in results) {
			if (g != null) {
				g.SetActive (false);
			}
		}
	}
}
