using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;

public class DictionaryManager : MonoBehaviour {
	Dictionary<string, int> positiveDictionary = new Dictionary<string, int>();

	// Use this for initialization
	void Start () {
		string file = Application.dataPath + "/Resources/positiveWordList.txt";
		Debug.Log (file);
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
								positiveDictionary.Add(positiveWord[i], 0);
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
	
	// Update is called once per frame
	void Update () {
	
	}
}
