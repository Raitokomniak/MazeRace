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

	[SyncVar(hook = "OnSeedChanged")]
	public int playerCount = 0;

	[SyncVar(hook = "OnSeedChanged")]
	public int seed = 1337;

	[SyncVar(hook = "OnSizeXChanged")]
	public int sizeX = 10;

	[SyncVar(hook = "OnSizeYChanged")]
	public int sizeY = 10;

	public override void OnStartServer(){
		Debug.Log ("server started");

	}

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

	void Update(){
		//SendSeed ();
	}

	public void SendSeed(){
		//Debug.Log ("Sending seed");
		SeedMessage msg = new SeedMessage();
		msg.seed = "1111";
		//NetworkServer.SendToAll (MyMsgType.Seed, msg);
		//NetworkServer.SendToClient (1, MyMsgType.Seed, msg);

	}

	public override void OnStartClient(){
		playerCount++;
		Debug.Log ("Generating maze with seed " + seed);
		GameControl.gameControl.maze.seed = seed;
		StartCoroutine (GameControl.gameControl.maze.WaitForLoad ());

	}


	public void OnSeed(NetworkMessage netMsg){
		
		//SeedMessage seedMsg = netMsg.ReadMessage<SeedMessage> ();
		//Debug.Log ("getting seed " + seedMsg.seed);
	}

	public void OnPlayerConnected(NetworkPlayer player){
		Debug.Log ("someone connected, send seed");
	}

	public void OnConnected(NetworkMessage netMsg)
	{
		Debug.Log("Connected to server");
	}
}

