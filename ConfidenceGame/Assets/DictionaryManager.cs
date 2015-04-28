using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;

public class DictionaryManager : MonoBehaviour {
	public static DictionaryManager instance{get; private set;}
	private Dictionary<string, int> positiveDictionary = new Dictionary<string, int>();
	private Dictionary<string, int> multiplierDictionary = new Dictionary<string, int>();
	public int startingPointVal = 4;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		string file = Application.dataPath + "/Resources/positiveWordList.txt";
		string file2 = Application.dataPath + "/Resources/multiplierList.txt";
		FillDictionary (file, positiveDictionary);
		FillDictionary (file2, multiplierDictionary);
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
								d.Add(positiveWord[i], startingPointVal);
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
		//Debug.Log (words.Length);
		ArrayList multiples = new ArrayList ();
		int points = 0;
		if(words.Length > 1) {
			for(int i = 0; i < words.Length; i++) {
				if(positiveDictionary.ContainsKey(words[i])) {
					int worth = positiveDictionary[words[i]];
					points += worth;
					if(worth > 0) positiveDictionary[words[i]] = worth-1;
				}
				if(multiplierDictionary.ContainsKey(words[i]) {
					multiples.Add(startingPointVal);
				}
			}
		}
		foreach(int i in multiples) {
					points *= i;
		}
		//Debug.Log ("points worth: " + points);
		return points;
	}
}
