using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Ingredients : MonoBehaviour {

	public Button milk;
	public Button barley;
	public Button feather;
	public Button wool;
	public Button failures;
	public Button smith;
	private AudioClip[] milkA;
	private AudioClip[] milkB;
	private AudioClip[] barleyA;
	private AudioClip[] barleyB;
	private AudioClip[] woolA;
	private AudioClip[] woolB;
	private AudioClip[] featherA;
	private AudioClip[] featherB;
	private AudioClip[] smithing;
	private Dictionary<Ingredient, AudioClip[][]> ingredientToSound;
	private Dictionary<Ingredient, AudioClip[]> ingredientToMusic;
	private int failuresCount = 0;
	private int verseIndex = -1;
	private Queue<Ingredient> ingredientQueue;
	private List<Ingredient> ingredientSet;
	private PointerEventData pointer = new PointerEventData(EventSystem.current);
	private Ingredient[] correctOrder = {Ingredient.Feather, Ingredient.Milk, Ingredient.Barley, Ingredient.Wool, Ingredient.Failures};
	private AudioClip[] musicSmithing;
	private AudioClip[] musicBarley;
	private AudioClip[] musicWool;
	private AudioClip[] musicMilk;
	private AudioClip[] musicFeather;
	private GameObject smithEFX;
	private AudioSource sound;
	private bool rhythm = true;
	private bool TimerOn = false;
	private float timer;
	private bool[] progress = {false, false, false, false};
	private ResultSpawner results;

	// Use this for initialization
	void Start () {
		ingredientQueue = new Queue<Ingredient> ();
		ingredientSet = new List<Ingredient> ();
		results = FindObjectOfType<ResultSpawner> ();
		smithEFX = GameObject.Find ("FireComplex");
		milkA = Resources.LoadAll<AudioClip>("Audio/Ingredients/Milk/A");
		milkB = Resources.LoadAll<AudioClip>("Audio/Ingredients/Milk/B");
		barleyA = Resources.LoadAll<AudioClip>("Audio/Ingredients/Barley/A");
		barleyB = Resources.LoadAll<AudioClip>("Audio/Ingredients/Barley/B");
		woolA = Resources.LoadAll<AudioClip>("Audio/Ingredients/Wool/A");
		woolB = Resources.LoadAll<AudioClip>("Audio/Ingredients/Wool/B");
		featherA = Resources.LoadAll<AudioClip>("Audio/Ingredients/Feather/A");
		featherB = Resources.LoadAll<AudioClip>("Audio/Ingredients/Feather/B");
		musicSmithing = Resources.LoadAll<AudioClip> ("Audio/Music/Smithing");
		musicBarley = Resources.LoadAll<AudioClip> ("Audio/Music/Barley");
		musicWool = Resources.LoadAll<AudioClip> ("Audio/Music/Wool");
		musicMilk = Resources.LoadAll<AudioClip> ("Audio/Music/Milk");
		musicFeather = Resources.LoadAll<AudioClip> ("Audio/Music/Feather");

		smithing = Resources.LoadAll<AudioClip> ("Audio/Smithing");
		sound = GetComponent<AudioSource> ();
		ingredientToSound = new Dictionary<Ingredient, AudioClip[][]> ();
		ingredientToSound.Add(Ingredient.Milk, new AudioClip[][]{milkA, milkB});
		ingredientToSound.Add(Ingredient.Barley, new AudioClip[][]{barleyA, barleyB});
		ingredientToSound.Add(Ingredient.Feather, new AudioClip[][]{featherA, featherB});
		ingredientToSound.Add(Ingredient.Wool, new AudioClip[][]{woolA, woolB});

		ingredientToMusic = new Dictionary<Ingredient, AudioClip[]> ();
		ingredientToMusic.Add (Ingredient.Milk, musicMilk);
		ingredientToMusic.Add (Ingredient.Barley, musicBarley);
		ingredientToMusic.Add (Ingredient.Feather, musicFeather);
		ingredientToMusic.Add (Ingredient.Wool, musicWool);
	}

	void PlaySound(Ingredient ingredient){
		AudioClip[] clips = ingredientToSound [ingredient][verseIndex];

		sound.PlayOneShot (clips [Random.Range(0, clips.Length)]);
		sound.PlayOneShot (ingredientToMusic[ingredient][verseIndex], 0.1f);
	}

	IEnumerator PlaySounds(AudioClip[] clips){
		int index = 0;
		sound.clip = clips[0];
		sound.PlayOneShot (musicSmithing [index % 2], 0.1f);
		sound.Play ();
		while (index < clips.Length) {
			if (!sound.isPlaying) {
				index++;
				if (index == clips.Length) {
					break;
				}
				sound.clip = clips [index];
				sound.PlayOneShot (musicSmithing [index % 2], 0.1f);
				sound.Play ();
			}
			yield return new WaitForSeconds (0.25f);
		}
		results.Reset ();
	}
		
	void PlayEFX(){
		smithEFX.GetComponent<Animator> ().Play ("FireComplexBurst");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			AddIngredient (Ingredient.Milk);
			ExecuteEvents.Execute(milk.gameObject, pointer, ExecuteEvents.submitHandler);
		} else if (Input.GetKeyDown (KeyCode.W)) {
			AddIngredient (Ingredient.Barley);
			ExecuteEvents.Execute(barley.gameObject, pointer, ExecuteEvents.submitHandler);
		} else if (Input.GetKeyDown (KeyCode.E)) {
			AddIngredient (Ingredient.Feather);
			ExecuteEvents.Execute(feather.gameObject, pointer, ExecuteEvents.submitHandler);
		} else if (Input.GetKeyDown (KeyCode.R)) {
			AddIngredient (Ingredient.Wool);
			ExecuteEvents.Execute(wool.gameObject, pointer, ExecuteEvents.submitHandler);
		} else if (Input.GetKeyDown (KeyCode.T)) {
			AddIngredient (Ingredient.Failures);
		} else if (Input.GetKeyDown (KeyCode.Space)) {
			StartCoroutine(PlaySounds (smithing));
			CheckIngredients ();
			PlayEFX ();
			ExecuteEvents.Execute(smith.gameObject, pointer, ExecuteEvents.submitHandler);
		}
		if (TimerOn) {
			timer += Time.deltaTime;
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	void AddIngredient (Ingredient ingredient){
		// check timer to keep rhytm
		if (!TimerOn) {
			TimerOn = true;
			rhythm = true;
			timer = 0;
		} else {
			print (timer);
			if (timer < 4.8f || timer > 5.2f) {
				rhythm = false;
				print ("RHYTHM MISSED");
			} else {
				print ("Timing good");
			}
			timer = 0;
		}

		verseIndex = ((verseIndex + 1) % 2);
		PlaySound (ingredient);
		if(!ingredientSet.Contains(ingredient)){
			ingredientSet.Add(ingredient);
		}
		ingredientQueue.Enqueue (ingredient);
	}

	void CheckIngredients (){
		Queue<Ingredient> tempIngredientQueue = ingredientQueue;
		//print ("Queue before checks: " + ingredientQueue.Count);

		if (rhythm && !(timer < 4.8f || timer > 5.2f)) {
			print (timer);
			print ("Rhythm correct");
		} else {
			print (timer);
			print ("Rhythm missed");
		}
		timer = 0;
		TimerOn = false;

		if (CheckOrder ()) {
			print ("Correct order");
		} else {
			print ("Incorrect order");
		}

		if (AllIngredients ()) {
			print ("All ingredients");
		} else {
			print ("Something missing");
		}

		if (CheckRepeats ()) {
			results.Spawn (Result.Bow);
			print ("Everything twice");
		} else {
			print ("Something not twice");
		}

		ingredientSet.Clear ();
		ingredientQueue.Clear ();
	}

	bool CheckOrder(){
		for (int i = 0; i < ingredientSet.Count; i++) {
			if (ingredientSet [i] != correctOrder [i]) {
				return false;
			}
		}
		if (ingredientSet.Count > 3) {
			return true;
		} else {
			return false;
		}
	}

	bool AllIngredients () {
		if (ingredientQueue.Contains (Ingredient.Milk)
			&& ingredientQueue.Contains (Ingredient.Wool)
			&& ingredientQueue.Contains (Ingredient.Barley)
			&& ingredientQueue.Contains (Ingredient.Feather)) {
			if (failuresCount > 3) {
				if (ingredientQueue.Contains (Ingredient.Failures)) {
					return true;
				} else {
					return false;
				}
			}
			return true;
		} else {
			return false;
		}
	}

	bool CheckRepeats(){
		while (ingredientQueue.Count > 0) {
			if (ingredientQueue.Count == 1) {
				return false;
			}
			if (ingredientQueue.Dequeue () != ingredientQueue.Dequeue ()) {
				return false;
			}
		}
		return true;
	}
}
