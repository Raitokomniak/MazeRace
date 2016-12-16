using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using UnityEngine.Networking;

class SeedMessage : MessageBase{
	public string seed;
}

public class MyMsgType {
	public static short Seed = MsgType.Highest + 1;
}

public class NetworkHandler : NetworkBehaviour {


	public NetworkClient client;
	public NetworkManager netManager;

	public const short SeedMsgId = 1337;


	public int playerId = 0;
	bool joinToastPlayed;
	bool leaveToastPlayed = true;

	[SyncVar(hook = "OnRecentlyJoinedChanged")]
	int recentlyJoined = 0;

	[SyncVar(hook = "OnRecentlyLeftChanged")]
	int recentlyLeft = 0;


	[SyncVar(hook = "OnPlayerCountChanged")]
	public int playerCount = 0;

	public int playerCountStash =0;

	[SyncVar(hook = "OnFinishedChanged")]
	public int finishedPlayer = 0;

	[SyncVar(hook = "OnSeedChanged")]
	public int seed = 1337;

	[SyncVar(hook = "OnSizeXChanged")]
	public int sizeX = 0;

	[SyncVar(hook = "OnSizeYChanged")]
	public int sizeY = 0;

	public override void OnStartServer(){

	}

	public override void OnStartClient(){
		
	}

	public void Finish(){
		finishedPlayer = playerId;
	}

	void Update(){
		int foundPlayers = GameObject.FindGameObjectsWithTag ("Player").Length;
		if(playerCount != foundPlayers)
		{
			
			//If was this who joined
			if (playerId == 0) {
				Debug.Log ("Connected to server");
				playerId = foundPlayers;
				if(playerCount < foundPlayers)	recentlyJoined = playerId;

				playerCount = GameObject.FindGameObjectsWithTag ("Player").Length;
				playerCountStash = playerCount;

				//Set seed and generate
				GameControl.gameControl.maze.seed = seed;
				Debug.Log ("Generating maze with seed " + seed);
				StartCoroutine (GameControl.gameControl.maze.WaitForLoad (playerId));

				Debug.Log ("Connected, " + playerCount + " players, playedID: " + playerId);
			} else {
				if (playerCount < foundPlayers) {
					recentlyJoined = GameObject.FindGameObjectsWithTag ("Player").Length;
					playerCount = GameObject.FindGameObjectsWithTag ("Player").Length;
				}
			}
		}

	}


	public void StartHosting(){
		netManager.StartHost();
	}

	public void JoinGame(){
		netManager.StartClient();
		netManager.networkAddress = GameControl.gameControl.ui.IPInput.text;

	}

	void OnGUI(){
		if (NetworkClient.active && !ClientScene.ready)
		{
			if (GUI.Button(new Rect(100, 100, 200, 20), "Client Ready"))
			{
				ClientScene.Ready(netManager.client.connection);

				if (ClientScene.localPlayers.Count == 0)
				{
					ClientScene.AddPlayer(0);
				}
			}
		}
	}

	/////////////////////
	/// MAP INFO
	/////////////////////
	void OnSeedChanged(int seed){

		GameControl.gameControl.maze.seed = seed;
		Debug.Log ("seed changed to " + seed);
	}

	void OnSizeXChanged(int sizeX)
	{
		GameControl.gameControl.maze.xSize = sizeX;
	}

	void OnSizeYChanged(int sizeY)
	{
		GameControl.gameControl.maze.ySize = sizeY;
	}


	/////////////////////
	/// PLAYER INFO
	/////////////////////

	void OnRecentlyJoinedChanged(int recentlyJoined)
	{
		Debug.Log ("Player " + recentlyJoined + " joined the game");
		GameControl.gameControl.ui.PlayToast ("Player " + recentlyJoined + " joined the game");
		joinToastPlayed = true;
		//leaveToastPlayed = false;
	}

	void OnRecentlyLeftChanged(int recentlyLeft)
	{
		GameControl.gameControl.ui.PlayToast ("Player " + recentlyLeft + " left the game");

	}

	void OnPlayerCountChanged(int playerCount){
		
		if (playerId != 0) {
			Debug.Log (playerCount + " players vs stash " + playerCountStash);
			if (playerCount > playerCountStash) {
				Debug.Log ("New Player, recently joined playedID: " + recentlyJoined);
				joinToastPlayed = false;
			} else if (playerCount < playerCountStash) {
				Debug.Log ("Just left id:  " + recentlyLeft);
				leaveToastPlayed = false;
			}

		}
	}

	void OnFinishedChanged(int finishedPlayer)
	{
		GameControl.gameControl.ui.PlayToast ("Player " + finishedPlayer + " has found the exit!");
	}



}
