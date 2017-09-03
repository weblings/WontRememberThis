using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonthText : MonoBehaviour {

	int month;
	public Text text;
	GameScript game;

	// Use this for initialization
	void Start () {
		game = GameObject.FindWithTag ("GameController").GetComponent<GameScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		month = game.month;
		text.text = getMonth(month);
	}

	//Return month in 3 char version corresponding to int given
	public string getMonth(int input){
		string month = "";
		if (input == 1) month = "JAN";
		else if (input == 2) month = "FEB";
		else if (input == 3) month = "MAR";
		else if (input == 4) month = "APR";
		else if (input == 5) month = "MAY";
		else if (input == 6) month = "JUN";
		else if (input == 7) month = "JUL";
		else if (input == 8) month = "AUG";
		else if (input == 9) month = "SEP";
		else if (input == 10) month = "OCT";
		else if (input == 11) month = "NOV";
		else if (input == 12) month = "DEC";

		return month;
	}

}
