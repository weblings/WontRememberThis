using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper{

	//Found myself making a few for printing loops
	public string ListToString<Ttype>(List<Ttype> list){
		string output = " [";
		for (int i = 0; i < list.Count; i++) {
			output += list [i];
			if (i + 1 < list.Count) output += ", ";
			else output += "]\n ";
		}
		return output;
	}

	//Also an Array for printing loop
	public string ArrayToString<Ttype>(Ttype[] list){
		string output = " [";
		for (int i = 0; i < list.Length; i++) {
			output += list [i];
			if (i + 1 < list.Length) output += ", ";
			else output += "]\n ";
		}
		return output;
	}

	//You cannot declare generic operators -> this kind of generic function won't work
	//public bool ItemInList<Ttype>(List<Ttype> list, Ttype item)

	//Unity seems to make the built in ToString() method for SortedDictionry garbage
	public string SDtoString<TKey, TValue>(SortedDictionary<TKey,TValue> dict){
		string output = "";
		foreach (KeyValuePair<TKey, TValue> d in dict)
		{
			output += d.Key + ": " + d.Value+", ";
		}
		if(output.Length-2 > 0)	output = output.Substring (0, output.Length - 2); //chops off last comma
		output += " ";
		return output;
	}

	//Prints a SortedDictionary mapping keys to Lists
	public string SDListValuetoString<TKey, TValue>(SortedDictionary<TKey,List<TValue>> dict){
		string output = "";
		foreach (KeyValuePair<TKey, List<TValue>> d in dict)
		{
			output += d.Key + ":" + ListToString<TValue> (d.Value);
		}
		return output;
	}
}

