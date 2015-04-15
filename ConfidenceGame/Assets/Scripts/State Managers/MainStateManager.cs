using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainStateManager : NetworkObj {
	#region VARIABLES
	// INSTANCE
	public static MainStateManager Instance;

	// STATES
	public SimpleStateMachine stateMachine;
	SimpleState waitState, gameState;

	// PLAYER STUFF
	public PlayerStateManager me;
	public PlayerStateManager friend;

	// DEBUG STUFF
	public bool skipMultiplayer = false;
	#endregion

	#region GAME LOOP
	void Awake () 
	{
		Instance = this;
	}

	void Start () 
	{
		Setup();
		//DEFINE STATES
		waitState = new SimpleState(WaitEnter, WaitUpdate, WaitExit, "[WAIT]");
		gameState = new SimpleState(GameEnter, GameUpdate, GameExit, "[GAME]");
		
		stateMachine.SwitchStates(waitState);
	}

	void Update() 
	{
		Execute();
	}

	private void Setup() 
	{
		InitNetworkFunctions();
	}

	public void Execute () 
	{
		stateMachine.Execute();
	}
	#endregion

	#region MENU
	void WaitEnter() {}

	void WaitUpdate() 
	{
		if (friend.initialized || skipMultiplayer)
		{
			stateMachine.SwitchStates(gameState);
		}
	}	

	void WaitExit() {}
	#endregion

	#region GAME
	void GameEnter() 
	{
		me.Connect();
		friend.Connect();
	}

	void GameUpdate() {}

	void GameExit() {}
	#endregion

	#region Networking
	private enum Functions
	{
		JoinRequest,
		PadUpdated,
		PadDestroyed,
		DoubleTapped,
		Length
	}

	public override void InitNetworkFunctions()
	{
		Debug.Log("Initialize Network Functions");
		networkFunctions = new ByteParamDelegate[(int) Functions.Length];
		networkFunctions[(int) Functions.JoinRequest] = PlayerJoinRequest;
		networkFunctions[(int) Functions.PadUpdated] = PadUpdated;
		networkFunctions[(int) Functions.PadDestroyed] = PadDestroyed;
		networkFunctions[(int) Functions.DoubleTapped] = DoubleTapped;
		NetworkHandler.RegisterNetworkedMessage(ReceiveNetworkCall, (int) NetObject.MainStateManager);
		NetworkHandler.AddPlayerConnectListener(FriendConnected);
	}

	public override void ReceiveNetworkCall (byte[] data)
	{
		int function = System.BitConverter.ToInt32(data, sizeof(int));
		networkFunctions[function](data);
		//Debug.Log("Received Network Call for function " + function);
	}

	public void FriendConnected(NetworkPlayer player)
	{
		friend.SetNetworkPlayer(player);
		Debug.Log("Friend Connected");
	}

	public void PlayerJoinRequest(byte[] data)
	{
		Debug.Log("Join request");

		UnitySerializer serializer = new UnitySerializer(data);
		serializer.DeserializeInt();
		serializer.DeserializeInt();
		string ip = serializer.DeserializeString();
		int port = serializer.DeserializeInt();

		// If the player is already connected, ignore repeated requests to join
		if (!friend.initialized)
		{
			NetworkPlayer player = NetworkHandler.Instance.GetNetworkPlayer(ip);
			friend.networkPlayer = player;
			friend.Init();
		}
	}

	public void PadUpdated(byte[] data)
	{
		UnitySerializer serializer = new UnitySerializer(data);
		serializer.DeserializeInt();	// net object
		serializer.DeserializeInt();	// function
		int id = serializer.DeserializeInt();
		Vector3 position = serializer.DeserializeVector3();
		Vector3 cVector = serializer.DeserializeVector3();
		Color c = new Color(cVector.x, cVector.y, cVector.z, 1f);

		bool exists = false;
		foreach (Pad pad in friend.pads)
		{
			if (pad.id == id)
			{
				exists = true;
				pad.SetPosition3(position);
				pad.SetColor(c);
			}
		}

		if (!exists)
		{
			friend.CreatePad(id, position);
		}
	}

	public void SendPadUpdate(int id, Vector3 position, Vector3 c)
	{
		UnitySerializer serializer = new UnitySerializer();
		serializer.Serialize((int) NetObject.MainStateManager);
		serializer.Serialize((int) Functions.PadUpdated);
		serializer.Serialize(id);
		serializer.Serialize(position);
		serializer.Serialize(c);

		NetworkHandler.NetworkCallSend((int) NetObject.MainStateManager, serializer.ByteArray, RPCMode.Others);
	}

	public void PadDestroyed(byte[] data)
	{
		UnitySerializer serializer = new UnitySerializer(data);
		serializer.DeserializeInt();	// net object
		serializer.DeserializeInt();	// function
		int id = serializer.DeserializeInt();

		friend.DestroyPad(id);
	}

	public void SendPadDestroyed(int id)
	{
		UnitySerializer serializer = new UnitySerializer();
		serializer.Serialize((int) NetObject.MainStateManager);
		serializer.Serialize((int) Functions.PadDestroyed);
		serializer.Serialize(id);

		NetworkHandler.NetworkCallSend((int) NetObject.MainStateManager, serializer.ByteArray, RPCMode.Others);
	}

	public void DoubleTapped(byte[] data)
	{
		UnitySerializer serializer = new UnitySerializer(data);
		serializer.DeserializeInt();	// net object
		serializer.DeserializeInt();	// function
		Vector3 position = serializer.DeserializeVector3();

		Debug.Log("RECEIVED DOUBLE TAP at " + position);
	}

	public void SendDoubleTapped(Vector3 position)
	{
		UnitySerializer serializer = new UnitySerializer();
		serializer.Serialize((int) NetObject.MainStateManager);
		serializer.Serialize((int) Functions.DoubleTapped);
		serializer.Serialize(position);

		NetworkHandler.NetworkCallSend((int) NetObject.MainStateManager, serializer.ByteArray, RPCMode.Others);
	}
	#endregion
}
