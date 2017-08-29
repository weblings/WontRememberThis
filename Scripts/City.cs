using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City{

	public string name;
	public List<string> neighbors;
	public SortedDictionary<string,float> FactionsInfluence;
	public SortedDictionary<string,int> Buildings;
	public float happiness;
	public int citizens;

	//constructors ----------------------------------------------------------------------------------------------------------------------
	public City(string _name, List<string> _neighbors, SortedDictionary<string, float> _FI, SortedDictionary<string, int> _b, float _h, int _c){
		name = _name;
		neighbors = _neighbors;
		FactionsInfluence = _FI;
		Buildings = _b;
		happiness = _h;
		citizens = _c;
	}

	//For empty city getting built by faction with no neighbors
	public City(string _name, string faction, float _happiness, int _citizens){
		name = _name;
		neighbors = new List<string> ();
		FactionsInfluence = new SortedDictionary<string,float> ();
		FactionsInfluence.Add (faction, 1f); 
		Buildings = new SortedDictionary<string,int> ();
		happiness = _happiness;
		citizens = _citizens;
	}
}
