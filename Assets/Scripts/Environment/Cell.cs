using UnityEngine;
using System.Collections;

public class Cell {
	public bool visited;

	public GameObject north;
	public GameObject south;
	public GameObject west;
	public GameObject east;

	public GameObject floorObject;

	public Cell(){
		north = null;
		south = null;
		west = null;
		east = null;
	}
}
