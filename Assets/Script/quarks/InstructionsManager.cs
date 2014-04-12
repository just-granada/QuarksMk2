using UnityEngine;
using System.Collections;

public class InstructionsManager : MonoBehaviour {
	
	public GameObject play;
	
	// Subscribe to events
	void OnEnable(){
		EasyTouch.On_TouchStart += On_TouchStart;
	}
	// Unsubscribe
	void OnDisable(){
		EasyTouch.On_TouchStart -= On_TouchStart;
	}
	// Unsubscribe
	void OnDestroy(){
		EasyTouch.On_TouchStart -= On_TouchStart;
	}
	// Touch start event
	public void On_TouchStart(Gesture gesture){
		if(gesture.pickObject == play)
		{
			Application.LoadLevel("GameScene");
		}
	}
}
