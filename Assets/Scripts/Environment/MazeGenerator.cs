using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MazeGenerator : MonoBehaviour {

	public GameObject wall;
	public GameObject floor;
	public float wallLength = 1f;
	public float xSize = 5f;
	public float ySize = 5f;

	Vector3 initialPos;
	public GameObject wallHolder;
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

	// Use this for initialization
	void Start () {

		//Muuta tämä sitten triggeröitymään eri paikasta kun peli alkaa
		CreateWalls ();
	}

	void CreateWalls(){
		initialPos = new Vector3 ((-xSize / 2) + wallLength/2, 0, (-ySize/2)+wallLength/2);
		Vector3 myPos = initialPos;
		float floorMultiplier;
		floor.transform.localScale = new Vector3 (xSize / 8, 1, ySize / 8);
		floor.transform.position = initialPos + new Vector3(xSize / 2f, -.5f, ySize / 2f);
		GameObject tempWall;
		bool exit = false;

		//X axis
		for (int i = 0; i < ySize; i++) {
			for (int j = 0; j <= xSize; j++) {
				myPos = new Vector3 (initialPos.x + (j * wallLength) - wallLength / 2, 0, initialPos.z + (i * wallLength) - wallLength / 2);
				tempWall = Instantiate (wall, myPos, Quaternion.identity) as GameObject;
				tempWall.transform.SetParent (wallHolder.transform);
				if (j == 0 && !exit) {
					GameObject exitMarker = Instantiate (Resources.Load ("ExitMarker"), tempWall.transform.position, Quaternion.identity) as GameObject;
					Destroy (tempWall);
					exit = true;

				}
			}
		}

		//Y axis
		for (int i = 0; i <= ySize; i++) {
			for (int j = 0; j < xSize; j++) {
				myPos = new Vector3 (initialPos.x + (j * wallLength), 0, initialPos.z + (i * wallLength) - wallLength);
				tempWall = Instantiate (wall, myPos, Quaternion.Euler(0,90,0)) as GameObject;
				tempWall.transform.SetParent (wallHolder.transform);
			}
		}

		CreateCells ();
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

			while (CheckExistingTraps (i) == false) {

			}
			trapMarker.transform.position = cells [traps[i]].north.transform.position;
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
