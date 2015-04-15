using UnityEngine;
using System.Collections;

public class NetworkObj : MonoBehaviour {

	public int netId;

	protected ByteParamDelegate[] networkFunctions;

	public virtual void InitNetworkFunctions(){
		networkFunctions = new ByteParamDelegate[0];
	}

	public virtual void ReceiveNetworkCall(byte[] data){

	}
}
