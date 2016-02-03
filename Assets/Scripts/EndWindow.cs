using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndWindow : MonoBehaviour {

	public void SetScore(int score) 
	{
		GetComponentInChildren<Text>().text = "TOTAL SCORE: " + score;
	}

	public void TryAgain()
	{
		Application.LoadLevel(1);
	}
}
