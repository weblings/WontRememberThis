using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event{
	public List<string> participants; //those involved
	public List<int> date; //Y,M,D (most likely to be altered)
	public List<int> consequences; //represent buffs/debuffs for participants
	public string name; //name of event
	public int importance; //importance of event (need to figure out ordering)
	public List<string> records; //location of recording. Multiply by 25% in genetic alg for protction value
	public int type; //0 = historical, 1 = mythical
	public List<List<string>> changes; //if more than 3 changes -> mythical

	//Constructor -----------------------------------------------------------------------------------------------------------------------
	public Event(List<string> _participants, List<int> _date, List<int> _consequences, string _name, int _importance){
		participants = _participants;
		date = _date;
		consequences = _consequences;
		name = _name;
		importance = _importance;
		records = new List<string> ();
		type = 0; //default will be historical
		changes = new List<List<string>>();
	}

	public Event(List<string> _participants, List<int> _date, List<int> _consequences, string _name, int _importance, int _type){
		participants = _participants;
		date = _date;
		consequences = _consequences;
		name = _name;
		importance = _importance;
		records = new List<string> ();
		type = _type; 
		changes = new List<List<string>>();
	}

	public Event(List<string> _participants, List<int> _date, List<int> _consequences, string _name, int _importance, List<string> _records){
		participants = _participants;
		date = _date;
		consequences = _consequences;
		name = _name;
		importance = _importance;
		records = _records;
		type = 0; //default will be historical
		changes = new List<List<string>>();
	}

	public Event(List<string> _participants, List<int> _date, List<int> _consequences, string _name, int _importance, List<string> _records, int _type){
		participants = _participants;
		date = _date;
		consequences = _consequences;
		name = _name;
		importance = _importance;
		records = _records;
		type = _type; 
		changes = new List<List<string>>();
	}

	//functions -------------------------------------------------------------------------------------------------------------------------
	public string toString(){
		string output = "{Event: " + name + ", Date: " + date [1] + " " + date [2] + ", " + date [0] + " Importance: " + importance;
		output+= ", Participants/Consequences: ";
		for (int i = 0; i < participants.Count; i++) {
			output+= participants [i]+"/"+consequences[i];
			if (i + 1 != participants.Count) output += ", ";
		}
		output += "}";
		return output;
	}

	//public void recordChange(string ChangerID, string change){

	//}

	public void makeMythical(){
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

	public void removeRandomRecording(){
		int index = Random.Range (0, records.Count);
		records.RemoveAt (index);
	}
}
