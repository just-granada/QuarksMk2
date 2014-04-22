using UnityEngine;
using System.Collections;

public class quarkScript : MonoBehaviour {
	public int quarkType;
	public tk2dSprite sprite;
	public tk2dSprite halo;
	public bool activated;
	public float transitionTime =0.6f;
	public float activeOffset = -0.2f;
	public float scaleChange = 0.1f;
	public GameObject sparkEmmiter;
	
	private float transitionStartTime;
	private float transitionStartingScale;
	private float startingZ;
	private float startingScale;
	private float transitionStartingZ;
	private bool inTransition = false;
	
	private static Color redQuark = new Color(1f,0f,0f,0.5f);
	private static Color greenQuark = new Color(0f,1f,0f,0.5f);
	private static Color blueQuark = new Color(0f,0f,1f,0.5f);
	private static Color yellowQuark = new Color(0.7f,1f,0f,0.5f);
	private static Color purpleQuark = new Color(0.8f,0f,1f,0.5f);
	private static Color orangeQuark = new Color(1f,0.6f,0f,0.5f);
	
	private Color[] haloColors = {redQuark, blueQuark, yellowQuark,greenQuark,purpleQuark,orangeQuark};
	
	private float timeLeft;
	
	// Use this for initialization
	void Start () {
		randomizeType();
		startingZ = transform.position.z;
		startingScale = transform.localScale.x;
	}
	
	private void randomizeType()
	{
		quarkType = Mathf.CeilToInt(Random.value*5.99f);
		sprite.spriteId = sprite.GetSpriteIdByName("Quark"+quarkType);
		halo.color = haloColors[quarkType-1];
		halo.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		float newZ=0f;
		if(inTransition)
		{
			if(activated)
			{
				if(Time.time-transitionStartTime < transitionTime)
				{
					newZ = Mathf.SmoothStep(transitionStartingZ, startingZ+activeOffset, (Time.time-transitionStartTime)/transitionTime);
					transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
					transform.localScale = Vector3.one * Mathf.Lerp(transitionStartingScale, startingScale+scaleChange, (Time.time-transitionStartTime)/transitionTime);
				}
				else
				{
					inTransition = false;
					transform.position = new Vector3(transform.position.x, transform.position.y, startingZ+activeOffset);
					transform.localScale = Vector3.one * (startingScale+scaleChange);
				}
			}
			else
			{
				if(Time.time-transitionStartTime < transitionTime)
				{
					newZ = Mathf.SmoothStep(transitionStartingZ, startingZ, (Time.time-transitionStartTime)/transitionTime);
					transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
					transform.localScale = Vector3.one * Mathf.Lerp(transitionStartingScale, startingScale, (Time.time-transitionStartTime)/transitionTime);
				}
				else
				{
					inTransition = false;
					transform.position = new Vector3(transform.position.x, transform.position.y, startingZ);
					transform.localScale = Vector3.one * startingScale;
				}
			}
		}
			
			
		
		if(activated)
		{
			timeLeft -= Time.deltaTime;
			halo.scale = Vector3.one * (Mathf.Clamp(timeLeft,0,2)+0.12f);
		}
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
		if(gesture.pickObject == gameObject)
		{
			Debug.Log( "Touch in " + gesture.position);
			GameManager.Instance.quarkClicked(this);
		}
	}
	
	public void activate(bool activated, float timeout)
	{
		this.activated = activated;
		transitionStartTime = Time.time;
		transitionStartingZ = transform.position.z;
		transitionStartingScale = transform.localScale.x;
		timeLeft = timeout;
		halo.gameObject.SetActive(activated);
		inTransition=true;
	}
	
	public void resetQuark()
	{
		emitSparks();
		
		transform.Translate(0,0,-10);
		transitionStartTime = Time.time;
		activated=false;
		transitionStartingZ = transform.position.z;
		transitionStartingScale = transform.localScale.x;
		randomizeType();
		inTransition = true;
	}
	
	private void emitSparks()
	{
		GameObject newSparkEmmiter = (GameObject)Instantiate(sparkEmmiter, transform.position, Quaternion.identity);
		newSparkEmmiter.GetComponent<ParticleSystem>().startColor = haloColors[quarkType-1];
	}
}
