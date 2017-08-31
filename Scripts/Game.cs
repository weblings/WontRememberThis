using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour{

	public List<Faction> factions;
	public Timeline t;
	SortedDictionary<string,City> cities;
	public int year; //current year

	// Use this for initialization
	void Start () {

		//basically a constructor
		t = RandomTimeline (5);
		factions = new List<Faction> ();
		cities = new SortedDictionary<string, City> ();
		year = 0;

		//initilizing some testing values
		for (int i = 0; i < 5; i++) {
			string factionName = "The " + RandomString (Random.Range (5, 8));
			string newCityName = RandomString (Random.Range (5, 13));
			City NewCity = new City (newCityName, factionName, .5f, 150);
			cities.Add (newCityName, NewCity);
			Timeline yourT = t;
			mutate (yourT);
			print (t.toString());
			print (yourT.toString());
			Faction f = new Faction(factionName,newCityName, yourT);
			factions.Add (f);
		}
		for (int i = 0; i < factions.Count; i++) {
			//print(factions [i].toString ());
		}
	}

	//Real functions ----------------------------------------------------------------------------------------------


	//Randomizer functions ---------------------------------------------------------------------------------------
	public Event RandomEvent(){
		int partsMax = Random.Range (2, 5);
		List<string> parts = new List<string>();
		List<int> cons = new List<int>();
		for (int i = 0; i < partsMax; i++) {
			parts.Add (RandomString(9));
			cons.Add(Random.Range(-3,3));
		}
		List<int> date = new List<int> (new int[]{ Random.Range (0, 1500), Random.Range (1, 12), Random.Range (1, 30) });
		string name = "Battle of the " + RandomString (Random.Range (3, 8));
		int importance = Random.Range (0, 5);
		//int numCopies = Random.Range (0, 4);
		Event rando = new Event (parts, date, cons, name, importance); //, numCopies);
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
		
	//Mutator functions ------------------------------------------------------------------------------------------------------

	//have a second one that attaches identity of deliberate changes
	//starting with pure random
	//Add change storing
	public void mutate(Timeline t){
		int index = Random.Range (0, t.timeline.Count-1);
		int data;

		//determining which data to mutate
		if (Random.Range(0,4) > t.timeline[index].records.Count){
			data = Random.Range (0, 5);
			//print (index + ", " + data);
		} else { //the data's recording has protected it
			return;
		}

		if (data == 0) { //participants

			int change = Random.Range(0, t.timeline[index].participants.Count-1);
			//Change participants to a random faction
			t.timeline [index].participants [change] = factions[Random.Range (0, factions.Count-1)].name;
		
		} else if (data == 1) { //date
			int change = Random.Range(0,100);

			if (change <= 36) { //0-36 are days
				change *= (5 / 6); //lower it to within 0-30
				t.timeline [index].date [2] = change;
			} else if (change >= 37 && change <= 72) { //37-72 are months
				change /= 3; //lower it to 1-12
				t.timeline [index].date [1] = change;
			} else if(change >= 73 && change <= 86){ //73-86 subtracts from years
				change -= 72; //make change between 0-14
				change *= -2; //make change between -28 and 0
				if (t.timeline [index].date [0] + change < 0) {
					t.timeline [index].date [0] = 0;
				} else {	
					t.timeline [index].date [0] += change;
				}
			} else { //87-100
				change -= 86; //makes change between 0-14
				change *= 2; //makes change between 0-28
				if (t.timeline [index].date [0] + change > year) {
					t.timeline [index].date [0] = year;
				} else {
					t.timeline [index].date [0] += change;
				}
			}
				
		} else if (data == 2) { //consequences
			
			int length = t.timeline[index].consequences.Count;
			int change = Random.Range (0, length);
			int amount = 1;
			if(Random.Range(0,1) == 1){
				amount = -1;
			}
			t.timeline [index].consequences [change] += amount;
		
		} else if (data == 3) { //name

			//Needs last word to be the one it will edit
			int spaceIndex = t.timeline[index].name.LastIndexOf(' ');
			string newName = t.timeline [index].name.Substring(0,spaceIndex);
			newName += RandomString (Random.Range (5, 10));

		} else if (data == 4) { //importance

			int amount = 1;
			if(Random.Range(0,1) == 1){
				amount = -1;
			}
			t.timeline [index].importance += amount;

		} else if (data == 5) { //recorded
			t.timeline[index].removeRandomRecording();

		}

	}
}
