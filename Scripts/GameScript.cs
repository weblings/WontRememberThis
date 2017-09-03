using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour{

	public List<Faction> factions;
	public List<string> factionNames; //probably will get rid of this after testing
	//public Timeline t;
	SortedDictionary<string,City> cities;
	public int year; //current year
	public int month;

	// Use this for initialization
	void Start () {
		month = 0;
		year = 0;

		//Initializing faction names so randomized timelines have real factions
		for (int i = 0; i < 5; i++) {
			string factionName = "The " + RandomString (Random.Range (5, 8));
			factionNames.Add (factionName);
		}

		//basically a constructor
		Timeline t = RandomTimeline (5);
		factions = new List<Faction> ();
		cities = new SortedDictionary<string, City> ();

		//initilizing some testing values
		for (int i = 0; i < 5; i++) {
			string factionName = factionNames [i];
			string newCityName = RandomString (Random.Range (5, 13));
			City NewCity = new City (newCityName, factionName, .5f, 150);
			cities.Add (newCityName, NewCity);
			Faction f = new Faction(factionName,newCityName,t);
			factions.Add (f);
		}
		InvokeRepeating ("Time", 0f, 0.5f); //2f might be good for normal gamerate
	}


	void Time(){

		//handle months
		if (month < 12)	month++;
		else month = 1;

		//possibility of a player's timeline getting hit each month
		if (Random.Range (0, 4) == 0) {
			int index = Random.Range (0, factions.Count);
			mutate (factions [index].t.timeline);
			print (factions [index].name + " hit!");
			print (factions [index].t.toString ());
		}

		//handle years
		if (month == 1) {
			//possibility of all player's timelines getting hit each year
			if(Random.Range(0,2) == 0){
				print ("ALL FACTIONS HIT!");
				for (int i = 0; i < factions.Count; i++) {
					print (factions [i].name + " hit!");
					mutate (factions[i].t.timeline);
					print(factions[i].t.toString());
				}
			}
			year++;
		}
	}

	//Randomizer functions ---------------------------------------------------------------------------------------
	public Event RandomEvent(){
		int partsMax = Random.Range (2, 5);
		List<string> parts = new List<string>();
		List<int> cons = new List<int>();
		for (int i = 0; i < partsMax; i++) {
			string newPart = factionNames [Random.Range (0, factionNames.Count)];

			//Checks if it is going to add a faction already in the list, if so: break
			if (parts.BinarySearch (newPart) >= 0)	break; 
			parts.Add (newPart);
			cons.Add(Random.Range(-3,3));
		}
		List<int> date = new List<int> (new int[]{ Random.Range (0, 1500), Random.Range (1, 12), Random.Range (1, 30) });
		string name = "Battle of the " + RandomString (Random.Range (3, 8));
		int importance = Random.Range (0, 5);
		Event rando = new Event (parts, date, cons, name, importance); 
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
	public void mutate(List<Event> t){
		int index = Random.Range (0, t.Count-1);
		int data;

		//determining which data to mutate
		if (Random.Range(0,4) > t[index].records.Count){
			data = Random.Range (0, 5);
		} else { //the data's recording has protected it
			print("protected");
			return;
		}

		if (data == 0) { print (index + " participants");//participants 

			int change = Random.Range(0, t[index].participants.Count-1);

			//Change participants to a random faction
			int randFact = Random.Range (0, factions.Count - 1);
			string partChange = factions [randFact].name;

			//Log the participants getting changed
			t[index].recordChange ("Time", "participants", change, partChange);
			t[index].participants [change] = factions[randFact].name;
		
		} else if (data == 1) { print (index + " date");//date
			int change = Random.Range(0,100);

			if (change <= 36) { //0-36 are days
				change *= (5 / 6); //lower it to within 0-30
				if(change == 0) change = 1; //There is no 0th day of a month
				string newDate = t[index].getMonth(t[index].date [1]) + " " + change + ", " + t[index].date [0];//t[index].date [1] + " " + t[index].date [2] + ", " + t[index].date [0];
				t [index].recordChange ("Time", "date", newDate);
				t [index].date [2] = change;
			} else if (change >= 37 && change <= 72) { //37-72 are months
				change /= 3; //lower it to 1-12
				string newDate = t[index].getMonth(change) + " " + t[index].date [2] + ", " + t[index].date [0];
				t [index].recordChange ("Time", "date", newDate);
				t[index].date [1] = change;
			} else if(change >= 73 && change <= 86){ //73-86 subtracts from years
				change -= 72; //make change between 0-14
				change *= -2; //make change between -28 and 0
				if (t[index].date [0] + change < 0) {
					string newDate = t[index].getMonth(t[index].date [1]) + " " + t[index].date [2] + ", 0";
					t [index].recordChange ("Time", "date", newDate);
					t [index].date [0] = 0;
				} else {
					string newDate = t[index].getMonth(t[index].date [1])  + " " + t[index].date [2] + ", " + change;
					t [index].recordChange ("Time", "date", newDate);	
					t[index].date [0] += change;
				}
			} else { //87-100
				change -= 86; //makes change between 0-14
				change *= 2; //makes change between 0-28
				if (t [index].date [0] + change > year) {
					string newDate = t[index].getMonth(t[index].date [1])  + " " + t[index].date [2] + ", " + year;
					t [index].recordChange ("Time", "date", newDate);
					t[index].date [0] = year;
				} else {
					string newDate = t[index].getMonth(t[index].date [1])  + " " + t[index].date [2] + ", " + change;
					t [index].recordChange ("Time", "date", newDate);
					t[index].date [0] += change;
				}
			}
				
		} else if (data == 2) { print (index + " consequences");//consequences
			
			int length = t[index].consequences.Count;
			int change = Random.Range (0, length);
			int amount = 1;
			if(Random.Range(0,1) == 1){
				amount = -1;
			}
			int newAmount = t [index].consequences [change] + amount;
			t [index].recordChange ("Time", "consequences", change, newAmount.ToString());
			t[index].consequences [change] = newAmount;
		
		} else if (data == 3) { print (index + " name");//name

			//Needs last word to be the one it will edit
			int spaceIndex = t[index].name.LastIndexOf(' ');
			string newName = t[index].name.Substring(0,spaceIndex+1);
			newName += RandomString (Random.Range (5, 10));

			t [index].recordChange ("Time", "name", newName); 

		} else if (data == 4) { print (index + " importance");//importance

			int amount = 1;
			if(Random.Range(0,1) == 1){
				amount = -1;
			}

			int newImportance = t [index].importance + amount;
			t [index].recordChange ("Time", "importance", newImportance.ToString());

			t[index].importance += amount;

		} else if (data == 5) { print (index + " recorded");//recorded
			t[index].removeRandomRecording("Time"); //change recording is handled inside this function

		}

	}

}
