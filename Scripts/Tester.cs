using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour {

	void Start(){
		/*List<int> parts = new List<int> (new int[]{1, 2});
		List<int> dates = new List<int> (new int[]{1, 5, 7});
		List<int> cons = new List<int> (new int[]{1, -1});
		Event e = new Event (parts, dates, cons, "Battle of Blah", 5, 0);
		print (e.toString());*/
		Timeline t = new Timeline ();
		for (int i = 0; i < 5; i++) {
			t.timeline.Add (RandomEvent ());
		}
		Faction a = new Faction("The Irish", t);
		print (a.toString ());
	}

	Event RandomEvent(){
		int partsMax = Random.Range (0, 5);
		List<int> parts = new List<int>();
		List<int> cons = new List<int>();
		for (int i = 0; i < partsMax; i++) {
			parts.Add (i);
			cons.Add(Random.Range(-3,3));
		}
		List<int> date = new List<int> (new int[]{ Random.Range (0, 1500), Random.Range (0, 12), Random.Range (0, 12) });
		string name = "Battle of the " + RandomString (Random.Range (3, 8));
		int importance = Random.Range (0, 5);
		int quality = Random.Range (0, 4);
		Event rando = new Event (parts, date, cons, name, importance, quality);
		return rando;
	}

	//returns randomized string of that length
	string RandomString(int length){
		string output = "";
		char[] alphabet = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
		for (int i = 0; i < length; i++) {
			int index = Random.Range (0, 26);
			output += alphabet [index];
		}
		return output;
	}
}
