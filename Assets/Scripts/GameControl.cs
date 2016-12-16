using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameControl : MonoBehaviour {

	//Kaikki erilliset kontrollit liitetään tähän luokkaan ja niitä voi sitten kutsua gameControl objektin kautta
	public static GameControl gameControl;

	public UIController ui;
	public MazeGenerator maze;

	void Awake(){
		if (gameControl	 == null) {
			DontDestroyOnLoad (gameObject);
			gameControl = this;
		} else if (gameControl != this) {
			Destroy (gameObject);
		}


		ui = GetComponent<UIController> ();
		maze = GetComponent<MazeGenerator> ();
	}


	public void StartGame(){
		SceneManager.LoadScene ("GameSetupScene");
		ui.lobbyPanel.SetActive (true);
		ui.startScreenPanel.SetActive (false);
	}
}
