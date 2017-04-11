using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSpawner : MonoBehaviour {

	public GameObject[] results;
	public SymbolStatus[] symbols;

	public void Spawn(Result result){
		if (result == Result.Failure) {
			print ("Failure");
		} else {
			symbols [(int)result].MarkDone ();
			results [(int)result].SetActive (true);
		}
	}

	public void Reset(){
		foreach (GameObject g in results) {
			if (g != null) {
				g.SetActive (false);
			}
		}
	}

	public bool CheckSymbolStatus(Result result){
		return symbols [(int)result].done;
	}
}
