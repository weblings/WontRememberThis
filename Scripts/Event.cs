﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : Helper{
	public SortedDictionary<string,int> participants; //those involved : consequences of involvement
	public List<int> date; //Y,M,D (most likely to be altered)
	public string name; //name of event
	public int importance; //importance of event (need to figure out ordering)
	public List<string> records; //location of recording. More records -> better protection of event
	public int type; //0 = historical, 1 = mythical
	public List<string> changes; //if more than 3 changes -> mythical

	//Constructor -----------------------------------------------------------------------------------------------------------------------
	public Event(List<string> _participants, List<int> _date, List<int> _consequences, string _name, int _importance){
		//participants = createParticipants(_participants, _consequences);
		participants = new SortedDictionary<string, int> ();
		createParticipants(_participants, _consequences);
		date = _date;
		name = _name;
		importance = _importance;
		records = new List<string> ();
		type = 0; //default will be historical
		changes = new List<string>();
	}

	public Event(List<string> _participants, List<int> _date, List<int> _consequences, string _name, int _importance, int _type){
		participants = new SortedDictionary<string, int> ();
		createParticipants(_participants, _consequences);		date = _date;
		name = _name;
		importance = _importance;
		records = new List<string> ();
		type = _type; 
		changes = new List<string>();
	}

	public Event(List<string> _participants, List<int> _date, List<int> _consequences, string _name, int _importance, List<string> _records){
		participants = new SortedDictionary<string, int> ();
		createParticipants(_participants, _consequences);		date = _date;
		name = _name;
		importance = _importance;
		records = _records;
		type = 0; //default will be historical
		changes = new List<string>();
	}

	public Event(List<string> _participants, List<int> _date, List<int> _consequences, string _name, int _importance, List<string> _records, int _type){
		participants = new SortedDictionary<string, int> ();
		createParticipants(_participants, _consequences);
		date = _date;
		name = _name;
		importance = _importance;
		records = _records;
		type = _type; 
		changes = new List<string>();
	}

	//functions -------------------------------------------------------------------------------------------------------------------------

	//contructor helper function
	void createParticipants(List<string> p, List<int> c){
		string[] p2 = new string[p.Count];
		p.CopyTo(p2);
		int[] c2 = new int[c.Count];
		c.CopyTo(c2);

		for (int i = 0; i < p.Count; i++) {
			if (participants.ContainsKey (p2 [i])) {
				Debug.Log(ArrayToString<string>(p2));
				Debug.Log (p2 [i]);
				Debug.Log (SDtoString<string,int> (participants));
			}
			participants.Add (p2 [i], c2 [i]);
		}
		//return part;
	}


	//returns Event represented as a string
	public string toString(){
		string output = "{Event: " + name + ", Date: " + getMonth(date [1]) + " " + date [2] + ", " + date [0] + " Importance: " + importance;
		output+= ", Participants:Consequences : ";
		output += SDtoString<string,int> (participants);
		output += ",\n(Changes: ";
		for (int i = 0; i < changes.Count; i++) {
			output += changes [i];
			if (i + 1 != changes.Count) output += ", ";
		}
		output += "), Type: ";
		if (type == 0) {
			output += "Historic";
		} else if (type == 1) {
			output += "Mythic";
		}
		output += "}";
		return output;
	}

	//recordChange() functions---------------------------------------------------------------------
	//Puts any change in the list of changes (this is for lists)
	public void recordChange(string ChangerID, string thingChanged, int index, string change){
		string oldData = "";

		if (thingChanged == "date") {
			oldData = dateToString ();
		} else if (thingChanged == "records") {
			oldData = records [index];
		}

		string toLog = ChangerID + " changed " +thingChanged+": "+ oldData + " -> " + change;
		changes.Add (toLog);
		mythCheck ();
	}

	//Puts any change in the list of changes (this is for single variables)
	public void recordChange(string ChangerID, string thingChanged, string change){
		string oldData = "";

		if (thingChanged == "name") {
			oldData = name;
		} else if (thingChanged == "importance") {
			oldData = importance.ToString ();
		} 

		string toLog = ChangerID + " changed "+thingChanged+": " + oldData + " -> " + change;
		changes.Add (toLog);
		mythCheck ();
	}

	//Puts any change in the list of changes (this is for dictionaries)
	public void recordChange(string ChangerID, string thingChanged, string change, string key){
		string oldData = "";

		if (thingChanged == "participants") {
			oldData = key;
		} else if (thingChanged == "consequences") {
			oldData = participants [key].ToString();
			participants [key] = int.Parse(change);
		}

		string toLog = ChangerID + " changed "+thingChanged+": " + oldData + " -> " + change;
		changes.Add (toLog);
		mythCheck ();
	}

	public void mythCheck(){
		if (changes.Count >= 3) {
			type = 1; //made mythical
		}
	}

	public void addRecording(string location){
		records.Add (location);
	}

	public void removeRecording(string location){
		int index = records.BinarySearch (location);
		if (index >= 0) {
			records.RemoveAt (index);
		} else {
			Debug.Log(location + "does not have a record of: " + name);
		}
	}

	public void removeRandomRecording(string identity){
		int index = Random.Range (0, records.Count);
		recordChange (identity, "records", index, "record lost");
		records.RemoveAt (index);
	}

	//Convert date into a string
	public string dateToString(){
		string month = getMonth (date [1]);
		return month + " " + date [2] + ", " + date [0];
	}

	//Return month corresponding to int given
	public string getMonth(int input){
		string month = "";
		if (input == 1) month = "January";
		else if (input == 2) month = "February";
		else if (input == 3) month = "March";
		else if (input == 4) month = "April";
		else if (input == 5) month = "May";
		else if (input == 6) month = "June";
		else if (input == 7) month = "July";
		else if (input == 8) month = "August";
		else if (input == 9) month = "September";
		else if (input == 10) month = "October";
		else if (input == 11) month = "November";
		else if (input == 12) month = "December";

		return month;
	}
}
