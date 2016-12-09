using UnityEngine;
using System.Collections;

public class AIHandler : MonoBehaviour {

	Vector3 moveDir;
	Quaternion fromRotation;
	float movementSpeed;
	float turnRate;
	bool moving;
	bool rotating;
	float initialYRot;
	float targetYRot;
	bool checkingDirection;
	// Use this for initialization
	void Start () {
	
	}

	void Awake(){
		moving = true;
		movementSpeed = 1f;
		initialYRot = 90;
		targetYRot = 90;

		turnRate = 100f;
		moveDir = transform.forward;
		transform.rotation = Quaternion.LookRotation (Vector3.right);
		rotating = true;
	}



	// Update is called once per frame
	void FixedUpdate () {

		RaycastHit hit;
		Vector3 forward = transform.TransformDirection (Vector3.forward);

		Ray forwardRay = new Ray (transform.position, transform.forward);
		Debug.DrawRay (forwardRay.origin, forwardRay.direction * .5f, Color.red);


		if (moving) {
			if (Physics.Raycast (forwardRay, out hit, .5f)) {
				//Debug.Log ("free to go");
				if (hit.collider.tag == "Wall") {
					moving = false;
					checkingDirection = true;
					fromRotation = transform.rotation;
				}
			}
			transform.position += transform.forward * Time.deltaTime * movementSpeed;
		} else if (checkingDirection) {
			StartCoroutine (CheckDirection ());
		}

	}

	IEnumerator CheckDirection(){
		RaycastHit hitRight;
		RaycastHit hitLeft;

	Ray rightRay = new Ray (transform.position, transform.right);
	Ray leftRay = new Ray (transform.position, -transform.right);
	Debug.DrawRay (rightRay.origin, rightRay.direction, Color.red);
	Debug.DrawRay (leftRay.origin, leftRay.direction, Color.cyan);


	bool rightOpen = true;
	bool leftOpen = true;

	if (Physics.Raycast (rightRay, out hitRight, .5f)) {
		if (hitRight.collider.tag == "Wall") {
			Debug.Log ("wall on right");
			rightOpen = false;
		}
	}
	else if (Physics.Raycast (leftRay, out hitLeft, .5f)) {
		if (hitLeft.collider.tag == "Wall") {
			Debug.Log ("wall on left");
			leftOpen = false;
		} 
	}

		yield return new WaitForSeconds (2f);
	if (rightOpen && !leftOpen) {
		Debug.Log ("Go Right");
		//targetYRot = 180;
	} else if (!rightOpen && leftOpen) {
		Debug.Log ("Go Left");
		//targetYRot = 270;
	} else {
		Debug.Log ("must do u turn");
	}

	checkingDirection = false;
	}
}
