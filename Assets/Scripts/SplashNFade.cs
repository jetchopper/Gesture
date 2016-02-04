using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashNFade : MonoBehaviour {

	public AudioSource passed, error;

	private Image image;
	private bool doneDrawing;

	void Awake () 
	{
		image = GetComponent<Image>();
		image.enabled = false;
	}

	void Update () 
	{
		if (doneDrawing)
		{
			image.color -= Color.black * Time.deltaTime * 2;
			doneDrawing = image.color.a <= 0 ? false : true;
		}
	}

	public void SetSplash(bool correct)
	{
		//guessed or not check
		if (correct)
		{
			image.color = Color.green;
			passed.Play();
		}
		else
		{
			image.color = Color.red;
			error.Play();
		}
		doneDrawing = true;
		image.enabled = true;
	}
}
