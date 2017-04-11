using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSpawner : MonoBehaviour {

	public GameObject[] results;
	public SymbolStatus[] symbols;
	private Animator animator;
	private AudioSource whoosh;

	void Start(){
		animator = GetComponent<Animator> ();
		whoosh = GetComponent<AudioSource> ();
	}

	public void Spawn(Result result){
		if (result == Result.Failure) {
			results [(int)result].SetActive (true);
		} else {
			symbols [(int)result].MarkDone ();
			results [(int)result].SetActive (true);
		}
		animator.SetTrigger ("Spawn");
	}

	public void Reset(){
		StartCoroutine (WaitAndReset ());
	}

	IEnumerator WaitAndReset(){
		animator.SetTrigger ("Hide");
		whoosh.Play ();

		yield return new WaitForSeconds (1.0f);
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
