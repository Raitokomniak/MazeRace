using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MazeGenerator : MonoBehaviour {

	static Random.State seedGenerator;
	public int seed = 1337;
	static bool seedGeneratorInit = false;
	public int playerCount;

	public GameObject wall;
	public GameObject floorTile;
	GameObject floorHolder;
	public float wallLength = 1f;
	public float xSize = 5f;
	public float ySize = 5f;

	GameObject[] spawnPoints;

	//TRAps
	public GameObject upDownTrap;

	Vector3 initialPos;
	GameObject wallHolder;
	public Cell[] cells;
	public int currentCell = 0;
	int totalCells;
	int visitedCells = 0;
	bool startedBuilding = false;
	int currentNeighbour = 0;
	List<int> lastCells;
	int backingUp = 0;
	int wallToBreak;

	public int trapAmountMultiplier;
	int[] traps;

	bool init = false;


	// Use this for initialization
	void Start () {
		
	}
		
	public void Awake(){
		if (SceneManager.GetActiveScene ().name == "GameSetupScene") {
			
		} else if(SceneManager.GetActiveScene().name == "MazeLevel" && !init) {
			seed = 1337;
			StartGeneration ();
			init = true;
		}
	}

	public void StartDebugGeneration(){
		Random.InitState (seed);

		playerCount = 3;
		Debug.Log (playerCount + " players");
		CreateWalls ();
	}

	public void StartGeneration(){

		GameControl.gameControl.ui.ToggleGameSetup (false);
		Random.InitState (seed);
		xSize = 10;
		ySize = 10;

		//playerCount = GameControl.gameControl.ui.playerCountSelection.value + 1;
		playerCount = 1;
		spawnPoints = new GameObject[3];
		Debug.Log (playerCount + " players");

		CreateWalls ();

		//StartCoroutine (Load ());

	}
	IEnumerator Load(){
		Random.InitState (seed);
		xSize = float.Parse (GameControl.gameControl.ui.sizeXInput.text);
		ySize = float.Parse (GameControl.gameControl.ui.sizeYInput.text);

		playerCount = GameControl.gameControl.ui.playerCountSelection.value + 1;
		spawnPoints = new GameObject[3];
		Debug.Log (playerCount + " players");

		AsyncOperation loadScene = SceneManager.LoadSceneAsync ("MazeLevel");
		yield return loadScene;

		CreateWalls ();
	}



	public void GenerateSeed(){
		var generatedSeed = Random.Range(int.MinValue, int.MaxValue);
		seedGenerator = Random.state;
		seed = generatedSeed;
		GameControl.gameControl.ui.UpdateSeedInput ();
	}


	void CreateWalls(){
		initialPos = new Vector3 ((-xSize / 2) + wallLength/2, 0, (-ySize/2)+wallLength/2);
		Vector3 initialFloorPos = new Vector3 (6f, -1.3f, 15.25f);
		Vector3 myPos = initialPos;
		Vector3 myFloorPos = initialFloorPos;

		wallHolder = GameObject.Find ("Walls");
		floorHolder = GameObject.Find ("FloorTiles");

		GameObject tempWall;
		GameObject tempFloor;
		bool exit = false;

		//X axis
		for (int i = 0; i < ySize; i++) {
			for (int j = 0; j <= xSize; j++) {
				myPos = new Vector3 (initialPos.x + (j * wallLength) - wallLength / 2, 0, initialPos.z + (i * wallLength) - wallLength / 2);

				myFloorPos = new Vector3(initialPos.x + (j* wallLength) - wallLength, 0, initialPos.z + (i * wallLength) - wallLength / 2);
				tempWall = Instantiate (wall, myPos, Quaternion.identity) as GameObject;
				tempWall.transform.SetParent (wallHolder.transform);

				if (j != 0) {
					tempFloor = Instantiate (floorTile, myFloorPos, Quaternion.identity) as GameObject;
					tempFloor.transform.SetParent (floorHolder.transform);
				}

				if (j == 0 && !exit) {
					GameObject exitMarker = Instantiate (Resources.Load ("ExitMarker"), tempWall.transform.position, Quaternion.identity) as GameObject;
					Destroy (tempWall);
					exit = true;
				}
				CreateSpawnPoints (i, j, tempWall);
			}
		}

		//Y axis
		for (int i = 0; i <= ySize; i++) {
			for (int j = 0; j < xSize; j++) {
				myPos = new Vector3 (initialPos.x + (j * wallLength), 0, initialPos.z + (i * wallLength) - wallLength);
				tempWall = Instantiate (wall, myPos, Quaternion.Euler(0,90,0)) as GameObject;
				tempWall.transform.SetParent (wallHolder.transform);
				//tempWall.transform.localScale = new Vector3 (.15f, 2, 1);
			}
		}

		CreateCells ();
	}

	void CreateSpawnPoints(int i, int j, GameObject tempWall)
	{
		if (i == ySize - 1 && j == xSize && playerCount >= 1) {
			GameObject _spawnPoint = Instantiate (Resources.Load ("SpawnPoint"), tempWall.transform.position, Quaternion.identity) as GameObject;
			spawnPoints [0] = _spawnPoint;
		}
		else if (i == 0 && j == xSize && playerCount >= 2) {
			GameObject _spawnPoint = Instantiate (Resources.Load ("SpawnPoint"), tempWall.transform.position, Quaternion.identity) as GameObject;
			spawnPoints [1] = _spawnPoint;
		}
		else if (i == ySize - 1 && j == 0 && playerCount >= 3) {
			GameObject _spawnPoint = Instantiate (Resources.Load ("SpawnPoint"), tempWall.transform.position, Quaternion.identity) as GameObject;
			spawnPoints [2] = _spawnPoint;
		}
	}

	void AdjustSpawnPosition(Cell cell, int i){
		if (i == xSize && spawnPoints[0] != null) {
			spawnPoints [0].transform.position = cell.floorObject.transform.position;
		}
		else if (i == xSize * ySize && spawnPoints[1] != null) {
			spawnPoints [1].transform.position = cell.floorObject.transform.position;
		}
		else if (i == xSize * (ySize - 1) && spawnPoints[2] != null) {
			spawnPoints [2].transform.position = cell.floorObject.transform.position;
		}
	}



	void CreateCells(){
		lastCells = new List<int>();
		lastCells.Clear ();
		totalCells = Mathf.RoundToInt(xSize * ySize);
		GameObject[] allWalls;
		int children = wallHolder.transform.childCount;
		allWalls = new GameObject [children];
		cells = new Cell[Mathf.RoundToInt(xSize) * Mathf.RoundToInt(ySize)];

		int eastWestProcess = 0;
		int childProcess = 0;
		int termCount = 0;

		for (int i = 0; i < children; i++) {
			allWalls [i] = wallHolder.transform.GetChild (i).gameObject;
		}

		for (int cellProcess = 0; cellProcess < cells.Length; cellProcess++) {
			cells [cellProcess] = new Cell ();
			cells [cellProcess].east = allWalls [eastWestProcess];
			cells [cellProcess].south = allWalls [Mathf.RoundToInt(childProcess + (xSize+1) * ySize)];
			if (termCount == xSize) {
				eastWestProcess += 2;
				termCount = 0;
			} else
				eastWestProcess++;
			termCount++; 
			childProcess++;
			cells [cellProcess].west = allWalls [eastWestProcess];
			cells[cellProcess].north = allWalls[Mathf.RoundToInt((childProcess+(xSize+1)*ySize)+xSize-1)];
			cells [cellProcess].floorObject = floorHolder.transform.GetChild (cellProcess).gameObject;
			AdjustSpawnPosition (cells [cellProcess], cellProcess);
		}

		CreateMaze ();
	}

	void CreateMaze(){
		while (visitedCells < totalCells) {
			if (startedBuilding) {
				GiveMeNeighbour ();
				if (cells [currentNeighbour].visited == false && cells[currentCell].visited == true) {
					BreakWall ();
					cells [currentNeighbour].visited = true;
					visitedCells++;
					lastCells.Add (currentCell);
					currentCell = currentNeighbour;
					if (lastCells.Count > 0) {
						backingUp = lastCells.Count - 1;
					}
				}
			} else {
				currentCell = Random.Range (0, totalCells);
				cells [currentCell].visited = true;
				visitedCells++;
				startedBuilding = true;
			}
		}


		CreateTraps ();
	}

	//////////////////////
	// TRAPS
	//////////////////////
	void CreateTraps(){
		if (trapAmountMultiplier == 0)
			trapAmountMultiplier = 1;
		
		traps = new int[Mathf.RoundToInt(xSize / 2) * trapAmountMultiplier];

		for (int i = 0; i < traps.Length; i++) {

			GameObject trapMarker = Instantiate (Resources.Load ("TrapMarker")) as GameObject;
			GameObject trap = Instantiate (upDownTrap);

			while (CheckExistingTraps (i) == false) {

			}
			trapMarker.transform.position = cells [traps[i]].north.transform.position;
			trap.transform.position = cells [traps [i]].north.transform.position;
			TrapMechanic trapMechanic = trap.GetComponent<TrapMechanic>();
			trapMechanic.TrapStepsOnSecond = Random.Range (8, 15);
		}
	}

	bool CheckExistingTraps(int index){
		int randomCell = Random.Range (1, cells.Length);
		for (int j = 0; j < traps.Length; j++) {
			if (traps [j] == randomCell) {
				return false;
			}
		}
		traps [index] = randomCell;
		return true;
	}

	void BreakWall(){
		switch (wallToBreak) {
		case 1:
			Destroy (cells [currentCell].north);
			break;
		case 2:
			Destroy (cells [currentCell].east);
			break;
		case 3:
			Destroy (cells [currentCell].west);
			break;
		case 4:
			Destroy (cells [currentCell].south);
			break;
		}
	}

	void GiveMeNeighbour(){

		int length = 0;
		int[] neighbours = new int[4];
		int[] connectingWall = new int[4];
		int check = 0;
		check = Mathf.RoundToInt(((currentCell+1)/xSize));
		check -= 1;
		check *= Mathf.RoundToInt(xSize);
		check += Mathf.RoundToInt(xSize);


		//west
		if (currentCell + 1 < totalCells && (currentCell + 1) != check) {
			if (cells [currentCell + 1].visited == false) {
				neighbours [length] = currentCell + 1;
				connectingWall [length] = 3;
				length++;
			}
		}

		//east
		if (currentCell - 1 >= 0 && currentCell != check) {
			if (cells [currentCell - 1].visited == false) {
				neighbours [length] = currentCell - 1;
				connectingWall [length] = 2;
				length++;
			}
		}

		//north
		if (currentCell + xSize < totalCells) {
			if (cells [Mathf.RoundToInt(currentCell + xSize)].visited == false) {
				neighbours [length] = Mathf.RoundToInt(currentCell + xSize);
				connectingWall [length] = 1; 
				length++;
			}
		}

		//south
		if (currentCell - xSize >= 0) {
			if (cells [Mathf.RoundToInt(currentCell - xSize)].visited == false) {
				neighbours [length] = Mathf.RoundToInt(currentCell - xSize);
				connectingWall [length] = 4;
				length++;
			}
		}

		if (length != 0) {
			int theChosenOne = Random.Range (0, length);
			currentNeighbour = neighbours [theChosenOne];
			wallToBreak = connectingWall[theChosenOne];
		} else {
			if (backingUp > 0) {
				currentCell = lastCells [backingUp];
				backingUp--;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
