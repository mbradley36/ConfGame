using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerStateManager : MonoBehaviour
{
	public NetworkPlayer networkPlayer;
	private string networkPlayerString = "";
	public bool initialized;
	public bool isMe, winner;
	public GameObject opponentSprite, mySprite, canvas, finishLine, winText, typeGUI;

	public SimpleStateMachine stateMachine;
	SimpleState setupState, menuState, connectState, playState, disconnectState;

	public GameObject padPrefab;
	//public List<Pad> pads = new List<Pad>();
	public List<Color> padColors;

	public int padCount = 0;

	void Start()
	{
		Init();
		winner = false;
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
		HideNetworkGUI ();

	}

	private void HideNetworkGUI() {
		canvas.SetActive(false);
	}

	public void ResetNetworkPlayer(NetworkPlayer player)
	{
		//initialized = false;
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
		if (Network.isServer && !winner)
		{
			//if (mySprite.transform.position.x < 13.36) {
				if (Input.GetKeyDown (KeyCode.Return)) {
					int points = textFieldHandler.instance.GetWorth();
					mySprite.transform.position = new Vector3 (mySprite.transform.position.x + points*0.3f, mySprite.transform.position.y, mySprite.transform.position.z);
					MainStateManager.Instance.SendPointsScored(points);
				}
			//}
		} else if(!winner) {
			//if (opponentSprite.transform.position.x < 13.36) {
				if (Input.GetKeyDown (KeyCode.Return)) {
					int points = textFieldHandler.instance.GetWorth();
					opponentSprite.transform.position = new Vector3 (opponentSprite.transform.position.x + points*0.3f, opponentSprite.transform.position.y, opponentSprite.transform.position.z);
					MainStateManager.Instance.SendPointsScored(points);
				}
			//}
		}

		if(winner) typeGUI.SetActive(false);

		if (mySprite.transform.position.x > finishLine.transform.position.x) {
			if(Network.isServer) {
				winText.SetActive(true);
				winText.GetComponent<Text>().text = "You win!";
			} else {
				winText.SetActive(true);
				winText.GetComponent<Text>().text = "Your oppnonent wins!";
			}
		} else if (opponentSprite.transform.position.x > finishLine.transform.position.x){
			if(Network.isServer) {
				winText.SetActive(true);
				winText.GetComponent<Text>().text = "Your oppnonent wins!";
			} else {
				winText.SetActive(true);
				winText.GetComponent<Text>().text = "You win!";
			}
		}
	}

	void PlayExit() {}
	#endregion

	public void MovedAmt(int points){
		if(Network.isServer) {
			opponentSprite.transform.position = new Vector3 (opponentSprite.transform.position.x + points*0.3f, opponentSprite.transform.position.y, opponentSprite.transform.position.z);
		} else {
			mySprite.transform.position = new Vector3 (mySprite.transform.position.x + points*0.3f, mySprite.transform.position.y, mySprite.transform.position.z);
		}
	}

	#region DISCONNECT
	void DisconnectEnter() {
		Application.LoadLevel (1);
	}

	void DisconnectUpdate() {}

	void DisconnectExit() 
	{
		stateMachine.SwitchStates(menuState);
	}
	#endregion

}