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
			currentHealth = maxHealth;
			Debug.Log("Dead!");
			RpcRespawn ();
		}
	}

	void OnChangedHealth(int health) {
		healthbar.sizeDelta = new Vector2 (health, healthbar.sizeDelta.y);
	}

	[ClientRpc]
	void RpcRespawn()
	{
		if(isLocalPlayer)
		{
			//set actual spawn position here
			transform.position = Vector3.zero;
		}
	}
}
