using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum NetObject
{
	MainStateManager,
	Length
}

[RequireComponent (typeof(NetworkView))]
public class NetworkHandler : MonoBehaviour 
{
	public static NetworkHandler Instance;

	public ByteParamDelegate[] networkFunctions;
	public NetworkPlayerParamDelegate PlayerConnected;
	private Dictionary<string, NetworkPlayer> networkPlayerDict;

	public string ip;

	// Use this for initialization
	void Awake () 
	{
		if(Instance == null)
		{
			Instance = this;
			networkFunctions = new ByteParamDelegate[0];
			networkPlayerDict = new Dictionary<string, NetworkPlayer>();
		}
		else
		{
			GameObject.Destroy(this.gameObject);
		}
	}

	public void Update() {}


	public static void NetworkCallSend(int functionId, byte[] data, RPCMode mode) 
	{
		if(NetworkingActive()) Instance.networkView.RPC("NetworkCallReceive", mode, functionId, data);
	}

	public static void NetworkCallSend(int functionId, byte[] data, NetworkPlayer target) 
	{
		if(NetworkingActive()) Instance.networkView.RPC("NetworkCallReceive", target, functionId, data);
	}

	[RPC]
	private void NetworkCallReceive(int functionId, byte[] data)
	{
		//Debug.Log("Received");
		networkFunctions[functionId](data);
	}

	public static void RegisterNetworkedMessage(ByteParamDelegate function, int messageId)
	{
		//Debug.Log(function.Method.Name);

		if(Instance.networkFunctions.Length <= messageId)
		{
			ByteParamDelegate[] temp = new ByteParamDelegate[messageId + 1];
			System.Array.Copy(Instance.networkFunctions, temp, Instance.networkFunctions.Length);
			Instance.networkFunctions = temp;
		}

		//Debug.Log(Instance.networkFunctions.Length);
		Instance.networkFunctions[messageId] = function;
	}

	public static void AddNetworkedMessage(ByteParamDelegate function, int functionId)
	{
		if (Instance.networkFunctions.Length > functionId && Instance.networkFunctions[functionId] != null)
		{
			Instance.networkFunctions[functionId] += function;
		}
		else
		{
			RegisterNetworkedMessage(function, functionId);
		}
	}

	public static void AddPlayerConnectListener(NetworkPlayerParamDelegate function)
	{
		Instance.PlayerConnected += function;
	}

	public void OnPlayerConnected(NetworkPlayer player)
	{
		// HACK
		if(!MainStateManager.Instance.friend.initialized)
		{
			networkPlayerDict.Add(player.ipAddress, player);
			PlayerConnected(player);
		}	
	}

	public void OnPlayerDisconnected(NetworkPlayer player)
	{
		if(Network.isServer)
		{
			networkPlayerDict.Remove(player.ipAddress);
		}
	}

	public static void ServerStarted()
	{
		Debug.Log("Server started");
	}

	// Can we even send or receive messages right now?
	public static bool NetworkingActive()
	{
		return Instance != null && (Network.isServer || Network.isClient);
	}

	public bool IsPlayerConnected(string ip)
	{
		return networkPlayerDict.ContainsKey(ip);
	}

	public NetworkPlayer GetNetworkPlayer(string ip)
	{
		return networkPlayerDict[ip];
	}
}
