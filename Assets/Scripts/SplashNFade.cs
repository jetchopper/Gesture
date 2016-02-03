using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashNFade : MonoBehaviour {

	private Image image;
	private bool correct, doneDrawing;

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

	public void SetSplash(bool b)
	{
		correct = b;
		doneDrawing = true;
		image.color = correct ? Color.green : Color.red;
		image.enabled = true;
	}
}
