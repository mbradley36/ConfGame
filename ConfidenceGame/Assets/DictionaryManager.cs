using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;

public class DictionaryManager : MonoBehaviour {
	public static DictionaryManager instance{get; private set;}
	private Dictionary<string, int> positiveDictionary = new Dictionary<string, int>();
	private Dictionary<string, int> multiplierDictionary = new Dictionary<string, int>();
	private Dictionary<string, int> negativeDictionary = new Dictionary<string, int>();
	public int startingPointVal = 4;
	public GameObject feedbackObj;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		string file = Application.dataPath + "/Resources/positiveWordList.txt";
		string file2 = Application.dataPath + "/Resources/multiplierList.txt";
		string file3 = Application.dataPath + "/Resources/negativeWordList.txt";
		FillDictionary (file, positiveDictionary);
		FillDictionary (file2, multiplierDictionary);
		FillDictionary (file3, negativeDictionary);
	}

	void FillDictionary(string file, Dictionary<string, int> d){
		try {
			string line;
			StreamReader reader = new StreamReader(file, Encoding.Default);
			
			using(reader){
				do {
					line = reader.ReadLine ();
					if(line != null) {
						string[] positiveWord = line.Split(' ');
						if(positiveWord.Length > 0) {
							for(int i = 0; i < positiveWord.Length; i++){
								if(!d.ContainsKey(positiveWord[i])) d.Add(positiveWord[i], startingPointVal);
							}
						}
					}
				} while (line != null);
			}
			
			reader.Close();
		} catch (IOException e) {
			Debug.Log ("couldn't read positive word file");
		}
	}
	
	public int CheckDictionary(string sentence){
		string[] words = sentence.Split (' ');
		bool negative = false;
		//Debug.Log (words.Length);
		bool multiplierActive = false;
		int points = 0;
		if(words.Length > 1) {
			for(int i = 0; i < words.Length; i++) {
				if(negativeDictionary.ContainsKey(words[i])) {
					return 0;
				}
				if(positiveDictionary.ContainsKey(words[i])) {
					int worth = positiveDictionary[words[i]];
					points += worth;
					if(worth > 0) positiveDictionary[words[i]] = worth-1;
				}
				if(!multiplierActive && multiplierDictionary.ContainsKey(words[i])) {
					if(multiplierDictionary[words[i]]!=0) {
						multiplierDictionary[words[i]] = 0;
						multiplierActive = true;
					}
				}
			}
		}
		if(words.Length > 7) multiplierActive = true;
		if(multiplierActive) points *= startingPointVal;
		StartCoroutine("MultiplierFeedback");
		//Debug.Log ("points worth: " + points);
		return points;
	}

	IEnumerator MultiplierFeedback(){
		Debug.Log ("multiplier feedback");
		GameObject g = GameObject.Instantiate (feedbackObj) as GameObject;
		RectTransform r = g.GetComponent<RectTransform> ();
		float timer = Time.time + 0.5f;
		Vector2 modifiedPos = r.anchoredPosition;
		while(Time.time < timer) {
			Debug.Log (r.anchoredPosition);
			modifiedPos.y += 1.5f;
			r.anchoredPosition = modifiedPos;
			yield return new WaitForSeconds(0.001f);
		}
		GameObject.Destroy (g);
		yield return new WaitForSeconds (0f);
	}
}
