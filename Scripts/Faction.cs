﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : Helper {

	public string name;
	public Timeline t;
	public float AvgPerception;
	public List<string> cities;
	public SortedDictionary<string,bool> techTree;
	public SortedDictionary<string, int> resource;
	public SortedDictionary<string,float> knownFactions; //stores factionsKnown and the positive/negative feeling this faction feels towards them
	public SortedDictionary<string,bool> discoveredFactions; //whether this faction has encountered other faction themselves
	public SortedDictionary<string, float> factionsPerceptions; //record of what other factions think of this faction
	public SortedDictionary<string, List<int>> eventListing; //keeps list of what events in timeline contain a faction (for fast lookup later)
	float mythValue; //how much myths are worth to this faction

	//constructors ---------------------------------------------------
	public Faction(string _name, List<string> factionNames){
		name = _name;
		t = new Timeline ();
		cities = new List<string> ();
		//TODO: Length of tech tree
		createTechTree();
		resource = new SortedDictionary<string, int> ();
		resource.Add ("gold", 500);
		cities = new List<string> ();
		knownFactions = new SortedDictionary<string, float> ();
		setupKnownFactions (factionNames);
		mythValue = .7f;
		discoveredFactions = new SortedDictionary<string, bool> ();
		setupDiscoveredFactions (factionNames);
		factionsPerceptions = new SortedDictionary<string, float> ();
		eventListing = new SortedDictionary<string, List<int>> (); 
	}

	public Faction(string _name, Timeline _t, List<string> factionNames){
		name = _name;
		t = new Timeline(_t.timeline);	//Was having pointer issues
		cities = new List<string> ();
		//TODO: Length of tech tree
		createTechTree();
		resource = new SortedDictionary<string, int> ();
		resource.Add ("gold", 500);
		cities = new List<string> ();
		knownFactions = new SortedDictionary<string, float> ();
		setupKnownFactions (factionNames);
		mythValue = .7f;
		discoveredFactions = new SortedDictionary<string, bool> ();
		setupDiscoveredFactions (factionNames);
		factionsPerceptions = new SortedDictionary<string, float> ();
		eventListing = new SortedDictionary<string, List<int>> (); 
	}

	public Faction(string _name, string startingCity, Timeline _t, List<string> factionNames){
		name = _name;
		t = new Timeline(_t.timeline);	//Was having pointer issues
		cities = new List<string> ();
		//TODO: Length of tech tree
		createTechTree();
		resource = new SortedDictionary<string, int> ();
		resource.Add ("gold", 500);
		cities = new List<string> ();
		cities.Add (startingCity);
		knownFactions = new SortedDictionary<string, float> ();
		setupKnownFactions (factionNames);
		mythValue = .7f;
		discoveredFactions = new SortedDictionary<string, bool> ();
		setupDiscoveredFactions (factionNames);
		factionsPerceptions = new SortedDictionary<string, float> ();
		eventListing = new SortedDictionary<string, List<int>> (); 
	}

	public Faction(string _name, List<string> _cities, Timeline _t, List<string> factionNames){
		name = _name;
		t = new Timeline(_t.timeline);	//Was having pointer issues
		cities = new List<string> ();
		//TODO: Length of tech tree
		createTechTree();
		resource = new SortedDictionary<string, int> ();
		resource.Add ("gold", 500);
		cities = _cities;
		knownFactions = new SortedDictionary<string, float> ();
		setupKnownFactions (factionNames);
		mythValue = .7f;
		discoveredFactions = new SortedDictionary<string, bool> ();
		setupDiscoveredFactions (factionNames);
		factionsPerceptions = new SortedDictionary<string, float> ();
		eventListing = new SortedDictionary<string, List<int>> (); 
	}

	//print functions ------------------------------------------------------
	public string toString(){
		string output = "(Name: "+name+", AvgPerception: "+AvgPerception+" ,";
		output += t.toString () + "\nCities: ";
		for (int i = 0; i < cities.Count; i++) {
			output += cities [i];
			if (i + 1 != cities.Count) output += ", ";
		}
		output += "\nTech Tree: " + SDtoString<string,bool>(techTree);
		output += "\nResources: " + SDtoString<string,int>(resource);
		output += "\nKnown Factions: " + SDtoString<string,float>(knownFactions);
		output += "\nDiscovered Factions: " + SDtoString<string,bool>(discoveredFactions);
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

	//initializes knownFactions with placholder values to prevent null ptr errors later
	public void setupKnownFactions(List<string> factionNames){
		for (int i = 0; i < factionNames.Count; i++) {
			if (factionNames [i].Equals (name))	knownFactions.Add (name, 5);
			else knownFactions.Add (factionNames [i], 0);
		}
	}

	//Currently makes a single faction known at the beginning
	public void setupDiscoveredFactions(List<string> factions){

		bool chosen = false;
		int index = Random.Range (0, factions.Count);

		//Ensures the random index is actually another faction
		while (factions [index] == name) {
			index = Random.Range (0, factions.Count);
		}

		for (int i = 0; i < factions.Count; i++) {

			if (index == i)	chosen = true;
			else chosen = false;

			if (factions [i] != name) {
				discoveredFactions.Add (factions [i], chosen);
			}
		}
	}

	//updates AvgPerception with how other factions feel about that faction
	public void updateAvgPerception(){
		int divisor = 0;
		AvgPerception = 0; //reset AvgPerception
		foreach (KeyValuePair<string, float> fp in factionsPerceptions) {
			if (discoveredFactions [fp.Key]) {
				AvgPerception += fp.Value;
				divisor++;
			}
		}
		AvgPerception /= divisor;
	}

	//begins list of where factions are described in timeline
	public void updateAllEventListings(){
		for (int i = 0; i < t.timeline.Count; i++) {
			addEvent (t.timeline [i], i);
		}
	}

	//When an event in added to the timeline, any participants involved are added to eventListing at this index
	public void addEvent(Event e, int i){ //if a value isn't entered, default should be the end of the timeline this will be added at
		foreach (KeyValuePair<string, int> p in e.participants){
			if (eventListing.ContainsKey (p.Key)) {
				eventListing [p.Key].Add (i); //store in eventlisting the faction is mentioned at this index in the timeline
			} else {
				List<int> indices = new List<int> ();
				indices.Add (i);
				eventListing.Add (p.Key, indices);
			}
		}
		if(i > t.timeline.Count) t.timeline.Add (e); //if event is listing itself beyond where timeline exists, event should go in that spot
	}

	//returns Affinity for a faction from that event
	public float CalculateYourAffinities(int importance, int eventType){
		if (eventType == 0) {
			return importance;
		} else { // if (eventType == 1) { //in case more than two types exist later
			return importance * mythValue;
		}
	}

	//Will step through the whole timeline and replace knownFactions with the new SortedDictionary
	public void updateAllAffinities(){

		//SortedDictionary<string,float> nkf = new SortedDictionary<string, float> (); //"new knownFactions"
		//nkf.Add (name, 5);

		for (int i = 0; i < t.timeline.Count; i++) { //Step through timeline
			foreach (KeyValuePair<string, int> p in t.timeline[i].participants){
				if (knownFactions.ContainsKey(p.Key)) { //Check if participant is already in nkf
					float value = knownFactions [p.Key]; //get value stored in nkf mapped to that participant
					value += CalculateYourAffinities (t.timeline [i].importance, p.Value);
					knownFactions[p.Key] += value;
				} else { //this should never fire now that knownFactions gets setup, but doesn't hurt to leave it in
					float value = CalculateYourAffinities(t.timeline[i].importance, p.Value);
					knownFactions.Add (p.Key, value);
				}
			}
		}
	}

	//Should step through all occurences of the changedFaction in the timeline and recalculate this faction's affinity towards the changedFaction
	/*public void updateFactionAffinity(string changedFaction){
		int affinity = 0;
		for (int i = 0; i < eventListing [changedFaction]; i++) {

		}
	}*/

}
