using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public GameObject gameSetupPanel;
	public Text seedInput;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ToggleGameSetup(bool value)
	{
		gameSetupPanel.SetActive (value);

		if (value == true) {
			seedInput.text = GameControl.gameControl.maze.seed.ToString();
		}
	}
}
