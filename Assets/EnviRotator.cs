using UnityEngine;
using System.Collections;

public class EnviRotator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (GameObject.Find ("Finish").transform.position, Vector3.up, 20 * Time.deltaTime);
	}
}
