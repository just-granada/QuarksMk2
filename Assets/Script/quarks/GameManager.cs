using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public GameObject quarkPrefab;
	public int columns;
	public int rows;
	public float x0;
	public float y0;
	public float width;
	public float height;
	public tk2dTextMesh scoreDisplay;
	public tk2dTextMesh lastComboDisplay;
	public tk2dTextMesh timerTextDisplay;
	public AudioClip[] quarkSounds;
	public AudioClip[] comboSounds;
	public float gameTimer;
	public GameObject timerDisplay;
	
	private static GameManager instance;
	
	private float pieceLineWidth;
	private float pieceLineHeight;
	private List<quarkScript> currentCombo;
	
	private float comboTimeout;
	private int multiplier=1;
	private int playerScore;
	private float maxTime;
	private float timerAngleRange =  130;//2.2689f;
	private float timerMaxAngle =  155;//2.7053f;
	private float timerMinAngle = 25;//0.4363f;

	public static GameManager Instance {
		get {
			return instance;
		}
		set {
			instance = value;
		}
	}
	
	// Use this for initialization
	void Start () {
		instance = this;
		createQuarks(columns,rows,x0,y0,width,height);
		currentCombo = new List<quarkScript>();
		pieceLineWidth = width/columns;
		pieceLineHeight = height/rows;
		maxTime = gameTimer;
	}
	
	// Update is called once per frame
	void Update () {
		comboTimeout -= Time.deltaTime;
		gameTimer -= Time.deltaTime;
		updateTimerDisplay();
		if(comboTimeout <= 0 && currentCombo.Count > 1)
		{
			clearCurrentCombo();
		}
		if(gameTimer <=0)
		{
			gameOver();
		}
	}
	
	private void updateTimerDisplay()
	{
		float desiredAngle =  timerMaxAngle - gameTimer/maxTime *  timerAngleRange;
		float angleDelta = desiredAngle - timerDisplay.transform.rotation.z;
		timerDisplay.transform.eulerAngles = new Vector3(0,0,desiredAngle);
		timerTextDisplay.text = (Mathf.Round(gameTimer)).ToString();
		timerTextDisplay.Commit();
	}
	
	private void createQuarks(int columns, int rows, float x0, float y0, float width, float height)
	{
		int i;
		int j;
		quarkScript newQuark;
		
		for(j=0;j<rows;j++)
		{
			for(i=0;i<columns;i++)
			{
				newQuark = (Instantiate(quarkPrefab, new Vector3(x0+i*width/(columns-1), y0-j*height/(rows-1), -0.5f),Quaternion.identity) as GameObject).GetComponent<quarkScript>() as quarkScript;
			}
		}
		
	}
	
	public void quarkClicked(quarkScript quark)
	{
		if(currentCombo.Count==0 || isQuarkValidForCombo(quark))
		{
			quarkAddedToCombo();
		}
		else
		{
			clearCurrentCombo();
		}
		currentCombo.Add(quark);
		comboTimeout = 0.5f + 1f/(currentCombo.Count);
		quark.activate(true,0.5f + 1f/(currentCombo.Count));
	}
	
	private void quarkAddedToCombo()
	{
		audio.PlayOneShot(quarkSounds[Mathf.Clamp(currentCombo.Count,0,quarkSounds.Length-1)]);
	}
	
	private bool isQuarkValidForCombo(quarkScript quark)
	{
		bool isSameColor = quark.quarkType == currentCombo[0].quarkType;
		bool isInVerticalLineWithLast = Mathf.Abs(quark.transform.position.x - currentCombo[currentCombo.Count-1].transform.position.x) < pieceLineWidth;
		bool isInHoriontalLineWithLast = Mathf.Abs(quark.transform.position.y - currentCombo[currentCombo.Count-1].transform.position.y) < pieceLineHeight;
		return isSameColor && (isInHoriontalLineWithLast || isInVerticalLineWithLast) && !currentCombo.Contains(quark);
	}
	
	private void clearCurrentCombo()
	{
		if(currentCombo.Count >= 3) // a valid combo was made
		{
			int previousScore = playerScore;
			float scoreDelta;
			Debug.Log("Valid combo made");
			foreach(quarkScript quark in currentCombo)
			{
				quark.resetQuark();
			}
			playerScore += calculateComboScore();
			scoreDelta = (playerScore - previousScore +0f)/(playerScore +0f);
			multiplier++;
			scoreDisplay.text = ""+playerScore;
			scoreDisplay.Commit();
			audio.PlayOneShot(comboSounds[Mathf.Clamp(currentCombo.Count-3,0,comboSounds.Length-1)]);
			
			gameTimer = Mathf.Min (maxTime, gameTimer + maxTime*scoreDelta);
		}
		else // invalid combo
		{
			Debug.Log("Invalid combo, BOOOOOO ");
			foreach(quarkScript quark in currentCombo)
			{
				quark.activate(false,0);
			}
			lastComboDisplay.text = "0 X 0";
			lastComboDisplay.Commit();
			multiplier=1;
		}
		currentCombo.Clear();
	}
	
	private int calculateComboScore()
	{
		int score = (10 * Mathf.RoundToInt(Mathf.Pow(2,currentCombo.Count-3)));
		Debug.Log("combo score=" + score +" multiplier: "+multiplier);
		lastComboDisplay.text = score.ToString() + " X " + multiplier.ToString();
		lastComboDisplay.Commit();
		return score*multiplier;
	}
	
	private void saveNewScore(int score)
	{
		int index=0;
		JSONObject jsonScores;
		if(PlayerPrefs.HasKey("localHighScores"))
		{
			jsonScores = new JSONObject(PlayerPrefs.GetString("localHighScores"));
		}
		else
		{
			jsonScores = new JSONObject(JSONObject.Type.ARRAY);
		}
		
		if(jsonScores.list.Count > 0)
		{
			while(index < jsonScores.list.Count && index < 10 && jsonScores[index].n > score)
			{
				index++;
			}
		}
		if(index<jsonScores.list.Count)
			jsonScores.list.Insert(index, new JSONObject(score));
		else if(index<10)
			jsonScores.Add(new JSONObject(score));
		PlayerPrefs.SetString("localHighScores", jsonScores.print());
		PlayerPrefs.SetString("lastLocalScore", score.ToString());
	}
	
	private void gameOver()
	{
		saveNewScore(playerScore);
		Application.LoadLevel("gameOver");
	}
}