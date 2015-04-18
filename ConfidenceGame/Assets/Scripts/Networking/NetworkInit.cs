using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(NetworkView))]
public class NetworkInit : MonoBehaviour {

	private string serverIp = "143.215.54.5";
	public int numConnections;
	public int port;
	public bool useNatPunch;

	public RectTransform clientUI;
	public RectTransform serverUI;
	public RectTransform joinUI;

	public Text ipField;
	public Text myIp;
	public Toggle clientConnected;
	public Toggle serverUp;
	public Toggle joinedGame;

	public Timer timer;

	// Use this for initialization
	public void Start () {
		if(PlayerPrefs.HasKey("ip")) 
			ipField.text = PlayerPrefs.GetString("ip");
		
		myIp.text = "Not Connected";
		
		if(Application.internetReachability != NetworkReachability.NotReachable) myIp.text = 
			"My IP: " + Network.player.ipAddress;
	}
	
	// Update is called once per frame
	public void Update () {
	
	}

	public void StartServer(){
		Network.InitializeServer(numConnections, port, useNatPunch);
		StartCoroutine(VerifyServerRunning());
	}

	public void ConnectToServer(){
		serverIp = ipField.text.Trim();
		
		PlayerPrefs.SetString("ip", serverIp);
		PlayerPrefs.Save();
		
		Network.Connect(serverIp, port);
		StartCoroutine(VerifyConnectedAsClient());
	}

	private IEnumerator VerifyServerRunning(){
		timer.Restart();

		while(!timer.IsFinished() && !Network.isServer){
			yield return null;
		}

		if(Network.isServer){
			serverUp.isOn = true;
			myIp.gameObject.SetActive(true);
			clientUI.gameObject.SetActive(false);
			NetworkHandler.ServerStarted();
		}
	}

	private IEnumerator VerifyConnectedAsClient(){

		timer.Restart();

		while(!timer.IsFinished() && !Network.isClient){
			yield return null;
		}
		
		if(Network.isClient){

		PlayerPrefs.SetString("ip", serverIp);
		PlayerPrefs.Save();
		
			clientConnected.isOn = true;
			serverUI.gameObject.SetActive(false);
			joinUI.gameObject.SetActive(true);
		}
	}

	public void JoinGame(){
		UnitySerializer serializer = new UnitySerializer();
		serializer.Serialize((int) NetObject.MainStateManager);
		serializer.Serialize(2); // NetworkHandler.JoinRequest
		serializer.Serialize(Network.player.ipAddress);
		serializer.Serialize(Network.player.port);
		NetworkHandler.NetworkCallSend((int) NetObject.MainStateManager, serializer.ByteArray, RPCMode.Server);
	}
}
