using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvgPerception : MonoBehaviour {

	float avg;
	public Text text;
	GameScript game;

	// Use this for initialization
	void Start () {
		game = GameObject.FindWithTag ("GameController").GetComponent<GameScript> ();
	}

	// Update is called once per frame
	void Update () {
		avg = game.factions[0].AvgPerception;
		text.text = avg.ToString();
	}
}
