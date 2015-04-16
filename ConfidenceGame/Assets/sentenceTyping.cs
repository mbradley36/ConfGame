using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sentenceTyping : MonoBehaviour {

	public Text sentence;
	//private string tempSentence;
	// Use this for initialization
	void Start () {
		sentence.GetComponent<Text>().text = "";
		//tempSentence = "";
	}
	
	// Update is called once per frame
	void Update () {
//		if (Input.anyKeyDown) {
//			if (Input.GetKeyDown (KeyCode.Return)) {
//				sentence.GetComponent<Text>().text = "d";
//			}
//			else {
//				//sentence.GetComponent<Text>().text = sentence.GetComponent<Text>().text + Input.inputString.ToString();
//				foreach(char c in Input.inputString) {
//					sentence.GetComponent<Text>().text = sentence.GetComponent<Text>().text + Input.inputString.ToString();
//					Debug.Log (c);
//				}
//				//Debug.Log(Input.inputString);
//				//sentence.GetComponent<Text>().text = "dog";
//			}
//		}
		foreach (char c in Input.inputString) {
			if (c == "\b"[0]){
				if (sentence.GetComponent<Text>().text.Length != 0)
					sentence.GetComponent<Text>().text = sentence.GetComponent<Text>().text.Substring(0, sentence.GetComponent<Text>().text.Length - 1);
			}
			if (c == "\n"[0] || c == "\r"[0]){
				sentence.GetComponent<Text>().text = "";
			}
			else
				sentence.GetComponent<Text>().text += c;
		}
	}
}
