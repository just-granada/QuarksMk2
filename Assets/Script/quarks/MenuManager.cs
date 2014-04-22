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

	public tk2dTextMesh versionDisplay;

	// Use this for initialization
	void Awake () {
		// Initialize FB SDK              
		enabled = false;                  
		FB.Init(SetInit, OnHideUnity);  
	}

	void Start () {
		createBgBubbles();

		versionDisplay.text = "0.003";
		versionDisplay.Commit ();
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

	private void SetInit()                                                                       
	{                                                                                            
		Debug.Log("SetInit");                                                                  
		enabled = true; // "enabled" is a property inherited from MonoBehaviour                  
		if (FB.IsLoggedIn)                                                                       
		{                                                                                        
			Debug.Log("Already logged in");                                                    
		}   
		if (!FB.IsLoggedIn)                                                                                              
		{                                                                                                                
			FB.Login("publish_actions", LoginCallback);                                                                                                                                                      
		}  
	}

	private void OnHideUnity(bool isGameShown)                                                   
	{                                                                                            
		Debug.Log("OnHideUnity");                                                              
		if (!isGameShown)                                                                        
		{                                                                                        
			// pause the game - we will need to hide                                             
			Time.timeScale = 0;                                                                  
		}                                                                                        
		else                                                                                     
		{                                                                                        
			// start the game back up - we're getting focus again                                
			Time.timeScale = 1;                                                                  
		}                                                                                        
	}   

	void LoginCallback(FBResult result)                                                        
	{                                                                                          
		Debug.Log("LoginCallback");                                                       
		
		if (FB.IsLoggedIn)                                                                     
		{                                                                                      
			OnLoggedIn();                                                                      
		}                                                                                      
	}                                                                                          
	
	void OnLoggedIn()                                                                          
	{                                                                                          
		Debug.Log("Logged in. ID: " + FB.UserId);                                            
	}
}
