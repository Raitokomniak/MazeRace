using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

	void Awake() {
		
	}

	void OnCollisionEnter(Collision c) {
		var hit = c.gameObject;
		var health = hit.GetComponent<Health>();
		if (health  != null) {
			health.TakeDamage(10);
		}
		Destroy (this.gameObject);
	}
}
