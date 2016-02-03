using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using PDollarGestureRecognizer;

public class Panel : MonoBehaviour {

	public LineRenderer lineRenderer;
	public float recognitionTreshold, startTimer, timerDecreaseStepPerLevel;
	public GameObject end;

	private SplashNFade splashNFade;
	private GestureRecognizer gestureRecognizer;
	private Gesture randGesture;
	private bool newFigure;
	private float timer;
	private Text timeText, scoreText;
	private int totalScore;
	
	void Start () 
	{
		end.SetActive(false);
		gestureRecognizer = FindObjectOfType(typeof(GestureRecognizer)) as GestureRecognizer;
		splashNFade = FindObjectOfType(typeof(SplashNFade)) as SplashNFade;
		newFigure = true;
		lineRenderer = Instantiate(lineRenderer);
		timer = startTimer;
		timeText = gameObject.GetComponentsInChildren<Text>()[0];
		scoreText = gameObject.GetComponentsInChildren<Text>()[1];
		totalScore = 0;
		scoreText.text = "SCORE: 0";
	}

	void Update () 
	{
		timeText.text = "" + (int)timer;
		timer -= Time.deltaTime;
		if (newFigure)
		{
			//decreasing timer per level
			startTimer -= timerDecreaseStepPerLevel;
			//refresh line
			lineRenderer.SetVertexCount(0);
			//get random gesture
			randGesture = gestureRecognizer.GetRandomGesture();
			//create it with lines
			lineRenderer.SetVertexCount(randGesture.Points.Length);
			for (int i = 0; i < randGesture.Points.Length; i++)
			{
				lineRenderer.SetPosition(i, new Vector3(randGesture.Points[i].X + 2f, -randGesture.Points[i].Y, -7f));
			}
			newFigure = false;
		}
		if ((int)timer <= 0)
		{
			timeText.text = "";
			scoreText.text = "";
			gestureRecognizer.StopGame();
			end.SetActive(true);
			end.GetComponent<EndWindow>().SetScore(totalScore);
		}
		if (Input.GetKey("escape"))
		{
			Application.Quit();
		}
	}

	public void SetResult(string s, float f)
	{
		if (randGesture.Name.Equals(s) && f > recognitionTreshold)
		{
			newFigure = true;
			timer = startTimer;
			splashNFade.SetSplash(true);
			totalScore++;
			scoreText.text = "SCORE: " + totalScore;
		}
		else
		{
			splashNFade.SetSplash(false);
		}
	}
}
