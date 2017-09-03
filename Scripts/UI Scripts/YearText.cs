using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YearText : MonoBehaviour {

	int year;
	public Text text;
	GameScript game;

	// Use this for initialization
	void Start () {
		game = GameObject.FindWithTag ("GameController").GetComponent<GameScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		year = game.year;
		text.text = year.ToString();
	}

}
