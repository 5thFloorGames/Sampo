using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSpawner : MonoBehaviour {

	public GameObject[] results;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Spawn(Result result){
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
