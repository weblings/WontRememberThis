using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : Helper {

	public string name;
	public Timeline t;
	public float influence;
	public List<string> cities;
	public SortedDictionary<string,bool> techTree;
	public SortedDictionary<string, int> resource;
	public SortedDictionary<string,float> knownFactions; //stores factionsKnown and the positive/negative feeling this faction feels towards them
	float mythValue; //how much myths are worth to this faction

	//constructors ---------------------------------------------------
	public Faction(string _name){
		name = _name;
		t = new Timeline ();
		influence = 0.1f;
		cities = new List<string> ();
		//TODO: Length of tech tree
		createTechTree();
		resource = new SortedDictionary<string, int> ();
		resource.Add ("gold", 500);
		cities = new List<string> ();
		knownFactions = new SortedDictionary<string, float> ();
		knownFactions.Add (name, 5f); //abritrarily starting with 5 (subject to change)
		mythValue = .7f;
	}

	public Faction(string _name, Timeline _t){
		name = _name;
		t = new Timeline(_t.timeline);	//Was having pointer issues
		influence = 0.1f;
		cities = new List<string> ();
		//TODO: Length of tech tree
		createTechTree();
		resource = new SortedDictionary<string, int> ();
		resource.Add ("gold", 500);
		cities = new List<string> ();
		knownFactions = new SortedDictionary<string, float> ();
		knownFactions.Add (name, 5f); //abritrarily starting with 5 (subject to change)
		mythValue = .7f;
	}

	public Faction(string _name, string startingCity, Timeline _t){
		name = _name;
		t = new Timeline(_t.timeline);	//Was having pointer issues
		influence = 0.1f;
		cities = new List<string> ();
		//TODO: Length of tech tree
		createTechTree();
		resource = new SortedDictionary<string, int> ();
		resource.Add ("gold", 500);
		cities = new List<string> ();
		cities.Add (startingCity);
		knownFactions = new SortedDictionary<string, float> ();
		knownFactions.Add (name, 5f); //abritrarily starting with 5 (subject to change)
		mythValue = .7f;
	}

	public Faction(string _name, List<string> _cities, Timeline _t){
		name = _name;
		t = new Timeline(_t.timeline);	//Was having pointer issues
		influence = 0.1f;
		cities = new List<string> ();
		//TODO: Length of tech tree
		createTechTree();
		resource = new SortedDictionary<string, int> ();
		resource.Add ("gold", 500);
		cities = _cities;
		knownFactions = new SortedDictionary<string, float> ();
		knownFactions.Add (name, 5f); //abritrarily starting with 5 (subject to change)
		mythValue = .7f;
	}

	//print functions ------------------------------------------------------
	public string toString(){
		string output = "(Name: "+name+", Influence: "+influence+" ,";
		output += t.toString () + "\nCities: ";
		for (int i = 0; i < cities.Count; i++) {
			output += cities [i];
			if (i + 1 != cities.Count) output += ", ";
		}
		output += "\nTech Tree: " + SDtoString<string,bool>(techTree);
		output += "\nResources: " + SDtoString<string,int>(resource);
		output += "\nKnown Factions: " + SDtoString<string,float>(knownFactions);
		return output;
	}

	//setup functions ----------------------------------------------------------
	public void createTechTree(){
		techTree = new SortedDictionary<string, bool> ();
		string[] techs = { "cave painting", "written language" };
		for (int i = 0; i < techs.Length; i++) {
			techTree.Add (techs [i], false);
		}
	}

	//returns Affinity for a faction from that event
	public float CalculateAffinity(int importance, int eventType){
		if (eventType == 0) {
			return importance;
		} else { // if (eventType == 1) { //in case more than two types exist later
			return importance * mythValue;
		}
	}

	//Will step through the whole timeline and replace knownFactions with the new SortedDictionary
	public void updateAllAffinities(){

		SortedDictionary<string,float> nkf = new SortedDictionary<string, float> (); //"new knownFactions"
		nkf.Add (name, 5);

		for (int i = 0; i < t.timeline.Count; i++) { //Step through timeline
			for(int j=0; j < t.timeline[i].participants.Count; j++){ //Step through participants in an event
				if (nkf.ContainsKey (t.timeline [i].participants [j])) { //Check if participant is already in nkf
					float value = nkf [t.timeline [i].participants [j]]; //get value stored in nkf mapped to that participant
					value += CalculateAffinity (t.timeline [i].importance, t.timeline [i].consequences [j]);
					nkf[t.timeline [i].participants [j]] += value;
				} else {
					float value = CalculateAffinity(t.timeline[i].importance, t.timeline[i].consequences[j]);
					nkf.Add (t.timeline [i].participants [j], value);
				}
			}
		}

		knownFactions = nkf;
	}

}
