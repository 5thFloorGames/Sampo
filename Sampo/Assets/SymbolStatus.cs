using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolStatus : MonoBehaviour {

	private Image symbol;
	public bool done = false;
	public Sprite doneImage;

	void Start(){
		symbol = GetComponent<Image> ();
	}

	public void MarkDone(){
		symbol.sprite = doneImage;
		symbol.color = new Color (symbol.color.r, symbol.color.g, symbol.color.b, 1.0f);
	}
}
