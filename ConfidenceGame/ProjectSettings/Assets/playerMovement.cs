using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour {

	public GameObject runner;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (runner.transform.position.x < 13.36) {
			if (Input.GetKeyDown (KeyCode.Return)) {
				runner.transform.position = new Vector3 (runner.transform.position.x + 1, runner.transform.position.y, runner.transform.position.z);
			}
		}
	}
}
