using UnityEngine;
using System.Collections;

public class NetworkFunction : MonoBehaviour {

	public static NetworkFunction Instance;

	public TestNetFunction customObj;

	// Use this for initialization
	void Start () 
	{
		Debug.Log("Function");
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {}

	public static void NetworkReceiveFunction(byte[] data)
	{
		Instance.customObj = TestNetFunction.Deserialize(data);

		Debug.Log("Message Received");
	}

}