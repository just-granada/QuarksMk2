using UnityEngine;
using System.Collections;

public class CreditsManager : MonoBehaviour {
	
	public GameObject back;
	public GameObject YorchLink;
	
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
		if(gesture.pickObject == back)
		{
			Application.LoadLevel("Splash");
		}
		else if(gesture.pickObject == YorchLink)
		{
			Application.OpenURL("http://Interactiva.grupoap.com");
		}
	}
}
