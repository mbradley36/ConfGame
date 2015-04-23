using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStateManager : MonoBehaviour
{
	public NetworkPlayer networkPlayer;
	private string networkPlayerString = "";
	public bool initialized;
	public bool isMe;
	public GameObject opponentSprite, mySprite;

	public SimpleStateMachine stateMachine;
	SimpleState setupState, menuState, connectState, playState, disconnectState;

	public GameObject padPrefab;
	//public List<Pad> pads = new List<Pad>();
	public List<Color> padColors;

	public int padCount = 0;

	void Start()
	{
		Init();
	}

	public void Init () 
	{
		setupState = new SimpleState(SetupEnter, SetupUpdate, SetupExit, "SETUP");
		menuState = new SimpleState(MenuEnter, MenuUpdate, MenuExit, "MENU");
		connectState = new SimpleState(ConnectEnter, ConnectUpdate, ConnectExit, "CONNECT");
		playState = new SimpleState(PlayEnter, PlayUpdate, PlayExit, "PLAY");
		disconnectState = new SimpleState(DisconnectEnter, DisconnectUpdate, DisconnectExit, "DISCONNECT");

		Setup();
	}

	public void SetNetworkPlayer(NetworkPlayer player)
	{
		networkPlayer = player;
		initialized = true;
		networkPlayerString = "" + networkPlayer;

	}

	public void ResetNetworkPlayer(NetworkPlayer player)
	{
		initialized = false;
		networkPlayerString = "";
	}
	
	void Update () 
	{
		Execute();
	}

	public void Setup () 
	{
		stateMachine.SwitchStates(setupState);
	}

	public void Connect ()
	{
		stateMachine.SwitchStates(connectState);
	}

	public void Execute () 
	{
		stateMachine.Execute();
	}

	#region SETUP
	void SetupEnter() 
	{

	}

	void SetupUpdate() 
	{
		stateMachine.SwitchStates(connectState);
	}

	void SetupExit() {}
	#endregion

	#region MENU
	void MenuEnter() {}

	void MenuUpdate() {}

	void MenuExit() {}
	#endregion

	#region CONNECT
	void ConnectEnter() {}

	void ConnectUpdate() 
	{
		// HACK
		if (Network.isClient || Network.isServer)
		{
			stateMachine.SwitchStates(playState);
		}
	}

	void ConnectExit() 
	{
		if (isMe)
		{
			SetNetworkPlayer(Network.player);
		}
	}
	#endregion

	#region PLAY
	void PlayEnter()
	{
		
	}

	void PlayUpdate() 
	{
		if (Network.isServer)
		{
			if (mySprite.transform.position.x < 13.36) {
				if (Input.GetKeyDown (KeyCode.Return)) {
					int points = textFieldHandler.instance.GetWorth();
					mySprite.transform.position = new Vector3 (mySprite.transform.position.x + points*0.5f, mySprite.transform.position.y, mySprite.transform.position.z);
					MainStateManager.Instance.SendPointsScored(points);
				}
			}
		} else {
			if (mySprite.transform.position.x < 13.36) {
				if (Input.GetKeyDown (KeyCode.Return)) {
					int points = textFieldHandler.instance.GetWorth();
					opponentSprite.transform.position = new Vector3 (opponentSprite.transform.position.x + points*0.5f, opponentSprite.transform.position.y, opponentSprite.transform.position.z);
					MainStateManager.Instance.SendPointsScored(points);
				}
			}
		}
	}

	void PlayExit() {}
	#endregion

	public void MovedAmt(int points){
		if(Network.isServer) {
			opponentSprite.transform.position = new Vector3 (opponentSprite.transform.position.x + points*0.5f, opponentSprite.transform.position.y, opponentSprite.transform.position.z);
		} else {
			mySprite.transform.position = new Vector3 (mySprite.transform.position.x + points*0.5f, mySprite.transform.position.y, mySprite.transform.position.z);
		}
	}

	#region DISCONNECT
	void DisconnectEnter() {}

	void DisconnectUpdate() {}

	void DisconnectExit() 
	{
		stateMachine.SwitchStates(menuState);
	}
	#endregion

	/*public Pad CreatePad(Vector3 position)
	{
		GameObject newPadObject = Instantiate(padPrefab, new Vector3(position.x, position.y, 0f), Quaternion.identity) as GameObject;
		Pad newPad = newPadObject.GetComponent<Pad>();
		newPad.transform.parent = this.transform;
		newPad.owner = networkPlayer;
		newPad.id = padCount++;
		newPad.SetPosition3(position);
		newPad.color = padColors[Random.Range(0, padColors.Count)];
		pads.Add(newPad);

		return newPad;
	}

	public Pad CreatePad(Finger finger)
	{
		Vector3 fingerPos = finger.GetWorldPosition3();
		GameObject newPadObject = Instantiate(padPrefab, fingerPos, Quaternion.identity) as GameObject;
		Pad newPad = newPadObject.GetComponent<Pad>();
		newPad.transform.parent = this.transform;
		newPad.owner = networkPlayer;
		newPad.id = padCount++;
		newPad.SetPosition3(finger.GetWorldPosition3());
		newPad.color = padColors[Random.Range(0, padColors.Count)];
		newPad.Hold(finger);
		pads.Add(newPad);

		return newPad;
	}

	public Pad CreatePad(int id, Vector3 position)
	{
		// prevent padCount from lagging behind
		if (id > padCount)
		{
			padCount = id;
		}

		return CreatePad(position);
	}
	
	public void ReturnPadToFinger(Finger finger){
		Pad existingPad = pads[0];
		existingPad.SetPosition3(finger.GetWorldPosition3());
		existingPad.Hold(finger);
	}

	public void DestroyPad(int id)
	{
		// Mark a pad for destruction
		foreach (Pad pad in pads)
		{
			if (pad.id == id)
			{
				pad.isDead = true;
			}
		}
	}*/
}