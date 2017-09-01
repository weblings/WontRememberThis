using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline{

	public List<Event> timeline;

	//Constructors ---------------------------------------------
	public Timeline(){
		timeline = new List<Event> ();
	}

	public Timeline(List<Event> _timeline){
		timeline = new List<Event>( _timeline.ToArray()); //new List<Event> (_timeline); caused all timelines to ref eachother
	}

	//functions --------------------------------------------------
	public string toString(){
		string output = "Timeline[";
		for (int i = 0; i < timeline.Count; i++) {
			output += timeline [i].toString ();
			if (i + 1 != timeline.Count) output += "\n";
		}
		output += "]";
		return output;
	}
}
