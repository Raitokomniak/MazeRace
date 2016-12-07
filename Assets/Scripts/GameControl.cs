﻿using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

	//Kaikki erilliset kontrollit liitetään tähän luokkaan ja niitä voi sitten kutsua gameControl objektin kautta
	public static GameControl gameControl;


	void Awake(){
		if (gameControl	 == null) {
			DontDestroyOnLoad (gameObject);
			gameControl = this;
		} else if (gameControl != this) {
			Destroy (gameObject);
		}
	}

}
