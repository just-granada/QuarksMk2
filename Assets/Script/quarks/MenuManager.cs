using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
	public GameObject play;
	public GameObject instructions;
	public GameObject credits;
	public GameObject bgBubblePrefab;
	public int totalBgBubbles;
	public float x0;
	public float y0;
	public float width;
	public float height;
	
	// Use this for initialization
	void Start () {
		createBgBubbles();
	}
	
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
			Debug.Log("play clicked");
			Application.LoadLevel("GameScene");
		}
		else if(gesture.pickObject == instructions)
		{
			Debug.Log("instructions clicked");
			Application.LoadLevel("Instructions");
		}
		else if(gesture.pickObject == credits)
		{
			Debug.Log("credits clicked");
			Application.LoadLevel("Credits");
		}
	}
	
	private void createBgBubbles()
	{
		int i;
		if(GameObject.FindGameObjectsWithTag("bgBubble").Length ==0)
		{
			for(i=0;i<totalBgBubbles;i++)
			{
				Instantiate(bgBubblePrefab,new Vector3(Random.Range(x0,x0+width),Random.Range(y0,y0+height),-0.4f), Quaternion.identity);
			}
		}
	}
}
