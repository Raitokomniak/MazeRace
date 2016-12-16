using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

	void Awake() {
		//GetComponent<Rigidbody>().velocity = bulletSpawn.transform.up * -6;
	}

	void OnCollisionEnter(Collision c) {
		var hit = c.gameObject;
		var health = hit.GetComponent<Health>();
		if (health  != null) {
			health.TakeDamage(10);
		}
		Debug.Log ("collided with " + c.gameObject.name);
		Destroy (this.gameObject);
	}
}
