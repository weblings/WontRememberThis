using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour{

	public List<Faction> factions;
	public Timeline t;
	SortedDictionary<string,City> cities;

	// Use this for initialization
	void Start () {

		//basically a constructor
		t = RandomTimeline (5);
		factions = new List<Faction> ();
		cities = new SortedDictionary<string, City> ();

		//initilizing some testing values
		for (int i = 0; i < 5; i++) {
			string factionName = "The " + RandomString (Random.Range (5, 8));
			string newCityName = RandomString (Random.Range (5, 13));
			City NewCity = new City (newCityName, factionName, .5f, 150);
			cities.Add (newCityName, NewCity);
			Faction f = new Faction(factionName,newCityName, t);
			factions.Add (f);
		}
		for (int i = 0; i < factions.Count; i++) {
			print(factions [i].toString ());
		}
	}

	//Real functions ----------------------------------------------------------------------------------------------


	//Randomizer functions ---------------------------------------------------------------------------------------
	public Event RandomEvent(){
		int partsMax = Random.Range (2, 5);
		List<int> parts = new List<int>();
		List<int> cons = new List<int>();
		for (int i = 0; i < partsMax; i++) {
			parts.Add (i);
			cons.Add(Random.Range(-3,3));
		}
		List<int> date = new List<int> (new int[]{ Random.Range (0, 1500), Random.Range (1, 12), Random.Range (1, 30) });
		string name = "Battle of the " + RandomString (Random.Range (3, 8));
		int importance = Random.Range (0, 5);
		int quality = Random.Range (0, 4);
		Event rando = new Event (parts, date, cons, name, importance, quality);
		return rando;
	}

	public Timeline RandomTimeline(int length){
		Timeline t = new Timeline ();
		for (int i = 0; i < length; i++) {
			t.timeline.Add (RandomEvent ());
		}
		return t;
	}

	//returns randomized string of that length
	public string RandomString(int length){
		string output = "";
		char[] upper = {'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
		char[] alphabet = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
		for (int i = 0; i < length; i++) {
			int index = Random.Range (0, 26);
			if (i == 0)	output += upper [index];
			else output += alphabet [index];
		}
		return output;
	}
}
