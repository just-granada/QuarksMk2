using UnityEngine;
using System.Collections;

public class ScoreLoader : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
		Debug.Log("attempting www call");
		WWW www = new WWW("http://www.dantadigital.com/Quarks/getScores.php");
   		Debug.Log("WWW call made");
		yield return www;
        Debug.Log("call result" + www.text);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
