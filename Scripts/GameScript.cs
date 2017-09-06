using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour{

	public List<Faction> factions;
	public List<string> factionNames; //probably will get rid of this after testing
	//public Timeline t;
	SortedDictionary<string,City> cities;
	Helper h = new Helper();

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
			Faction f = new Faction(factionName,newCityName,t,factionNames);
			factions.Add (f);
			f.updateAllAffinities ();
			f.updateAllEventListings ();
			print(f.toString ());
		}

		setupFactionsPerceptions ();
		//for (int i = 0; i < factions.Count; i++) {
			//print(factions [i].SDtoString<string,float> (factions [i].factionsPerceptions));
			//print(factions[i].SDListValuetoString<string,int>(factions[i].eventListing));
		//`}
		int index = Random.Range (0, factions.Count);
		mutate (factions [index]);
		InvokeRepeating ("Time", 0f, 2f); //2f might be good for normal gamerate
	}

	//make time move forward, mutates as it progresses
	void Time(){

		//handle months
		if (month < 12)	month++;
		else month = 1;

		//possibility of a player's timeline getting hit each month
		if (Random.Range (0, 4) == 0) {
			int index = Random.Range (0, factions.Count);
			mutate (factions [index]);
			print (factions [index].name + " hit!");
			//print (factions [index].t.toString ());
			print(factions[index].name+"\'s knownFactions: "+h.SDtoString<string,float>(factions[index].knownFactions));
		}

		//handle years
		if (month == 1) {
			//possibility of all player's timelines getting hit each year
			if(Random.Range(0,2) == 0){
				print ("ALL FACTIONS HIT!");
				for (int i = 0; i < factions.Count; i++) {
					print (factions [i].name + " hit!");
					mutate (factions[i]);
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

			//parts.BinarySearch(newPart); BinarySearch does not work if list is not sorted, sorting + BinarySearch = O((n+1)logn)
			//so literally doing O(n) dumb search is faster (length <= 5 anyway but still)

			//Checks if it is going to add a faction already in the list, if so: say a duplicate has been found and break from further searching
			bool same = false;
			for (int j = 0; j < parts.Count; j++) {
				if (parts [j].Equals (newPart)) {
					same = true;
					break;
				}
			}
			if (!same) {
				parts.Add (newPart);
				cons.Add (Random.Range (-3, 3));
			}
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
	public void mutate(Faction f){
		int index = Random.Range (0, f.t.timeline.Count-1);
		int data;

		//determining which data to mutate
		if (Random.Range(0,4) > f.t.timeline[index].records.Count){
			data = Random.Range (0, 5);
		} else { //the data's recording has protected it
			//print("protected");
			return;
		}

		if (data == 0) { //print (index + " participants");//participants 

			int change = Random.Range(0, f.t.timeline[index].participants.Count);
			string[] part = new string[f.t.timeline[index].participants.Count]; //make an array, the length of participants
			f.t.timeline[index].participants.Keys.CopyTo (part, 0); //put the keys in this array
			string oldPart = part[change];

			//Change participants to a random faction
			int randFact = Random.Range (0, factions.Count);
			string partChange = factions [randFact].name;

			//Log the participants getting changed
			f.t.timeline[index].recordChange ("Time", "participants", partChange, oldPart);

			//Change them
			int value = f.t.timeline[index].participants [oldPart];
			f.removeParticipant (oldPart, index);
			f.t.timeline[index].participants.Remove (oldPart);

			if(f.t.timeline[index].participants.ContainsKey(partChange)){
				f.t.timeline[index].participants[partChange] = value;
			}else{
				f.t.timeline[index].participants.Add (partChange, value);
				f.addParticipant (partChange, index);
			}

			//Update faction's affinity
			f.updateFactionAffinity(oldPart);
			f.updateFactionAffinity (partChange);
		
		} else if (data == 1) { //print (index + " date");//date
			int change = Random.Range(0,100);

			if (change <= 36) { //0-36 are days
				change *= (5 / 6); //lower it to within 0-30
				if(change == 0) change = 1; //There is no 0th day of a month
				string newDate = f.t.timeline[index].getMonth(f.t.timeline[index].date [1]) + " " + change + ", " + f.t.timeline[index].date [0];//t[index].date [1] + " " + t[index].date [2] + ", " + t[index].date [0];
				f.t.timeline[index].recordChange ("Time", "date", newDate);
				f.t.timeline[index].date [2] = change;
			} else if (change >= 37 && change <= 72) { //37-72 are months
				change /= 3; //lower it to 1-12
				string newDate = f.t.timeline[index].getMonth(change) + " " + f.t.timeline[index].date [2] + ", " + f.t.timeline[index].date [0];
				f.t.timeline[index].recordChange ("Time", "date", newDate);
				f.t.timeline[index].date [1] = change;
			} else if(change >= 73 && change <= 86){ //73-86 subtracts from years
				change -= 72; //make change between 0-14
				change *= -2; //make change between -28 and 0
				if (f.t.timeline[index].date [0] + change < 0) {
					string newDate = f.t.timeline[index].getMonth(f.t.timeline[index].date [1]) + " " + f.t.timeline[index].date [2] + ", 0";
					f.t.timeline[index].recordChange ("Time", "date", newDate);
					f.t.timeline[index].date [0] = 0;
				} else {
					string newDate = f.t.timeline[index].getMonth(f.t.timeline[index].date [1])  + " " + f.t.timeline[index].date [2] + ", " + change;
					f.t.timeline[index].recordChange ("Time", "date", newDate);	
					f.t.timeline[index].date [0] += change;
				}
			} else { //87-100
				change -= 86; //makes change between 0-14
				change *= 2; //makes change between 0-28
				if (f.t.timeline[index].date [0] + change > year) {
					string newDate = f.t.timeline[index].getMonth(f.t.timeline[index].date [1])  + " " + f.t.timeline[index].date [2] + ", " + year;
					f.t.timeline[index].recordChange ("Time", "date", newDate);
					f.t.timeline[index].date [0] = year;
				} else {
					string newDate = f.t.timeline[index].getMonth(f.t.timeline[index].date [1])  + " " + f.t.timeline[index].date [2] + ", " + change;
					f.t.timeline[index].recordChange ("Time", "date", newDate);
					f.t.timeline[index].date [0] += change;
				}
			}
				
		} else if (data == 2) { //print (index + " consequences");//consequences
			
			mutateConsequences (f, index);

		} else if (data == 3) { //print (index + " name");//name

			//Needs last word to be the one it will edit
			int spaceIndex = f.t.timeline[index].name.LastIndexOf(' ');
			string newName = f.t.timeline[index].name.Substring(0,spaceIndex+1);
			newName += RandomString (Random.Range (5, 10));

			f.t.timeline[index].recordChange ("Time", "name", newName); 

		} else if (data == 4) { //print (index + " importance");//importance

			int amount = 1;
			if(Random.Range(0,1) == 1){
				amount = -1;
			}

			int newImportance = f.t.timeline[index].importance + amount;
			f.t.timeline[index].recordChange ("Time", "importance", newImportance.ToString());

			f.t.timeline[index].importance += amount;

		} else if (data == 5) { //print (index + " recorded");//recorded
			f.t.timeline[index].removeRandomRecording("Time"); //change recording is handled inside this function

		}

	}

	//V1 of mutating consequences
	void mutateConsequences(Faction f, int index){
		int change = Random.Range(0, f.t.timeline[index].participants.Count);
		string[] part = new string[f.t.timeline[index].participants.Count]; //make an array, the length of participants
		f.t.timeline [index].participants.Keys.CopyTo (part, 0); //put the keys in this array
		string oldPart = part[change];

		int amount = 1;
		if(Random.Range(0,1) == 1){
			amount = -1;
		}
		int newAmount = f.t.timeline [index].participants [oldPart] + amount;
		f.t.timeline [index].recordChange ("Time", "consequences", newAmount.ToString(), oldPart);

		//Update faction's affinity
		f.updateFactionAffinity(oldPart);
	}

	//Faction functions that require access to all factions ------------------------------------------------------------------------------------------

	//faction perceptions
	void setupFactionsPerceptions(){
		//Need a dictionary to keep this runtime down TODO: replace List<Faction> factions with this
		SortedDictionary<string,Faction> f = new SortedDictionary<string, Faction> ();

		for (int i = 0; i < factions.Count; i++) {
			f.Add (factions [i].name, factions [i]);
		}

		for (int i = 0; i < factions.Count; i++){

			foreach (KeyValuePair<string, bool> k in factions[i].discoveredFactions){
				//Keeping this because there used to be a null ptr error. Fixed now, but if it breaks in the future these will be handy
				if(!f.ContainsKey(k.Key)) print(k.Key+" is not in factions");
				if(!f[k.Key].knownFactions.ContainsKey(factions[i].name)) print(factions[i].name+" is not in faction: "+k.Key+"\'s knownFactions");

				factions [i].factionsPerceptions.Add ( k.Key, f[k.Key].knownFactions [factions[i].name]);
			}
		}

		for (int i = 0; i < factions.Count; i++) {
			factions [i].updateAvgPerception ();
			print (factions[i].name + ": " + factions [i].AvgPerception);
		}
	}

}
