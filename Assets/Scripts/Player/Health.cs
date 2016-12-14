using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

	public const int maxHealth = 100;
	[SyncVar(hook = "OnChangedHealth")]
	public int currentHealth = maxHealth;
	public RectTransform healthbar;

	public void TakeDamage(int amount)
	{
		if(!isServer)
		{
			return;
		}

		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			currentHealth = 0;
			Debug.Log("Dead!");
		}
	}

	void OnChangedHealth(int health) {
		healthbar.sizeDelta = new Vector2 (health, healthbar.sizeDelta.y);
	}
}
