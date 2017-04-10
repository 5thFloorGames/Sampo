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
	private int failuresCount = 0;
	private Queue<Ingredient> ingredientQueue;
	private PointerEventData pointer = new PointerEventData(EventSystem.current);
	public GameObject smithEFX;

	// Use this for initialization
	void Start () {
		ingredientQueue = new Queue<Ingredient> ();
		smithEFX = GameObject.Find ("FireComplex");
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
			CheckIngredients ();
			PlayEFX ();
			ExecuteEvents.Execute(smith.gameObject, pointer, ExecuteEvents.submitHandler);
		}
	}
		
	void AddIngredient (Ingredient ingredient){
		ingredientQueue.Enqueue (ingredient);
	}

	void CheckIngredients (){
		Queue<Ingredient> tempIngredientQueue = ingredientQueue;
		//print ("Queue before checks: " + ingredientQueue.Count);

		if (AllIngredients ()) {
			print ("All ingredients");
		} else {
			print ("Something missing");
		}

		if (CheckRepeats ()) {
			print ("Everything twice");
		} else {
			print ("Something not twice");
		}
		//print ("Queue after schecks: " + ingredientQueue.Count);

		ingredientQueue.Clear ();
	}

	void PlayEFX(){
		smithEFX.GetComponent<Animator> ().Play ("FireComplexBurst");
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
			if (ingredientQueue.Dequeue () != ingredientQueue.Dequeue () || ingredientQueue.Count == 1) {
				return false;
			}
		}
		return true;
	}
}
