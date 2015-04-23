using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class textFieldHandler : MonoBehaviour {
	public static textFieldHandler instance { get; private set;}

	public InputField sentence = null;
	private int worth = 0;
	private string typed;

	void Awake(){
		instance = this;
	}
	
	// Use this for initialization
	void Start () {
		//setup for the fuction that will return the input field string
		InputField.SubmitEvent submitEvent = new InputField.SubmitEvent ();
		submitEvent.AddListener (saveString);
		sentence.onEndEdit = submitEvent;
		sentence.Select ();
		sentence.ActivateInputField ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	//function that is called when enter is pressed after typing something into input field sentence
	//the string it takes in is what the user typed
	void saveString(string _typed){
		Debug.Log (_typed);
		typed = _typed;
		sentence.Select ();
		sentence.ActivateInputField ();

	}

	public int GetWorth(){
		int worth = DictionaryManager.instance.CheckDictionary (typed);
		return worth;
	}

}
