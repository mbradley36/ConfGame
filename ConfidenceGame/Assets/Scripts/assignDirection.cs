using UnityEngine;
using System.Collections;

public class assignDirection : MonoBehaviour {

	public float direction;

	// Use this for initialization
	void Start () {
		float x = Random.value;
		if (x >= .5f)
			direction = 1f;
		else
			direction = -1f;
		Destroy (gameObject, 60f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public float getDirection(){
		return direction;
	}
}
