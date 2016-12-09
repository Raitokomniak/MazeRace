using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {
	public GameObject gameSetupPanel;
	public InputField seedInput;
	public InputField sizeXInput;
	public InputField sizeYInput;
	public Dropdown playerCountSelection;

	// Use this for initialization
	void Start () {

	}

	void Awake(){
		if (SceneManager.GetActiveScene ().name == "MazeLevel") {
			GameControl.gameControl.ui.ToggleGameSetup (false);
		} else if (SceneManager.GetActiveScene ().name == "GameSetupScene") {
			GameControl.gameControl.ui.ToggleGameSetup (true);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void ToggleGameSetup(bool value)
	{
		gameSetupPanel.SetActive (value);

		if (value == true) {
			//seedInput. = GameControl.gameControl.maze.seed.ToString();
			seedInput.text = GameControl.gameControl.maze.seed.ToString();
			sizeXInput.text = GameControl.gameControl.maze.xSize.ToString ();
			sizeYInput.text = GameControl.gameControl.maze.ySize.ToString ();

		}
	}

	//Update seed to match input field
	public void UpdateSeedValue(){
		GameControl.gameControl.maze.seed = int.Parse(seedInput.text);
	}
	//Update maze size to match input fields
	public void UpdateSizes(){
		if (int.Parse (sizeXInput.text) < 10)
			sizeXInput.text = "10";
		if (int.Parse (sizeYInput.text) < 10)
			sizeYInput.text = "10";
	}

	//Update input field after generation
	public void UpdateSeedInput(){
		seedInput.text = GameControl.gameControl.maze.seed.ToString ();
	}
		

}
