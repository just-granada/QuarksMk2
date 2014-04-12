using UnityEngine;
using System.Collections;

public class BrownianMotion : MonoBehaviour {
	public float minForce;
	public float maxForce;
	public float pushProbability;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Random.value < pushProbability)
		{
			Vector3 direction = Random.onUnitSphere;
			float magnitude= Random.Range(minForce,maxForce);
			rigidbody.AddForce(direction*magnitude);
		}
	}
}
