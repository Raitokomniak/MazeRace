using UnityEngine;
using System.Collections;

public class FinishHandler : MonoBehaviour {

	public NetworkHandler networkHandler;

	// Use this for initialization
	void Start () {
	
	}

	void Awake(){
		networkHandler = GameObject.Find ("Server").GetComponent<NetworkHandler> ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void Finish(){
		networkHandler.Finish ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			Finish ();
		}
	}
}
