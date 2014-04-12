using UnityEngine;
using System.Collections;

public class BGBubble : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.localScale *= Random.Range(0.2f,1.5f);
		DontDestroyOnLoad(this);
	}
	
}
