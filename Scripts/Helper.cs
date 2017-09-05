using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper{
	//Unity seems to make the built in ToString() method for SortedDictionry garbage
	public string SDtoString<TKey, TValue>(SortedDictionary<TKey,TValue> dict){
		string output = "";
		foreach (KeyValuePair<TKey, TValue> d in dict)
		{
			output += d.Key + ": " + d.Value+", ";
		}
		output = output.Substring (0, output.Length - 2); //chops off last comma
		output += " ";
		return output;
	}
}

