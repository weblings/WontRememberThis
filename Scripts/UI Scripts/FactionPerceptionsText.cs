﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactionPerceptionsText : MonoBehaviour {

	string fp;
	public Text text;
	GameScript game;

	// Use this for initialization
	void Start () {
		game = GameObject.FindWithTag ("GameController").GetComponent<GameScript> ();
	}

	// Update is called once per frame
	void Update () {
		fp = game.h.SDtoString<string,float>(game.factions[0].factionsPerceptions);
		text.text = fp;
	}
}
