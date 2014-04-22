using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GameOverManager : MonoBehaviour {
	public tk2dTextMesh highScoresTitle;
	public tk2dTextMesh highScoresNames;
	public tk2dTextMesh highScoreValues;
	public tk2dTextMesh lastScore;
	public tk2dTextMesh playAgain;
	public tk2dTextMesh submitButton;
	public tk2dTextMesh infoText;
	public GameObject playerScoreContainer;
	public tk2dUITextInput nameField;
	
	private WWW scoreLoadObject;
	private WWW scoreSubmitObject;
	private List<WWW> activeCalls;
	private static string scoreSubmitURL = "http://www.dantadigital.com/Quarks/submit.php?data=";
	private static string FBsubmitURL = "https://graph.facebook.com/";
	
	private int sanitizeCounter=0;
	
	private float transitionTime=0.3f;
	private float transitionStart;
	private float startingPosition;
	private bool inTransition;
	
	//private 
	// Use this for initialization
	void Start () {
		lastScore.text += PlayerPrefs.GetString("lastLocalScore");
		lastScore.Commit();
		nameField.Text = PlayerPrefs.GetString("lastPlayerName");
		nameField.SetFocus();
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
		if(gesture.pickObject == playAgain.gameObject)
		{
			Application.LoadLevel("GameScene");
		}
		else if(gesture.pickObject == submitButton.gameObject)
		{
			submitScore();
		}
	}
	
	void Update()
	{
		/*List<WWW> readyCalls = new List<WWW>();
		if(activeCalls!=null && activeCalls.Count > 0)
		{
			foreach(WWW activeCall in activeCalls)
			{		
				if(activeCall.isDone)
				{
					readyCalls.Add(activeCall);
				}
			}
			foreach(WWW activeCall in readyCalls)
			{
				activeCalls.Remove(activeCall);
				manageResponse(activeCall);
			}
		}*/
		
		
		if(sanitizeCounter==5)
		{
			sanitizeCounter = 0;
			sanitizeName();
		}
		sanitizeCounter++;
		
		if(inTransition)
		{
			if(Time.time-transitionStart > transitionTime)
			{
				playerScoreContainer.transform.position = new Vector3(playerScoreContainer.transform.position.x,0.15f,playerScoreContainer.transform.position.z);
				highScoresTitle.color = new Color(highScoresTitle.color.r, highScoresTitle.color.g, highScoresTitle.color.b, 1);
				highScoresNames.color = new Color(highScoresTitle.color.r, highScoresTitle.color.g, highScoresTitle.color.b, 1);
				highScoreValues.color = new Color(highScoresTitle.color.r, highScoresTitle.color.g, highScoresTitle.color.b, 1);
				playAgain.color = new Color(highScoresTitle.color.r, highScoresTitle.color.g, highScoresTitle.color.b, 1);
				highScoresTitle.Commit();
				highScoresNames.Commit();
				highScoreValues.Commit();
				playAgain.Commit();
				inTransition = false;
			}
			else
			{
				float newY = Mathf.Lerp(startingPosition, 0.15f,(Time.time - transitionStart)/transitionTime);
				float newA = Mathf.Lerp(0, 1,(Time.time - transitionStart)/transitionTime);
				playerScoreContainer.transform.position = new Vector3(playerScoreContainer.transform.position.x,newY,playerScoreContainer.transform.position.z);
				highScoresTitle.color = new Color(highScoresTitle.color.r, highScoresTitle.color.g, highScoresTitle.color.b, newA);
				highScoresNames.color = new Color(highScoresTitle.color.r, highScoresTitle.color.g, highScoresTitle.color.b, newA-0.2f);
				highScoreValues.color = new Color(highScoresTitle.color.r, highScoresTitle.color.g, highScoresTitle.color.b, newA-0.2f);
				playAgain.color = new Color(highScoresTitle.color.r, highScoresTitle.color.g, highScoresTitle.color.b, newA);
				highScoresTitle.Commit();
				highScoresNames.Commit();
				highScoreValues.Commit();
				playAgain.Commit();
			}
		}
	}
	
	private void submitScore()
	{
		submitButton.text = "SENDING...";
		submitButton.Commit();
		nameField.gameObject.SetActive(false);

		string constructedFBsubmitURL = FBsubmitURL + FB.UserId + "/scores?score=";
		constructedFBsubmitURL += (int.Parse(PlayerPrefs.GetString("lastLocalScore")) + "&access_token=");
		constructedFBsubmitURL += (FB.AccessToken);
		
		PlayerPrefs.SetString("lastPlayerName",nameField.Text);
		JSONObject score = new JSONObject(JSONObject.Type.OBJECT);
		score.AddField("name",nameField.Text);
		score.AddField("score",int.Parse(PlayerPrefs.GetString("lastLocalScore")));
		//Debug.Log("string pre-encrypt: "+score.print());
		//string encryptedScore = encryptString(score.print(),2,false);
		//Debug.Log("string post-encrypt: " + encryptedScore);
		//string encodedScore = WWW.EscapeURL(encryptedScore);
		//Debug.Log("string post-encode: " + encodedScore);
		//scoreSubmitObject = new WWW(scoreSubmitURL+encodedScore);
		infoText.text = "ATTEMPTING FB CALL";
		infoText.Commit();
		if (FB.IsLoggedIn)
		{
			infoText.text = "FB PUBLISH CALL";
			infoText.Commit();
			var query = new Dictionary<string, string>();
			query["score"] = PlayerPrefs.GetString("lastLocalScore");
			FB.API("/me/scores", Facebook.HttpMethod.POST, manageScorePostResponse, query);
		}   

		/*if(activeCalls==null)
			activeCalls=new List<WWW>();
		activeCalls.Add(scoreSubmitObject);*/
	}
	
	private string encryptString(string text, byte displacement, bool decrypt)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		var i=0;
		for(i=0;i<bytes.Length;i++)
		{
			if(!decrypt)
				bytes[i]+=displacement;
			else
				bytes[i]-=displacement;
		}
		return Encoding.UTF8.GetString(bytes);
	}
	
	void loadGobalHighScores()
	{
		scoreLoadObject = new WWW("http://www.dantadigital.com/Quarks/getScores.php");
		if(activeCalls==null)
			activeCalls = new List<WWW>();
		activeCalls.Add(scoreLoadObject);
	}
	
	void manageResponse(WWW call)
	{
		if(call == scoreLoadObject)
		{
			Debug.Log("Loaded Scores: "+call.text);
			JSONObject jsonScores = new JSONObject(call.text);
			displayHighScores(jsonScores);
		}
		else if(call == scoreSubmitObject)
		{
			Debug.Log("Score Submitted: "+call.text);
			loadGobalHighScores();
		}
	}
	
	private void sanitizeName()
	{
		string name = nameField.Text;
		name=name.Replace((string)",","");
		name=name.Replace("" + '"',"");
		name=name.Replace("'","");
		name=name.Replace("<","");
		name=name.Replace(">","");
		name=name.Replace("!","");
		name=name.Replace("*","");
		name=name.Replace("(","");
		name=name.Replace(")","");
		name=name.Replace("[","");
		name=name.Replace("]","");
		name=name.Replace("?","");
		name=name.Replace("/","");
		name=name.Replace("\\","");
		name=name.Replace("^","");
		name=name.Replace("#","");
		name=name.Replace("$","");
		name=name.Replace("%","");
		name=name.Replace("&","");
		name=name.Replace("|","");
		nameField.Text = name;
	}
	
	private void displayHighScores(JSONObject jsonScores)
	{
		highScoresNames.text = "";
		highScoreValues.text = "";
		int count =0;
		foreach(JSONObject score in jsonScores.list)
		{
			highScoresNames.text += score.GetField("name").str.ToUpper() + ": " + "\n";
			
			highScoreValues.text += score.GetField("score").n + "\n";
			count++;
			if(count>=10)
				break;
		}
		highScoresNames.Commit();
		highScoreValues.Commit();
		inTransition = true;
		transitionStart = Time.time;
		startingPosition = playerScoreContainer.transform.position.y;
		nameField.gameObject.SetActive(false);
		submitButton.gameObject.SetActive(false);
	}

	private void manageScorePostResponse(FBResult result)
	{
		infoText.text = "RESULT" + result.Text;
		infoText.Commit();
		readScores ();
	}

	private void readScores()
	{
		infoText.text = "FB SCORE CALL";
		infoText.Commit();
		var query = new Dictionary<string, string>();
		query["score"] = PlayerPrefs.GetString("lastLocalScore");
		Debug.Log (query ["score"]);
		FB.API("/app/scores", Facebook.HttpMethod.GET, manageScoreReadResponse, query);
	}

	private void manageScoreReadResponse(FBResult result)
	{
		infoText.text = "RESULT" + result.Text;
		infoText.Commit();
	}
}

