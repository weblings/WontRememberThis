using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction {

	public string name;
	public Timeline t;
	float influence;
	List<string> cities;
	SortedDictionary<string,bool> techTree;
	SortedDictionary<string, int> resource;

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
	}

	public Faction(string _name, Timeline _t){
		name = _name;
		t = _t;
		influence = 0.1f;
		cities = new List<string> ();
		//TODO: Length of tech tree
		createTechTree();
		resource = new SortedDictionary<string, int> ();
		resource.Add ("gold", 500);
		cities = new List<string> ();
	}

	public Faction(string _name, string startingCity, Timeline _t){
		name = _name;
		t = _t;
		influence = 0.1f;
		cities = new List<string> ();
		//TODO: Length of tech tree
		createTechTree();
		resource = new SortedDictionary<string, int> ();
		resource.Add ("gold", 500);
		cities = new List<string> ();
		cities.Add (startingCity);
	}

	public Faction(string _name, List<string> _cities, Timeline _t){
		name = _name;
		t = _t;
		influence = 0.1f;
		cities = new List<string> ();
		//TODO: Length of tech tree
		createTechTree();
		resource = new SortedDictionary<string, int> ();
		resource.Add ("gold", 500);
		cities = _cities;
	}

	//functions ------------------------------------------------------
	public string toString(){
		string output = "(Name: "+name+", Influence: "+influence+" ,";
		output += t.toString () + "Cities: ";
		for (int i = 0; i < cities.Count; i++) {
			output += cities [i];
			if (i + 1 != cities.Count) output += ", ";
		}
		output += "Tech Tree: " + techTree.ToString ();
		output += "\nResources: " + resource.ToString ();
		return output;
	}

	public void createTechTree(){
		techTree = new SortedDictionary<string, bool> ();
		string[] techs = { "cave painting", "written language" };
		for (int i = 0; i < techs.Length; i++) {
			techTree.Add (techs [i], false);
		}
	}
}
