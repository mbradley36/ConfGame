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
