using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event{
	public List<int> participants; //those involved
	public List<int> date; //Y,M,D (most likely to be altered)
	public List<int> consequences; //represent buffs/debuffs for participants
	public string name; //name of event
	public int importance; //importance of event (need to figure out ordering)
	public int recorded; //quality of recording of event. Multiply by 25% in genetic alg for protction value

	//Constructor -----------------------------------------------------------------------------------------------------------------------
	public Event(List<int> _participants, List<int> _date, List<int> _consequences, string _name, int _importance, int _recorded){
		participants = _participants;
		date = _date;
		consequences = _consequences;
		name = _name;
		importance = _importance;
		recorded = _recorded;
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

}
