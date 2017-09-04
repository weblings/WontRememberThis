using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleButtons : MonoBehaviour {

	public float timeScale; //how fast the button will set the game speed
	public Image image; //the Image that holds the color of the button that will be changed if it is on/off
	public bool isPause; //whether this is the 'Pause' button or not
	public float ColorScale; //amount from 0f-1f new color's Red will be

	float ColorScale2; //will hold new color's Green
	float ColorScale3; //will hold new color's Blue
	float scaleAmount;

	public void Start(){
		scaleAmount = .65f;
		if (isPause) { //if Pause, make new color Red
			ColorScale2 = scaleAmount;
			ColorScale3 = scaleAmount;
		} else { //if a normal time button make new color Green
			ColorScale2 = ColorScale;
			ColorScale3 = scaleAmount;
			ColorScale = scaleAmount;
		}
	}

	public void Update(){
		if (Time.timeScale == timeScale) image.color = new Color(ColorScale, ColorScale2, ColorScale3);
		else image.color = new Color(1f, 1f, 1f); //if button is not active, remove color tint
	}

	//Actually affects the speed of the game (is called by OnClick() Event)
	public void changeTime(){
		Time.timeScale = timeScale;
	}


		
}
