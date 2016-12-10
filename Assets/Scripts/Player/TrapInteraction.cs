using UnityEngine;
using System.Collections;

public class TrapInteraction : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Trap"))
        {
            gameObject.SetActive(false);
        }
    }
}
