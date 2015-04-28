using UnityEngine;
using System.Collections;

public class cloud_handler : MonoBehaviour {

	public Transform[] cloudPrefabs;
	Transform test, test2, test3, test4;
	GameObject[] currClouds;

	IEnumerator makeClouds() 
	{	
		while (true) {
			yield return new WaitForSeconds (2.5f);
			//Debug.Log ("This is running");
			int random = (int)Random.Range (0f, 3f);
			float randomZ = Random.Range (3.4f, 18.12f);
			float randomY = Random.Range (-11f, 13.6f);
			float randomX;
			float x = Random.value;
			if (x >= .5f)
				randomX = 20.5f;
			else
				randomX = -20.5f;
			Vector3 start = new Vector3(randomX, randomY, randomZ);
			//Transform cloud;
			Instantiate (cloudPrefabs [random], start, Quaternion.identity);
		//	Debug.Log(cloud.GetComponent<assignDirection>().direction);
//			if (cloud.GetComponent<assignDirection>().direction == 1){
//				cloud.transform.position = new Vector3(-26.5f, randomY, randomZ);
//			}
		}
	}

	// Use this for initialization
	void Start () 
	{
//		test  = Instantiate (cloudPrefabs[0]) as Transform;
//		test2 = Instantiate (cloudPrefabs[1]) as Transform;
//		test3 = Instantiate (cloudPrefabs[2]) as Transform;
//		test4 = Instantiate (cloudPrefabs[3]) as Transform;
		StartCoroutine(makeClouds());
		//Destroy (test, 4f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//StartCoroutine(makeClouds());
		currClouds = GameObject.FindGameObjectsWithTag("cloud");
		foreach (GameObject cloud in currClouds) 
		{
			float y = cloud.GetComponent<assignDirection> ().direction;
			cloud.GetComponent<Transform>().Translate (Vector3.right * y * Time.deltaTime);
//			if (cloud.transform.position.x > 26.5)
//				DestroyImmediate(cloud);
//			if (cloud.transform.position.x < -26.5)
//				DestroyImmediate(cloud);
		}
	}


}
